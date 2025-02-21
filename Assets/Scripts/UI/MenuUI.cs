using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour {
    // Editor variables
    [SerializeField, Range(3, 12)]
    private int backgroundAsteroidsCount;
    [SerializeField]
    private GameObject asteroidsPrefab;

    // Private variables
    private AsteroidsActions actions;

    #region Unity methods
    private void Awake() {
        actions = new AsteroidsActions();
        actions.Game.Player1Start.started += HandleStartGame;

        for (var i = 0; i < backgroundAsteroidsCount; i++)
            Instantiate(asteroidsPrefab, GetRandomOutBoundPosition(), Quaternion.identity);
    }

    private void OnEnable() {
        actions.Game.Enable();
    }

    private void OnDisable() {
        actions.Game.Disable();
    }

    private void Start() {
        PersistentInterface.Instance.ResetPlayerScores();
        PersistentInterface.Instance.ShowPlayer2Panel(true);
    }
    #endregion

    #region Private methods
    private void HandleStartGame(InputAction.CallbackContext context) {
        SceneManager.LoadScene("GameScene");
    }
    #endregion

    #region Auxiliar functions
    private Vector2 GetRandomOutBoundPosition() {
        var mainCamera = Camera.main;
        var camHeight = mainCamera.orthographicSize;
        var camWidth = camHeight * mainCamera.aspect;

        var side = Random.Range(0, 4);
        return side switch {
            0 => new Vector2(Random.Range(-camWidth, camWidth), camHeight),
            1 => new Vector2(Random.Range(-camWidth, camWidth), -camHeight),
            2 => new Vector2(-camWidth, Random.Range(-camHeight, camHeight)),
            3 => new Vector2(camWidth, Random.Range(-camHeight, camHeight)),
            _ => Vector2.zero,
        };
    }
    #endregion
}
