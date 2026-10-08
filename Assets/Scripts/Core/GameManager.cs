using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Liên kết")]
    [SerializeField] Board board;
    [SerializeField] Shooter shooter;
    [SerializeField] ShotQueue queue;
    [SerializeField] ReactionTable table;
    [SerializeField] HUD hud;

    [Header("Luật chơi")]
    [SerializeField] int startRows = 4;
    [SerializeField] int movesPerDrop = 3;   // [UI] Cứ N lượt thì bảng xuống 1 hàng
    [SerializeField] int maxMoves = 30;      // [UI] Hết lượt mà chưa clear = THUA
    [SerializeField] int totalBalls = 20;    // [UI] Tổng số bóng xuất hiện trong màn

    int moves, score, spawned, cleared;
    bool over;

    void Start()
    {
        for (int r = 0; r < startRows; r++)
            spawned += board.FillRow(r, table.RandomIon, totalBalls - spawned);

        shooter.BallLanded += OnBallLanded;
        queue.Changed += () => hud.ShowQueue(queue.Next);

        hud.ShowQueue(queue.Next);
        RefreshHUD();
    }

    void RefreshHUD() => hud.Refresh(moves, maxMoves, score, cleared, totalBalls);

    void OnBallLanded(Ball placed)
    {
        if (over) return;

        moves++;
        ResolveReactions(placed);

        if (moves % movesPerDrop == 0 && spawned < totalBalls)
            DropRow();

        RefreshHUD();
        CheckEnd();
    }

    void ResolveReactions(Ball placed)
    {
        var matched = new List<Ball>();
        int gained = 0;
        string lastName = "";

        foreach (var c in board.Neighbours(placed.Cell))
        {
            if (!board.TryGet(c, out Ball other)) continue;

            ReactionRule rule = table.Find(placed.Data, other.Data);
            if (rule == null) continue;

            matched.Add(other);
            gained += rule.points;
            lastName = rule.precipitate;
        }

        if (matched.Count == 0) return;

        matched.Add(placed);
        foreach (var b in matched) board.Remove(b);

        score += gained;
        cleared += matched.Count;
        hud.ShowPrecipitate(lastName, gained);
    }

    void DropRow()
    {
        board.Descend();
        spawned += board.FillRow(0, table.RandomIon, totalBalls - spawned);
    }

    void CheckEnd()
    {
        if (board.ReachedDeadLine())
            EndGame(false);                          // Chạm vạch đỏ
        else if (spawned >= totalBalls && board.IsEmpty)
            EndGame(true);                           // Xoá hết bóng
        else if (moves >= maxMoves)
            EndGame(false);                          // Hết lượt
    }

    void EndGame(bool win)
    {
        over = true;
        shooter.enabled = false;
        hud.ShowResult(win, score, moves);
    }

    // ---------- Gọi từ UI Button ----------
    public void OnSwapPressed()
    {
        if (!over && !shooter.Busy) queue.SwapWithNext();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}