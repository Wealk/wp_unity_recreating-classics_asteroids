using System;
using TMPro;
using UnityEngine;

public class PersistentInterface : MonoBehaviour {
    // Global properties
    private static PersistentInterface instance;
    public static PersistentInterface Instance => instance;

    // Public properties
    public PlayerScore Player1Score => player1Score;
    public PlayerScore Player2Score => player2Score;

    // Editor variables
    [Header("Components")]
    [SerializeField]
    private PlayerScore player1Score;
    [SerializeField]
    private PlayerScore player2Score;
    [SerializeField]
    private TextMeshProUGUI highestScoreText;

    #region Unity methods
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else Destroy(gameObject);
    }
    #endregion

    #region Public calls
    public void ResetPlayerLifes(int lifes) {
        player1Score.ResetLifes(lifes);
        player2Score.ResetLifes(lifes);
    }

    public void ResetPlayerScores() {
        player1Score.SetScoreText(0);
        player2Score.SetScoreText(0);
    }

    public void ShowPlayer1Lifes(bool showLifes) {
        player1Score.ShowLifePanel(showLifes);
    }

    public void ShowPlayer2Panel(bool showPanel) {
        player2Score.gameObject.SetActive(showPanel);
    }

    public void ShowPlayer2Lifes(bool showLifes) {
        player2Score.ShowLifePanel(showLifes);
    }

    public string GetHighestScoreText() {
        return highestScoreText.text;
    }

    public void SetHighestScoreText(int newScore) {
        highestScoreText.SetText($"{newScore:00}");
    }
    #endregion
}
