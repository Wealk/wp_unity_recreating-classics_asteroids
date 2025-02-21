using UnityEngine;

public class OVNIController : MonoBehaviour {
    private const float CANON_DISTANCE = 0.75f;

    // Editor variables
    [SerializeField]
    private Vector2 speedRange;
    [SerializeField]
    private Vector2 changeDirectionRange;
    [SerializeField]
    private int points;
    [SerializeField]
    private AudioClip destroyEffect;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float shootDelay;
    [SerializeField]
    private bool shouldShootToPlayer;
    [SerializeField, Range(0, 1)]
    private float aimPrecision;

    // Private variables
    private new Rigidbody2D rigidbody;
    private Transform playerPosition;
    private float movementSpeed;
    private float currentShootTime;
    private float currentChangeDirection;

    #region Unity methods
    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        playerPosition = GetPlayerPosition();
        movementSpeed = Random.Range(speedRange.x, speedRange.y);
        currentShootTime = shootDelay;
        currentChangeDirection = Random.Range(changeDirectionRange.x, changeDirectionRange.y);

        var mainCamera = Camera.main;
        var cameraWidth = mainCamera.orthographicSize * mainCamera.aspect;
        var cameraHeight = mainCamera.orthographicSize;
        var startY = Random.Range(-cameraHeight, cameraHeight);

        if (Random.Range(0, 2) == 0) {
            transform.position = new Vector2(-cameraWidth, startY);
            rigidbody.linearVelocity = Vector2.right * movementSpeed;
        } else {
            transform.position = new Vector2(cameraWidth, startY);
            rigidbody.linearVelocity = Vector2.left * movementSpeed;
        }
    }

    private void Update() {
        var delta = Time.deltaTime;
        currentShootTime -= delta;
        if (currentShootTime <= 0) Shoot();

        currentChangeDirection -= delta;
        if (currentChangeDirection <= 0) ChangeDirection();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("PlayerBullet") && collision.collider.TryGetComponent<BulletController>(out var bullet) && bullet.Owner != null)
            bullet.Owner.AddScore(points);

        AudioController.Instance.PlayOnce(destroyEffect);
        Destroy(gameObject);
    }
    #endregion

    #region Private methods
    private void Shoot() {
        var direction = shouldShootToPlayer ? GetPlayerTargetShootDirection() : GetRandomShootDirection();
        var bulletPosition = transform.position + (Vector3)direction * CANON_DISTANCE;
        Instantiate(bulletPrefab, bulletPosition, Quaternion.FromToRotation(Vector2.up, direction));

        currentShootTime = shootDelay;
    }

    private void ChangeDirection() {
        rigidbody.linearVelocity = GetRandomMovementDirection(rigidbody.linearVelocityX > 0) * movementSpeed;
        currentChangeDirection = Random.Range(changeDirectionRange.x, changeDirectionRange.y);
    }
    #endregion

    #region Auxiliar functions
    private Vector2 GetRandomMovementDirection(bool isGoingRight) {
        return new Vector2(isGoingRight ? 1 : -1, Random.Range(-1f, 1f)).normalized;
    }

    private Vector2 GetRandomShootDirection() {
        return Random.insideUnitCircle.normalized;
    }

    private Vector2 GetPlayerTargetShootDirection() {
        if (playerPosition != null) {
            var playerDirection = (playerPosition.position - transform.position).normalized;
            var randomOffset = Random.insideUnitCircle;

            return Vector2.Lerp(randomOffset, playerDirection, aimPrecision).normalized;
        }

        return Random.insideUnitCircle.normalized;
    }

    private Transform GetPlayerPosition() {
        var players = GameObject.FindGameObjectsWithTag("Player");
        if (players != null && players.Length > 0)
            return players[Random.Range(0, players.Length)].transform;

        return null;
    }
    #endregion
}
