using UnityEngine;
using UnityEngine.UI;

// Đặt trên ResultPanel
public class Stars : MonoBehaviour
{
    [SerializeField] Image[] icons;                       // 3 Image sao
    [SerializeField] Color on = Color.yellow;
    [SerializeField] Color off = new(1f, 1f, 1f, 0.25f);

    public void Show(int count)
    {
        for (int i = 0; i < icons.Length; i++)
            icons[i].color = i < count ? on : off;
    }
}
