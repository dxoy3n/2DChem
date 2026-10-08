using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [SerializeField] Board board;
    [SerializeField] ShotQueue queue;
    [SerializeField] Ball ballPrefab;
    [SerializeField] Transform muzzle;
    [SerializeField] Ball preview;           // Bóng hiển thị trên súng

    public System.Action<Ball> BallLanded;
    public bool Busy { get; private set; }

    Camera cam;

    void Awake() => cam = Camera.main;

    void Start()
    {
        queue.Changed += ShowPreview;
        ShowPreview();
    }

    void ShowPreview() => preview.Setup(queue.Current, Vector2Int.zero, muzzle.position);

    void Update()
    {
        var pointer = Pointer.current;
        if (Busy || pointer == null || !pointer.press.wasPressedThisFrame) return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        Vector2 target = cam.ScreenToWorldPoint(pointer.position.ReadValue());
        Vector2 dir = target - (Vector2)muzzle.position;
        if (dir.y < 0.15f) return;

        Fire(dir);
    }

    void Fire(Vector2 dir)
    {
        Busy = true;
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
        BallLanded?.Invoke(board.Place(ion, pos));
    }
}