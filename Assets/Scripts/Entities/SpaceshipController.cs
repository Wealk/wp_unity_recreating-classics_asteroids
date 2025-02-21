using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipController : MonoBehaviour {
    private const int LIFE_THRESHOLD_SCORE = 10000;
    private const int STARTING_LIFES = 3;
    private const float CANON_DISTANCE = 0.75f;

    // Public properties
    public bool CanBeRespawned => canBeRespawned;

    // Editor variables
    [SerializeField]
    private float rotationForce;
    [SerializeField]
    private float impulseForce;
    [SerializeField]
    private float shootDelay;
    [SerializeField]
    private float hyperspaceDelay;
    [SerializeField]
    private float respawnTime;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject deathPrefab;

    // Private variables
    private Camera mainCamera;
    private new Rigidbody2D rigidbody;
    private AsteroidsActions actions;
    private PlayerScore scoreUI;
    private float currentRotation;
    private float currentImpulse;
    private int score;
    private int lifes;
    private int nextLifeScore;
    private bool isPlayer1;
    private bool isShooting;
    private bool isUsingHyperspace;
    private bool canShoot;
    private bool canUseHyperspace;
    private bool canBeRespawned;

    #region Unity methods
    private void Awake() {
        mainCamera = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        score = 0;
        lifes = STARTING_LIFES;
        nextLifeScore = LIFE_THRESHOLD_SCORE;
        isPlayer1 = false;
        isShooting = false;
        isUsingHyperspace = false;
        canShoot = true;
        canUseHyperspace = true;
        canBeRespawned = false;
    }

    private void OnEnable() {
        if (actions != null)
            if (isPlayer1) actions.Player1.Enable();
            else actions.Player2.Enable();

        canBeRespawned = false;
    }

    private void OnDisable() {
        if (actions != null)
            if (isPlayer1) actions.Player1.Disable();
            else actions.Player2.Disable();

        Invoke(nameof(EnableRespawn), respawnTime);
    }

    private void Update() {
        if (isShooting && canShoot)
            Shoot();
        if (isUsingHyperspace && canUseHyperspace)
            Hyperspace();
    }

    private void FixedUpdate() {
        if (currentImpulse != 0)
            rigidbody.AddForce(transform.up * impulseForce, ForceMode2D.Impulse);
        if (currentRotation != 0)
            rigidbody.AddTorque(currentRotation);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Instantiate(deathPrefab, transform.position, transform.rotation);
        rigidbody.linearVelocity = Vector2.zero;
        rigidbody.angularVelocity = 0f;
        RemoveLife();

        gameObject.SetActive(false);
    }
    #endregion

    #region Public calls
    public void Initialize(bool isPlayer1) {
        scoreUI = isPlayer1 ? PersistentInterface.Instance.Player1Score : PersistentInterface.Instance.Player2Score;
        scoreUI.SetScoreText(score);
        scoreUI.ResetLifes(STARTING_LIFES);
        scoreUI.gameObject.SetActive(true);
        scoreUI.ShowLifePanel(true);

        this.isPlayer1 = isPlayer1;
        if (isPlayer1) InitilizePlayer1Controls();
        else InitilizePlayer2Controls();
    }

    public void AddScore(int points) {
        score += points;
        scoreUI.SetScoreText(score);

        if (score >= nextLifeScore) {
            AddLife();
            nextLifeScore += LIFE_THRESHOLD_SCORE;
        }
    }
    #endregion

    #region Private methods
    private void AddLife() {
        scoreUI.ShowLife(lifes);
        lifes++;
    }

    private void RemoveLife() {
        if (lifes > 0) {
            lifes--;
            scoreUI.HideLife(lifes);

            if (lifes == 0) {
                scoreUI.ShowLifePanel(false);
                GameController.Instance.SpaceshipWithoutLifes(this);
            }
        }
    }

    private void InitilizePlayer1Controls() {
        actions = new AsteroidsActions();
        actions.Player1.Rotation.performed += HandleRotation;
        actions.Player1.Rotation.canceled += HandleRotationStop;

        actions.Player1.Impulse.performed += HandleImpulse;
        actions.Player1.Impulse.canceled += HandleImpulseStop;

        actions.Player1.Hyperspace.performed += HandleHyperspace;
        actions.Player1.Hyperspace.canceled += HandleHyperspaceStop;

        actions.Player1.Shoot.performed += HandleShoot;
        actions.Player1.Shoot.canceled += HandleShootStop;

        actions.Player1.Enable();
    }

    private void InitilizePlayer2Controls() {
        actions = new AsteroidsActions();
        actions.Player2.Rotation.performed += HandleRotation;
        actions.Player2.Rotation.canceled += HandleRotationStop;

        actions.Player2.Impulse.performed += HandleImpulse;
        actions.Player2.Impulse.canceled += HandleImpulseStop;

        actions.Player2.Hyperspace.performed += HandleHyperspace;
        actions.Player2.Hyperspace.canceled += HandleHyperspaceStop;

        actions.Player2.Shoot.performed += HandleShoot;
        actions.Player2.Shoot.canceled += HandleShootStop;

        actions.Player2.Enable();
    }

    private void HandleRotation(InputAction.CallbackContext context) {
        var direction = context.ReadValue<float>();
        currentRotation = -direction * rotationForce;
    }

    private void HandleRotationStop(InputAction.CallbackContext context) {
        currentRotation = 0f;
    }

    private void HandleImpulse(InputAction.CallbackContext context) {
        currentImpulse = impulseForce;
    }

    private void HandleImpulseStop(InputAction.CallbackContext context) {
        currentImpulse = 0f;
    }

    private void HandleHyperspace(InputAction.CallbackContext context) {
        isUsingHyperspace = true;
    }


    private void HandleHyperspaceStop(InputAction.CallbackContext context) {
        isUsingHyperspace = false;
    }

    private void HandleShoot(InputAction.CallbackContext context) {
        isShooting = true;
    }

    private void HandleShootStop(InputAction.CallbackContext context) {
        isShooting = false;
    }

    private void Shoot() {
        var bullet = Instantiate(bulletPrefab, transform.position + transform.up * CANON_DISTANCE, transform.rotation);
        if (bullet.TryGetComponent<BulletController>(out var bulletController)) bulletController.Initialize(this);

        canShoot = false;
        Invoke(nameof(EnableShooting), shootDelay);
    }

    private void Hyperspace() {
        transform.position = GetRandomWorldPoint();
        canUseHyperspace = false;
        Invoke(nameof(EnableHyperspace), hyperspaceDelay);
    }

    private void EnableShooting() {
        canShoot = true;
    }

    private void EnableHyperspace() {
        canUseHyperspace = true;
    }

    private void EnableRespawn() {
        canBeRespawned = true;
    }
    #endregion

    #region Auxiliar functions
    private Vector2 GetRandomWorldPoint() {
        var camHeight = mainCamera.orthographicSize;
        var camWidth = camHeight * mainCamera.aspect;

        return new Vector2(Random.Range(-camWidth, camWidth), Random.Range(-camHeight, camHeight));
    }
    #endregion
}
