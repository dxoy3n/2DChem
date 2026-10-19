using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Đặt trên object Menu (gốc Hierarchy, scene MainMenu)
public class Menu : MonoBehaviour
{
    [SerializeField] Level[] levels;           // Theo thứ tự: Màn 1, Màn 2
    [SerializeField] Button[] levelButtons;    // Theo thứ tự: Màn 1, Màn 2

    [Header("Panel")]
    [SerializeField] GameObject mainPanel;       // MainPanel
    [SerializeField] GameObject levelPanel;      // LevelPanel
    [SerializeField] GameObject solubilityImage; // Ảnh "Bảng tính tan" (con của BtnTutorial)
    [SerializeField] GameObject[] hideWhileSolubility; // BtnStart, BtnQuit

    void Start()
    {
        Time.timeScale = 1f;

        for (int i = 0; i < levelButtons.Length; i++)
            levelButtons[i].interactable = Progress.IsUnlocked(i);

        solubilityImage.SetActive(false);
        ShowMain();
    }

    // ---------- Chuyển panel ----------
    public void ShowMain() => SetPanel(mainPanel);
    public void ShowLevels() => SetPanel(levelPanel);

    void SetPanel(GameObject active)
    {
        mainPanel.SetActive(active == mainPanel);
        levelPanel.SetActive(active == levelPanel);
    }

    // Gắn vào BtnTutorial: hiện/ẩn bảng tính tan, đồng thời ẩn các nút khác
    public void ToggleSolubility()
    {
        bool show = !solubilityImage.activeSelf;
        solubilityImage.SetActive(show);

        foreach (var g in hideWhileSolubility)
            g.SetActive(!show);
    }

    // ---------- Hành động ----------
    public void Play(int index)
    {
        if (!Progress.IsUnlocked(index)) return;

        Session.Current = levels[index];
        SceneManager.LoadScene("Game");
    }

    public void Quit() => Application.Quit();

    // Chuột phải component Menu > Reset tiến trình (để test)
    [ContextMenu("Reset tiến trình")]
    void ResetProgress()
    {
        Progress.Reset();
        Start();
        Debug.Log("Đã reset tiến trình.");
    }
}