using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Đặt trên Aim (con của Shooter)
public class Trajectory : MonoBehaviour
{
    [SerializeField] Shooter shooter;
    [SerializeField] Board board;
    [SerializeField] LineRenderer line;
    [SerializeField] float speed = 14f;      // Phải bằng Projectile.speed
    [SerializeField] float step = 0.02f;
    [SerializeField] int maxSteps = 200;

    readonly List<Vector3> points = new();

    void Update()
    {
        line.enabled = false;

        var pointer = Pointer.current;
        if (!shooter.enabled || shooter.Busy || pointer == null || !pointer.press.isPressed) return;
        if (Shooter.OverUI()) return;
        if (!shooter.TryAim(out Vector2 dir)) return;

        line.enabled = true;
        Draw(dir.normalized);
    }

    // Mô phỏng đúng luật bay của Projectile: nảy tường, dừng khi chạm trần hoặc bóng
    void Draw(Vector2 dir)
    {
        points.Clear();
        Vector2 p = shooter.Muzzle.position;
        points.Add(p);

        for (int i = 0; i < maxSteps; i++)
        {
            Vector2 next = p + dir * speed * step;

            if (next.x < board.LeftX) { next.x = 2 * board.LeftX - next.x; dir.x = -dir.x; }
            else if (next.x > board.RightX) { next.x = 2 * board.RightX - next.x; dir.x = -dir.x; }

            bool stop = next.y >= board.TopY || board.HitsBall(next);
            points.Add(next);
            if (stop) break;

            p = next;
        }

        line.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
            line.SetPosition(i, points[i]);
    }
}
