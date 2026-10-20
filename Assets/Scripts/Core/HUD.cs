using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("Thông số")]
    [SerializeField] TMP_Text movesText, scoreText, clearText, toastText;

    [Header("Hàng chờ")]
    [SerializeField] Image nextImg;
    [SerializeField] TMP_Text nextLabel;
    [SerializeField] TMP_Text queueText;

    [Header("Kết quả")]
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject dim;             // Màn đen che HUD
    [SerializeField] GameObject resultPanel;
    [SerializeField] TMP_Text resultText;
    [SerializeField] GameObject nextButton;
    [SerializeField] float panelDelay = 0.3f;    // Dim hiện trước, panel hiện sau

    public void Refresh(int moves, int maxMoves, int score, int cleared, int total)
    {
        movesText.text = $"Lượt: {moves}";
        scoreText.text = $"Điểm: {score}";
        clearText.text = $"Phản ứng: {cleared}/{total}";
    }

    public void ShowQueue(Ion next, int count)
    {
        queueText.text = $" {count}";

        if (next == null)
        {
            nextImg.color = Color.clear;
            nextLabel.text = "";
            return;
        }

        nextImg.sprite = next.sprite;
        nextImg.color = next.color;
        nextLabel.text = next.symbol;
    }

    public void ShowPrecipitate(ReactionRule rule, int points)
    {
        toastText.text = $"Kết tủa {rule.precipitate}  +{points}\n{rule.equation}";
        CancelInvoke(nameof(ClearToast));
        Invoke(nameof(ClearToast), 1.5f);
    }

    void ClearToast() => toastText.text = "";

    public void ShowResult(bool win, int score, int moves, bool hasNext)
    {
        pauseButton.SetActive(false);
        resultText.text = $"{(win ? "THẮNG" : "THUA")}\nĐiểm {score} • {moves} lượt";
        nextButton.SetActive(win && hasNext);

        dim.SetActive(true);
        Invoke(nameof(OpenResultPanel), panelDelay);
    }

    void OpenResultPanel() => resultPanel.SetActive(true);
}