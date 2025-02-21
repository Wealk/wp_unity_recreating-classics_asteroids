using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndUI : MonoBehaviour {
    // Global properties

    // Public properties

    // Editor variables
    [SerializeField]
    private TextMeshProUGUI endGameText;

    // Private variables
    private AsteroidsActions actions;

    #region Unity methods
    private void Awake() {
        actions = new AsteroidsActions();
        actions.End.Continue.performed += HandleContinue;

        var player1Score = GetScore(PersistentInterface.Instance.Player1Score.GetScoreText());
        var player2Score = GetScore(PersistentInterface.Instance.Player2Score.GetScoreText());
        var highestScore = GetScore(PersistentInterface.Instance.GetHighestScoreText());

        if (player1Score >= player2Score && player1Score > highestScore) {
            endGameText.SetText($"PLAYER 1 HAS BEATEN HIGHEST SCORE\n<size=90>{player1Score}</size>");
            PersistentInterface.Instance.SetHighestScoreText(player1Score);
        } else if (player2Score > highestScore) {
            endGameText.SetText($"PLAYER 2 HAS BEATEN HIGHEST SCORE\n<size=90>{player2Score}</size>");
            PersistentInterface.Instance.SetHighestScoreText(player2Score);
        } else endGameText.SetText($"HIGHEST SCORE UNBEATEN\n<size=90>{highestScore}</size>");
    }

    private void OnEnable() {
        actions.End.Enable();
    }

    private void OnDisable() {
        actions.End.Disable();
    }
    #endregion

    #region Private methods
    private void HandleContinue(InputAction.CallbackContext context) {
        SceneManager.LoadScene("MenuScene");
    }
    #endregion

    #region Auxiliar functions
    private int GetScore(string text) {
        if (int.TryParse(text, out var score))
            return score;

        return 0;
    }
    #endregion
}
