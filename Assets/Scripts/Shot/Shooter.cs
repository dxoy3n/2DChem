using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Đặt trên Shooter
public class Shooter : MonoBehaviour
{
    [SerializeField] Board board;
    [SerializeField] ShotQueue queue;
    [SerializeField] Ball ballPrefab;
    [SerializeField] Transform muzzle;
    [SerializeField] Ball preview;           // Bóng hiển thị trên súng

    public System.Action<Ball> BallLanded;
    public bool Busy { get; private set; }
    public Transform Muzzle => muzzle;

    Camera cam;
    bool aiming;                             // Đang kéo để ngắm (bắt đầu ngoài UI)

    void Awake() => cam = Camera.main;

    void Start()
    {
        queue.Changed += ShowPreview;
        ShowPreview();
    }

    void ShowPreview()
    {
        preview.gameObject.SetActive(queue.HasBall);
        if (queue.HasBall) preview.Setup(queue.Current, Vector2Int.zero, muzzle.position);
    }

    void Update()
    {
        var pointer = Pointer.current;
        if (Busy || pointer == null) return;

        if (pointer.press.wasPressedThisFrame) aiming = !OverUI();

        // Kéo để ngắm (Trajectory vẽ khi đang giữ), thả tay để bắn
        if (aiming && pointer.press.wasReleasedThisFrame)
        {
            aiming = false;
            if (TryAim(out Vector2 dir)) Fire(dir);
        }
    }

    // Dùng chung với Trajectory
    public bool TryAim(out Vector2 dir)
    {
        dir = Vector2.zero;
        var pointer = Pointer.current;
        if (pointer == null) return false;

        Vector2 target = cam.ScreenToWorldPoint(pointer.position.ReadValue());
        dir = target - (Vector2)muzzle.position;
        return dir.y >= 0.15f;
    }

    public static bool OverUI() =>
        EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

    void Fire(Vector2 dir)
    {
        if (!queue.HasBall) return;

        Busy = true;
        Sfx.Play(SfxType.Fire);

        Ion ion = queue.TakeCurrent();
        Ball ball = Instantiate(ballPrefab, muzzle.position, Quaternion.identity);
        ball.Setup(ion, Vector2Int.zero, muzzle.position);
        ball.gameObject.AddComponent<Projectile>().Launch(dir, board, OnStop);
    }

    void OnStop(Projectile p)
    {
        Ion ion = p.GetComponent<Ball>().Data;
        Vector2 pos = p.transform.position;
        Destroy(p.gameObject);

        Busy = false;
        Sfx.Play(SfxType.Land);
        BallLanded?.Invoke(board.Place(ion, pos));
    }
}