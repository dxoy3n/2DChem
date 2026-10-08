using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 14f;      // [UI] Tốc độ bay

    Board board;
    Vector2 dir;
    float minX, maxX;
    bool flying;
    System.Action<Projectile> onStop;

    public void Launch(Vector2 direction, Board b, System.Action<Projectile> stopCallback)
    {
        dir = direction.normalized;
        board = b;
        minX = b.LeftX;
        maxX = b.RightX;
        onStop = stopCallback;
        flying = true;
    }

    void Update()
    {
        if (!flying) return;

        Vector2 p = (Vector2)transform.position + dir * speed * Time.deltaTime;

        if (p.x < minX) { p.x = 2 * minX - p.x; dir.x = -dir.x; }
        else if (p.x > maxX) { p.x = 2 * maxX - p.x; dir.x = -dir.x; }

        transform.position = p;

        if (p.y >= board.TopY || board.HitsBall(p))
        {
            flying = false;
            onStop?.Invoke(this);
        }
    }
}