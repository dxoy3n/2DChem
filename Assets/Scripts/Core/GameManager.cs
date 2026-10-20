using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Liên kết")]
    [SerializeField] Board board;
    [SerializeField] Shooter shooter;
    [SerializeField] ShotQueue queue;
    [SerializeField] HUD hud;
    [SerializeField] Level defaultLevel;   // Chỉ dùng khi bấm Play trực tiếp trong scene Game

    Level level;
    ReactionTable table;
    int moves, score, cleared;
    bool over;

    void Awake()
    {
        Time.timeScale = 1f;
        level = Session.Current != null ? Session.Current : defaultLevel;
        table = level.table;
        queue.Init(level.queueBalls, RandomIon);
    }

    void Start()
    {
        for (int r = 0; r < level.startRows; r++)
            board.FillRow(r, RandomIon);

        shooter.BallLanded += OnBallLanded;
        queue.Changed += UpdateQueue;

        UpdateQueue();
        RefreshHUD();
    }

    // Bóng ngẫu nhiên cho bảng lúc bắt đầu và cho hàng chờ / bóng bắn. Không có ion trung hòa.
    Ion RandomIon() => table.RandomIon();

    // Bóng cho hàng rơi xuống: đôi khi là ion trung hòa (chỉ rơi, không vào hàng chờ)
    Ion DropIon() =>
        table.fillers.Length > 0 && Random.value < table.fillerChance
            ? table.RandomFiller()
            : RandomIon();

    void RefreshHUD() => hud.Refresh(moves, level.maxMoves, score, cleared, level.clearGoal);

    // Chỉ hiển thị số bóng trong hàng chờ (không tính bóng trên súng)
    void UpdateQueue() => hud.ShowQueue(queue.Next, queue.Waiting);

    void OnBallLanded(Ball placed)
    {
        if (over) return;

        moves++;
        ResolveReactions(placed);

        if (moves % level.movesPerDrop == 0)
            DropRow();

        RefreshHUD();
        CheckEnd();
    }

    void ResolveReactions(Ball placed)
    {
        var matched = new List<Ball>();
        ReactionRule last = null;
        int gained = 0;

        foreach (var c in board.Neighbours(placed.Cell))
        {
            if (!board.TryGet(c, out Ball other)) continue;

            ReactionRule rule = table.Find(placed.Data, other.Data);
            if (rule == null) continue;

            matched.Add(other);
            gained += rule.points;
            last = rule;
        }

        if (matched.Count == 0) return;

        matched.Add(placed);
        foreach (var b in matched) board.Remove(b);

        score += gained;
        cleared += matched.Count;
        Sfx.Play(SfxType.Precipitate);
        hud.ShowPrecipitate(last, gained);
    }

    void DropRow()
    {
        board.Descend();
        if (board.FillRow(0, DropIon) > 0)
            Sfx.Play(SfxType.Drop);
    }

    void CheckEnd()
    {
        if (cleared >= level.clearGoal)
            EndGame(true);                                     // Clear đủ số bóng yêu cầu
        else if (board.ReachedDeadLine())
            EndGame(false);                                    // Chạm vạch đỏ
        else if (!queue.HasBall)
            EndGame(false);                                    // Hết bóng để bắn
        else if (moves >= level.maxMoves)
            EndGame(false);                                    // Hết lượt
    }

    void EndGame(bool win)
    {
        over = true;
        shooter.enabled = false;

        if (win) Progress.Complete(level.index);

        Sfx.Play(win ? SfxType.Win : SfxType.Lose);
        hud.ShowResult(win, score, moves, win && level.next != null);
    }

    // ---------- Gọi từ UI Button ----------
    public void OnSwapPressed()
    {
        if (!over && !shooter.Busy && !Pause.IsPaused) queue.SwapWithNext();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        Session.Current = level.next;
        Restart();
    }

    public void GoMenu()
    {
        Session.Current = null;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}