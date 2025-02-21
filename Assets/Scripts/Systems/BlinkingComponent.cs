using UnityEngine;

public class BlinkingComponent : MonoBehaviour {
    // Editor variables
    [SerializeField]
    private MonoBehaviour component;
    [SerializeField]
    private float toggleTime;

    #region Unity methods
    private void Awake() {
        InvokeRepeating(nameof(ToggleVisibility), toggleTime, toggleTime);
    }
    #endregion

    #region Private methods
    private void ToggleVisibility() {
        component.enabled = !component.enabled;
    }
    #endregion
}
