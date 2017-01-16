using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private Transform playerTransform;

	// Standard offset from player position:
	public float cameraHeight;
	public float cameraDepth;

	// Boundaries around player within which the camera doesn't move:
	public Vector2 cameraBound;

	// Use this for initialization
	void Start () {
		playerTransform = GameObject.Find("Player").GetComponent<Transform>();
	}

	// Update is called once per frame
	void Update () {
		Vector3 center = playerTransform.position + new Vector3(0f, cameraHeight, -1 * cameraDepth);

		Vector3 target;

		if (transform.position.x > center.x + cameraBound.x) {
			target.x = center.x + cameraBound.x;
		} else if (transform.position.x < center.x - cameraBound.x) {
			target.x = center.x - cameraBound.x;
		} else {
			target.x = transform.position.x;
		}

		if (transform.position.y > center.y + cameraBound.y) {
			target.y = center.y + cameraBound.y;
		} else if (transform.position.y < center.y - cameraBound.y) {
			target.y = center.y - cameraBound.y;
		} else {
			target.y = transform.position.y;
		}

		transform.position = new Vector3 (target.x, target.y, center.z);
	}
}
