using UnityEngine;
using TMPro;

public class Ball : MonoBehaviour
{
    [SerializeField] SpriteRenderer body;
    [SerializeField] TMP_Text label;

    public Ion Data { get; private set; }
    public Vector2Int Cell { get; private set; }

    public void Setup(Ion ion, Vector2Int cell, Vector2 pos)
    {
        Data = ion;
        body.color = ion.color;
        if (ion.sprite) body.sprite = ion.sprite;   // [UI] Dùng sprite khi có
        label.text = ion.symbol;
        MoveTo(cell, pos);
    }

    public void MoveTo(Vector2Int cell, Vector2 pos)
    {
        Cell = cell;
        transform.position = pos;
    }
}