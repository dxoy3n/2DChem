using System.Collections.Generic;
using System.Text;
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

    [Header("Kết quả")]
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject resultPanel;
    [SerializeField] TMP_Text resultText;
    [SerializeField] TMP_Text codexText;
    [SerializeField] Stars stars;
    [SerializeField] GameObject nextButton;

    public void Refresh(int moves, int maxMoves, int score, int cleared, int total)
    {
        movesText.text = $"Lượt: {moves}/{maxMoves}";
        scoreText.text = $"Điểm: {score}";
        clearText.text = $"Clear: {cleared}/{total}";
    }

    public void ShowQueue(Ion next)
    {
        if (next == null)
        {
            nextImg.color = Color.clear;
            nextLabel.text = "";
            return;
        }

        nextImg.sprite = next.sprite;
        nextImg.color = next.color;     // Nếu sprite đã có màu, đổi thành Color.white
        nextLabel.text = next.symbol;
    }

    public void ShowPrecipitate(ReactionRule rule, int points)
    {
        toastText.text = $"Kết tủa {rule.precipitate}  +{points}\n{rule.equation}";
        CancelInvoke(nameof(ClearToast));
        Invoke(nameof(ClearToast), 1.5f);
    }

    void ClearToast() => toastText.text = "";

    public void ShowResult(bool win, int score, int moves, int starCount,
                           IEnumerable<ReactionRule> found, bool hasNext)
    {
        pauseButton.SetActive(false);
        resultText.text = $"{(win ? "THẮNG" : "THUA")}\nĐiểm {score} • {moves} lượt";
        stars.Show(starCount);
        nextButton.SetActive(win && hasNext);
        codexText.text = BuildCodex(found);
        resultPanel.SetActive(true);
    }

    string BuildCodex(IEnumerable<ReactionRule> found)
    {
        var sb = new StringBuilder("Sổ tay phản ứng\n");
        foreach (var r in found)
            sb.AppendLine($"• {r.precipitate}: {r.equation}\n   {r.fact}");
        return sb.ToString();
    }
}
