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
    int moves, score, spawned, cleared;
    bool over;
    readonly HashSet<ReactionRule> found = new();   // Sổ tay: kết tủa đã gặp trong màn

    void Awake()
    {
        Time.timeScale = 1f;
        level = Session.Current != null ? Session.Current : defaultLevel;
        table = level.table;
        queue.Init(table);
    }

    void Start()
    {
        for (int r = 0; r < level.startRows; r++)
            spawned += board.FillRow(r, table.RandomIon, level.totalBalls - spawned);

        shooter.BallLanded += OnBallLanded;
        queue.Changed += () => hud.ShowQueue(queue.Next);

        hud.ShowQueue(queue.Next);
        RefreshHUD();
    }

    void RefreshHUD() => hud.Refresh(moves, level.maxMoves, score, cleared, level.totalBalls);

    void OnBallLanded(Ball placed)
    {
        if (over) return;

        moves++;
        ResolveReactions(placed);

        if (moves % level.movesPerDrop == 0 && spawned < level.totalBalls)
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
            found.Add(rule);
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
        spawned += board.FillRow(0, table.RandomIon, level.totalBalls - spawned);
        Sfx.Play(SfxType.Drop);
    }

    void CheckEnd()
    {
        if (board.ReachedDeadLine())
            EndGame(false);                                    // Chạm vạch đỏ
        else if (spawned >= level.totalBalls && board.IsEmpty)
            EndGame(true);                                     // Xóa hết bóng
        else if (moves >= level.maxMoves)
            EndGame(false);                                    // Hết lượt
    }

    void EndGame(bool win)
    {
        over = true;
        shooter.enabled = false;

        int stars = win ? level.StarsFor(moves) : 0;
        if (win) Progress.Complete(level.index);

        Sfx.Play(win ? SfxType.Win : SfxType.Lose);
        hud.ShowResult(win, score, moves, stars, found, win && level.next != null);
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
