using System;
using UnityEngine;

public class ScreenWrapping : MonoBehaviour {
    // Private variables
    private Vector2 screenBounds;
    private Vector2 objectSize;

    #region Unity methods
    private void Awake() {
        var mainCamera = Camera.main;
        var camHeight = mainCamera.orthographicSize;
        var camWidth = camHeight * mainCamera.aspect;
        screenBounds = new Vector2(camWidth, camHeight);

        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) objectSize = new Vector2(spriteRenderer.bounds.extents.x, spriteRenderer.bounds.extents.y);
    }

    private void Start() {
        EnsureInsideScreen();
    }

    private void OnBecameInvisible() {
        var newPosition = transform.position;

        if (transform.position.x > screenBounds.x + objectSize.x) newPosition.x = -screenBounds.x - objectSize.x;
        else if (transform.position.x < -screenBounds.x - objectSize.x) newPosition.x = screenBounds.x + objectSize.x;

        if (transform.position.y > screenBounds.y + objectSize.y) newPosition.y = -screenBounds.y - objectSize.y;
        else if (transform.position.y < -screenBounds.y - objectSize.y) newPosition.y = screenBounds.y + objectSize.y;

        transform.position = newPosition;
    }
    #endregion


    #region Private methods
    private void EnsureInsideScreen() {
        var newPosition = transform.position;
       
        if (newPosition.x > screenBounds.x + objectSize.x) newPosition.x = screenBounds.x - objectSize.x;
        else if (newPosition.x < -screenBounds.x - objectSize.x) newPosition.x = -screenBounds.x + objectSize.x;

        if (newPosition.y > screenBounds.y + objectSize.y) newPosition.y = screenBounds.y - objectSize.y;
        else if (newPosition.y < -screenBounds.y - objectSize.y) newPosition.y = -screenBounds.y + objectSize.y;

        if (newPosition != transform.position)
            transform.position = newPosition;
    }
    #endregion
}
