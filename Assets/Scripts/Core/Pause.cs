using UnityEngine;

// Đặt trên Canvas
public class Pause : MonoBehaviour
{
    [SerializeField] Shooter shooter;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject dim;     // Màn đen che HUD, giống khi thắng/thua

    public static bool IsPaused { get; private set; }

    void Awake() => Set(false);

    public void Toggle() => Set(!IsPaused);

    void Set(bool on)
    {
        IsPaused = on;
        Time.timeScale = on ? 0f : 1f;   // Đóng băng bóng đang bay
        shooter.enabled = !on;
        panel.SetActive(on);
        dim.SetActive(on);
    }
}