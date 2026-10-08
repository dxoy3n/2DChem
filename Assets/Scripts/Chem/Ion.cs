using UnityEngine;

[CreateAssetMenu(menuName = "Chem/Ion", fileName = "Ion")]
public class Ion : ScriptableObject
{
    public string symbol;
    public Color color = Color.white;
    public Sprite sprite;        // [UI] Sprite bóng, thay khi hoàn thiện
}