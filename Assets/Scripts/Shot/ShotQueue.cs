using System;
using System.Collections.Generic;
using UnityEngine;

public class ShotQueue : MonoBehaviour
{
    [SerializeField] ReactionTable table;
    [SerializeField] int waitingCount = 1;   // [UI] 1 bóng chờ. Cộng thêm 1 bóng trên súng

    readonly List<Ion> items = new();        // [0] = trên súng, [1] = hàng chờ
    public Action Changed;

    public Ion Current => items[0];
    public Ion Next => items.Count > 1 ? items[1] : null;

    void Awake()
    {
        for (int i = 0; i <= waitingCount; i++)
            items.Add(table.RandomIon());
    }

    public Ion TakeCurrent()
    {
        Ion ion = items[0];
        items.RemoveAt(0);
        items.Add(table.RandomIon());
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