using UnityEngine;

public class AudioController : MonoBehaviour {
    // Global properties
    private static AudioController instance;
    public static AudioController Instance => instance;

    // Private variables
    private new AudioSource audio;

    #region Unity methods
    private void Awake() {
        if (instance == null) {
            instance = this;
            audio = GetComponent<AudioSource>();
        } else Destroy(this);
    }
    #endregion

    #region Public calls
    public void PlayOnce(AudioClip effect) {
        audio.PlayOneShot(effect);
    }
    #endregion
}
