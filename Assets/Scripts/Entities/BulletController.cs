using UnityEngine;

public class BulletController : MonoBehaviour {

    // Public properties
    public SpaceshipController Owner => spaceshipOwner;

    // Editor variables
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float activeTime;

    // Private variables
    private SpaceshipController spaceshipOwner;
    private new Rigidbody2D rigidbody;
    private float currentTime;

    #region Unity methods
    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        currentTime = activeTime;
    }

    private void Start() {
        rigidbody.linearVelocity = transform.up * movementSpeed;
    }

    private void Update() {
        if (currentTime > 0) {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0) Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }
    #endregion

    #region Public calls
    public void Initialize(SpaceshipController owner) {
        spaceshipOwner = owner;
    }
    #endregion
}
