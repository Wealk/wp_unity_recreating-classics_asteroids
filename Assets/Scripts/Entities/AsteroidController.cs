using UnityEngine;

public class AsteroidController : MonoBehaviour {
    private const int SMALLER_SECTIONS = 2;

    // Editor variables
    [SerializeField]
    private Sprite[] asteroidSprites;
    [SerializeField]
    private Vector2 speedRange;
    [SerializeField]
    private int points;
    [SerializeField]
    private AudioClip destroySound;
    [SerializeField]
    private GameObject particlesEffect;
    [SerializeField]
    private GameObject smallerAsteroid;

    // Private variables
    private new SpriteRenderer renderer;
    private new Rigidbody2D rigidbody;
    private new AudioSource audio;

    #region Unity methods
    private void Awake() {
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        renderer.sprite = asteroidSprites[Random.Range(0, asteroidSprites.Length)];
        rigidbody.linearVelocity = Random.insideUnitCircle.normalized * Random.Range(speedRange.x, speedRange.y);

        if (GameController.Instance != null)
            GameController.Instance.AddAsteroid();
    }

    private void OnDestroy() {
        if (GameController.Instance != null)
            GameController.Instance.RemoveAsteroid();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("PlayerBullet") && collision.collider.TryGetComponent<BulletController>(out var bullet) && bullet.Owner != null)
            bullet.Owner.AddScore(points);

        if (smallerAsteroid != null)
            InstantiateSmallerAsteroids();

        AudioController.Instance.PlayOnce(destroySound);
        Destroy(Instantiate(particlesEffect, transform.position, Quaternion.identity), 1);
        Destroy(gameObject);
    }

    private void InstantiateSmallerAsteroids() {
        for (var i = 0; i < SMALLER_SECTIONS; i++)
            Instantiate(smallerAsteroid, transform.position, Quaternion.identity);
    }
    #endregion
}
