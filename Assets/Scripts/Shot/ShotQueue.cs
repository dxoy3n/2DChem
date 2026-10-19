using System;
using System.Collections.Generic;
using UnityEngine;

// Đặt trên ShotQueue (con của Shooter)
public class ShotQueue : MonoBehaviour
{
    readonly List<Ion> items = new();        // [0] = trên súng, [1..] = hàng chờ
    public Action Changed;

    public bool HasBall => items.Count > 0;
    public int Waiting => Mathf.Max(0, items.Count - 1);   // Chỉ hàng chờ, không tính bóng trên súng
    public Ion Current => items.Count > 0 ? items[0] : null;
    public Ion Next => items.Count > 1 ? items[1] : null;

    // GameManager gọi khi bắt đầu màn: 1 bóng trên súng + waitingCount bóng chờ
    public void Init(int waitingCount, Func<Ion> draw)
    {
        items.Clear();
        for (int i = 0; i <= waitingCount; i++)
            items.Add(draw());
        Changed?.Invoke();
    }

    public Ion TakeCurrent()
    {
        Ion ion = items[0];
        items.RemoveAt(0);
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