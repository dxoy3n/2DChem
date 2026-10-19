using UnityEngine;

// Đặt trên Deadline
public class DeadlineWarn : MonoBehaviour
{
    [SerializeField] Board board;
    [SerializeField] SpriteRenderer line;
    [SerializeField] float speed = 4f;
    [SerializeField] Color safe = new(1f, 0.32f, 0.32f, 0.5f);
    [SerializeField] Color danger = Color.red;

    void Update()
    {
        if (!board.IsDanger)
        {
            line.color = safe;
            return;
        }

        float t = Mathf.PingPong(Time.time * speed, 1f);
        line.color = Color.Lerp(safe, danger, t);
    }
}
