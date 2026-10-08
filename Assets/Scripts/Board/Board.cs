using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] Ball ballPrefab;
    [SerializeField] int columns = 7;
    [SerializeField] float cell = 0.8f;      // [UI] Kích thước ô, chỉnh theo sprite
    [SerializeField] int deadRow = 9;        // Bóng chạm hàng này = thua

    [Header("Hàng")]
    [SerializeField] int minPerRow = 2;      // Luật: số bóng tối thiểu mỗi hàng
    [SerializeField] int maxPerRow = 3;      // Luật: số bóng tối đa mỗi hàng

    public int Columns => columns;
    public bool IsEmpty => cells.Count == 0;

    readonly Dictionary<Vector2Int, Ball> cells = new();
    static readonly Vector2Int[] dirs = { new(0, -1), new(0, 1), new(-1, 0), new(1, 0) };

    // Hàng 0 ở trên cùng, tăng dần xuống dưới. Căn giữa theo X.
    public Vector2 CellToWorld(Vector2Int c) =>
        (Vector2)transform.position + new Vector2((c.x - (columns - 1) / 2f) * cell, -c.y * cell);

    public float LeftX => CellToWorld(Vector2Int.zero).x - cell * 0.5f;
    public float RightX => CellToWorld(new(columns - 1, 0)).x + cell * 0.5f;
    public float TopY => CellToWorld(Vector2Int.zero).y + cell * 0.5f;

    public bool TryGet(Vector2Int c, out Ball b) => cells.TryGetValue(c, out b);

    public IEnumerable<Vector2Int> Neighbours(Vector2Int c)
    {
        foreach (var d in dirs) yield return c + d;
    }

    public bool HitsBall(Vector2 p)
    {
        float min = cell * 0.9f;
        foreach (var b in cells.Values)
            if (((Vector2)b.transform.position - p).sqrMagnitude < min * min)
                return true;
        return false;
    }

    public bool ReachedDeadLine()
    {
        foreach (var c in cells.Keys)
            if (c.y >= deadRow) return true;
        return false;
    }

    public Ball Spawn(Ion ion, Vector2Int c)
    {
        Ball b = Instantiate(ballPrefab, transform);
        b.Setup(ion, c, CellToWorld(c));
        cells[c] = b;
        return b;
    }

    // Đổ một hàng với 2-3 bóng ở cột ngẫu nhiên. Trả về số bóng đã tạo.
    public int FillRow(int row, System.Func<Ion> randomIon, int budget)
    {
        int n = Mathf.Min(Random.Range(minPerRow, maxPerRow + 1), budget);

        var free = new List<int>();
        for (int c = 0; c < columns; c++) free.Add(c);

        for (int i = 0; i < n; i++)
        {
            int k = Random.Range(0, free.Count);
            Spawn(randomIon(), new Vector2Int(free[k], row));
            free.RemoveAt(k);
        }
        return n;
    }

    // Đặt bóng bắn lên vào ô trống gần điểm dừng nhất
    public Ball Place(Ion ion, Vector2 worldPos)
    {
        Vector2Int best = Vector2Int.zero;
        float bestD = float.MaxValue;

        for (int r = 0; r <= deadRow; r++)
            for (int c = 0; c < columns; c++)
            {
                var cand = new Vector2Int(c, r);
                if (cells.ContainsKey(cand)) continue;

                float d = (CellToWorld(cand) - worldPos).sqrMagnitude;
                if (d < bestD) { bestD = d; best = cand; }
            }
        return Spawn(ion, best);
    }

    public void Remove(Ball b)
    {
        cells.Remove(b.Cell);
        Destroy(b.gameObject);
    }

    public void Descend()
    {
        var old = new List<Ball>(cells.Values);
        cells.Clear();
        foreach (var b in old)
        {
            var c = b.Cell + new Vector2Int(0, 1);
            b.MoveTo(c, CellToWorld(c));
            cells[c] = b;
        }
    }
}