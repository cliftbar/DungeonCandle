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

    // Use this for initialization
    void Start () {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update () {
        Vector3 center = playerTransform.position + new Vector3(0f, cameraHeight, -1 * cameraDepth);

        Vector3 target;

        if (transform.position.x < cameraMin.x) {
            target.x = Mathf.Min(transform.position.x + 0.4f, cameraMin.x);
        } else if (transform.position.x > cameraMax.x) {
            target.x = Mathf.Max(transform.position.x - 0.4f, cameraMax.x);
        } else if (transform.position.x < center.x - cameraPlayerBound.x && center.x - cameraPlayerBound.x < cameraMax.x) {
            target.x = Mathf.Min(transform.position.x + 0.4f, center.x - cameraPlayerBound.x);
        } else if (transform.position.x > center.x + cameraPlayerBound.x && center.x + cameraPlayerBound.x > cameraMin.x) {
            target.x = Mathf.Max(transform.position.x - 0.4f, center.x + cameraPlayerBound.x);
        } else {
            target.x = transform.position.x;
        }

        if (transform.position.y < cameraMin.y) {
            target.y = Mathf.Min(transform.position.y + 0.1f, cameraMin.y);
        } else if (transform.position.y > cameraMax.y) {
            target.y = Mathf.Max(transform.position.y - 0.1f, cameraMax.y);
        } else if (transform.position.y < center.y - cameraPlayerBound.y && center.y - cameraPlayerBound.y < cameraMax.y) {
            target.y = center.y - cameraPlayerBound.y;
        } else if (transform.position.y > center.y + cameraPlayerBound.y && center.y + cameraPlayerBound.y > cameraMin.y) {
            target.y = center.y + cameraPlayerBound.y;
        } else {
            target.y = transform.position.y;
        }

        transform.position = new Vector3 (target.x, target.y, center.z);
    }
}
