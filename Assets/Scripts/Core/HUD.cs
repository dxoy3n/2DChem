using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] TMP_Text movesText, scoreText, clearText, toastText, resultText;
    [SerializeField] GameObject resultPanel;
    [SerializeField] Image nextImg;          // [UI] Hàng chờ: sau này đổi sang sprite

    public void Refresh(int moves, int maxMoves, int score, int cleared, int total)
    {
        movesText.text = $"Lượt: {moves}/{maxMoves}";
        scoreText.text = $"Điểm: {score}";
        clearText.text = $"Clear: {cleared}/{total}";   // [UI] Thay bằng icon + số
    }

    public void ShowQueue(Ion next)
    {
        nextImg.color = next != null ? next.color : Color.clear;   // [UI] Gán next.sprite
    }

    public void ShowPrecipitate(string precipitate, int points)
    {
        toastText.text = $"Kết tủa {precipitate}  +{points}";
        CancelInvoke(nameof(ClearToast));
        Invoke(nameof(ClearToast), 1.5f);                           // [UI] Thời gian toast
    }

    void ClearToast() => toastText.text = "";

    public void ShowResult(bool win, int score, int moves)
    {
        resultText.text = $"{(win ? "THẮNG" : "THUA")}\nĐiểm {score} • {moves} lượt";
        resultPanel.SetActive(true);
    }
}