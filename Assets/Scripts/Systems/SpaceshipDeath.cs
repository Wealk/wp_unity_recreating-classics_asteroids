using UnityEngine;

public class SpaceshipDeath : MonoBehaviour {
    // Editor variables
    [SerializeField]
    private Vector2 sectionVisibilityDuration;
    [SerializeField]
    private Vector2 forceRange;

    #region Unity methods
    private void Awake() {
        var sections = GetComponentsInChildren<Rigidbody2D>();
        foreach (var section in sections) {
            section.AddForce(Random.insideUnitCircle.normalized * Random.Range(forceRange.x, forceRange.y));
            Destroy(section.gameObject, Random.Range(sectionVisibilityDuration.x, sectionVisibilityDuration.y));
        }

        Destroy(gameObject, sectionVisibilityDuration.y);
    }
    #endregion
}
