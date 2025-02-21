using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    private const int WAVE_INCREMENT = 2;
    private const int MAX_ASTEROIDS = 12;
    private const float SMALL_UFO_PERCENTAGE = 0.75f;

    // Global properties
    private static GameController instance;
    public static GameController Instance => instance;

    // Editor variables
    [Header("Entities prefabs")]
    [SerializeField]
    private GameObject spaceshipPrefab;
    [SerializeField]
    private GameObject asteroidPrefab;
    [SerializeField]
    private GameObject ufoBigPrefab;
    [SerializeField]
    private GameObject ufoSmallPrefab;

    [Header("Game settings")]
    [SerializeField, Range(1, 5)]
    private int startingAsteroids;
    [SerializeField]
    private Vector2 ufoApperanceRange;
    [SerializeField]
    private GameObject player1RespawnText;
    [SerializeField]
    private GameObject player2RespawnText;

    // Private variables
    private Camera mainCamera;
    private AsteroidsActions actions;
    private SpaceshipController player1;
    private SpaceshipController player2;
    private float ufoApperance;
    private int activeAsteroidsCount;
    private int asteroidsWave;
    private bool player2HasBeenSpawned;

    #region Unity methods
    private void Awake() {
        if (instance == null) {
            instance = this;
            mainCamera = Camera.main;
            actions = new AsteroidsActions();
            actions.Game.Player1Start.performed += HandlePlayer1Start;
            actions.Game.Player2Start.performed += HandlePlayer2Start;
            player1 = null;
            player2 = null;
            ufoApperance = Random.Range(ufoApperanceRange.x, ufoApperanceRange.y);
            activeAsteroidsCount = 0;
            asteroidsWave = startingAsteroids;
            player2HasBeenSpawned = false;

            var player = Instantiate(spaceshipPrefab);
            if (player.TryGetComponent(out player1)) player1.Initialize(true);

            StartWave(startingAsteroids);
            PersistentInterface.Instance.ShowPlayer2Panel(false);
        } else Destroy(this);
    }

    private void OnEnable() {
        actions.Game.Enable();
    }

    private void OnDisable() {
        actions.Game.Disable();
    }

    private void Update() {
        ufoApperance -= Time.deltaTime;
        if (ufoApperance <= 0) InstantiateUFO();
    }
    #endregion

    #region Public calls
    public void AddAsteroid() {
        activeAsteroidsCount++;
    }

    public void RemoveAsteroid() {
        activeAsteroidsCount--;
        if (activeAsteroidsCount == 0) StartWave(asteroidsWave + WAVE_INCREMENT);
    }

    public void ShowPlayer1RespawnText() {
        player1RespawnText.SetActive(true);
    }

    public void ShowPlayer2RespawnText() {
        player2RespawnText.SetActive(true);
    }

    public void SpaceshipWithoutLifes(SpaceshipController ship) {
        if (ship == player1) player1 = null;
        else if (ship == player2) player2 = null;
        Destroy(ship.gameObject);

        if (player1 == null && player2 == null)
            SceneManager.LoadScene("EndScene");
    }
    #endregion

    #region Private methods
    private void StartWave(int asteroidsCount) {
        asteroidsWave = asteroidsCount >= MAX_ASTEROIDS ? MAX_ASTEROIDS : asteroidsCount;
        for (var i = 0; i < asteroidsWave; i++) {
            var position = GetRandomOutBoundPosition();
            Instantiate(asteroidPrefab, position, Quaternion.identity);
        }
    }

    private void HandlePlayer1Start(InputAction.CallbackContext context) {
        if (player1 != null && !player1.gameObject.activeSelf && player1.CanBeRespawned) {
            player1.transform.position = Vector2.zero;
            player1.gameObject.SetActive(true);
            player1RespawnText.SetActive(false);
        }
    }

    private void HandlePlayer2Start(InputAction.CallbackContext context) {
        if (!player2HasBeenSpawned && player2 == null) {
            var player = Instantiate(spaceshipPrefab);
            if (player.TryGetComponent(out player2)) player2.Initialize(false);
            player2RespawnText.SetActive(false);
            player2HasBeenSpawned = true;
        } else if (player2 != null && !player2.gameObject.activeSelf && player2.CanBeRespawned) {
            player2.transform.position = Vector2.zero;
            player2.gameObject.SetActive(true);
            player2RespawnText.SetActive(false);
        }
    }

    private void InstantiateUFO() {
        Instantiate(Random.Range(0f, 1f) >= SMALL_UFO_PERCENTAGE ? ufoSmallPrefab : ufoBigPrefab);
        ufoApperance = Random.Range(ufoApperanceRange.x, ufoApperanceRange.y);
    }
    #endregion

    #region Auxiliar functions
    private Vector2 GetRandomOutBoundPosition() {
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
