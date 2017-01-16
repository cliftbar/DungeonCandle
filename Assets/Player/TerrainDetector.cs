using UnityEngine;
using System.Collections;

public class TerrainDetector : MonoBehaviour {
	private int collisionCount;
	public bool detected;

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Terrain") {
			collisionCount += 1;
			UpdateDetection();
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == "Terrain") {
			collisionCount -= 1;
			UpdateDetection();
		}
	}

	void UpdateDetection () {
		if (collisionCount > 0) {
			detected = true;
		} else {
			detected = false;
		}
	}
}
