using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour {
    // Editor variables
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private GameObject lifesPanel;
    [SerializeField]
    private GameObject[] lifesImages;

    #region Public calls
    public string GetScoreText() {
        return scoreText.text;
    }

    public void SetScoreText(int score) {
        scoreText.SetText($"{score:00}");
    }

    public void ShowLifePanel(bool showLifes) {
        lifesPanel.SetActive(showLifes);
    }

    public void ResetLifes(int lifes) {
        for (var i = 0; i < lifesImages.Length; i++)
            lifesImages[i].SetActive(i < lifes);
    }

    public void ShowLife(int life) {
        if (life >= 0 && life < lifesImages.Length)
            lifesImages[life].SetActive(true);
    }

    public void HideLife(int life) {
        if (life >= 0 && life < lifesImages.Length)
            lifesImages[life].SetActive(false);
    }
    #endregion
}
