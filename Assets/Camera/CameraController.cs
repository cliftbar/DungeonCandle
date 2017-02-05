using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    private Transform playerTransform;

    // Standard offset from player position:
    public float cameraHeight;
    public float cameraDepth;

    // Boundaries around player within which the camera doesn't move:
    public Vector2 cameraPlayerBound;

    // Hard boundaries outside which the camera can't move:
    public Vector2 cameraMin;
    public Vector2 cameraMax;

    // Variables for making the camera focus on a point:
    private bool focused;
    private float savedCameraHeight;
    private float savedCameraDepth;
    private Vector2 savedCameraMin;
    private Vector2 savedCameraMax;

    // Scroll rate:
    public float defaultScrollRate;
    public float defaultZoomRate;
    private float scrollRate;
    private float zoomRate;

    void Awake () {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    public void Initialize () {
        transform.position = playerTransform.position + new Vector3(0f, cameraHeight, -1 * cameraDepth);

        scrollRate = defaultScrollRate;
        zoomRate = defaultZoomRate;
    }

    // Update is called once per frame
    void Update () {
        ScrollTowardsPlayer();
    }

    private void ScrollTowardsPlayer () {
        Vector3 center = playerTransform.position + new Vector3(0f, cameraHeight, -1 * cameraDepth);

        Vector3 target;

        // Update camera x:
        if (transform.position.x < cameraMin.x) {
            target.x = Mathf.Min(transform.position.x + scrollRate, cameraMin.x);
        } else if (transform.position.x > cameraMax.x) {
            target.x = Mathf.Max(transform.position.x - scrollRate, cameraMax.x);
        } else if (transform.position.x < center.x - cameraPlayerBound.x && center.x - cameraPlayerBound.x < cameraMax.x) {
            target.x = Mathf.Min(transform.position.x + scrollRate, center.x - cameraPlayerBound.x);
        } else if (transform.position.x > center.x + cameraPlayerBound.x && center.x + cameraPlayerBound.x > cameraMin.x) {
            target.x = Mathf.Max(transform.position.x - scrollRate, center.x + cameraPlayerBound.x);
        } else {
            target.x = transform.position.x;
        }

        // Update camera y:
        if (transform.position.y < cameraMin.y) {
            target.y = Mathf.Min(transform.position.y + scrollRate, cameraMin.y);
        } else if (transform.position.y > cameraMax.y) {
            target.y = Mathf.Max(transform.position.y - scrollRate, cameraMax.y);
        } else if (transform.position.y < center.y - cameraPlayerBound.y && center.y - cameraPlayerBound.y < cameraMax.y) {
            target.y = Mathf.Min(transform.position.y + scrollRate, center.y - cameraPlayerBound.y);
        } else if (transform.position.y > center.y + cameraPlayerBound.y && center.y + cameraPlayerBound.y > cameraMin.y) {
            target.y = Mathf.Max(transform.position.y - scrollRate, center.y + cameraPlayerBound.y);
        } else {
            target.y = transform.position.y;
        }

        // Update camera z:
        if (transform.position.z < center.z) {
            target.z = Mathf.Min(transform.position.z + zoomRate, center.z);
        } else {
            target.z = Mathf.Max(transform.position.z - zoomRate, center.z);
        }

        transform.position = target;
    }

    public void FocusCamera (Vector2 focusPoint, float focusDepth, float customScrollRate = -1f, float customZoomRate = -1f) {
        focused = true;

        savedCameraHeight = cameraHeight;
        savedCameraDepth = cameraDepth;
        savedCameraMin = cameraMin;
        savedCameraMax = cameraMax;

        cameraHeight = 0f;
        cameraDepth = focusDepth;
        cameraMin = focusPoint;
        cameraMax = focusPoint;

        if (customScrollRate != -1f) {
            scrollRate = customScrollRate;
        }

        if (customZoomRate != -1f) {
            zoomRate = customZoomRate;
        }
    }

    public void ReleaseCamera () {
        focused = false;

        cameraHeight = savedCameraHeight;
        cameraDepth = savedCameraDepth;
        cameraMin = savedCameraMin;
        cameraMax = savedCameraMax;

        scrollRate = defaultScrollRate;
        zoomRate = defaultZoomRate;
    }
}
