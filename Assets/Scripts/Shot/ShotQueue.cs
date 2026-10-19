using System;
using System.Collections.Generic;
using UnityEngine;

// Đặt trên ShotQueue (con của Shooter)
public class ShotQueue : MonoBehaviour
{
    [SerializeField] int waitingCount = 1;   // Số bóng chờ, cộng thêm 1 bóng trên súng

    readonly List<Ion> items = new();        // [0] = trên súng, [1..] = hàng chờ
    Func<Ion> next;                          // Nguồn bóng của màn (null = hết bóng)
    public Action Changed;

    public bool HasBall => items.Count > 0;
    public int Count => items.Count;     // Số bóng sẵn sàng bắn (gồm bóng trên súng)
    public Ion Current => items.Count > 0 ? items[0] : null;
    public Ion Next => items.Count > 1 ? items[1] : null;

    // GameManager gọi khi bắt đầu màn
    public void Init(Func<Ion> draw)
    {
        next = draw;
        items.Clear();
        for (int i = 0; i <= waitingCount; i++)
        {
            Ion ion = next();
            if (ion == null) break;
            items.Add(ion);
        }
        Changed?.Invoke();
    }

    public Ion TakeCurrent()
    {
        Ion ion = items[0];
        items.RemoveAt(0);

        Ion fresh = next();
        if (fresh != null) items.Add(fresh);

        Changed?.Invoke();
        return ion;
    }

    public void SwapWithNext()
    {
        if (items.Count < 2) return;
        (items[0], items[1]) = (items[1], items[0]);
        Changed?.Invoke();
    }
}