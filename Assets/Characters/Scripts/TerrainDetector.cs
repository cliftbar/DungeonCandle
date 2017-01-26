using UnityEngine;
using System.Collections;

public class TerrainDetector : MonoBehaviour {
	private int collisionCount;
    private bool detected;
    public bool detectEnemies;

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Terrain") {
			collisionCount += 1;
			UpdateDetection();
		} else if (detectEnemies == true && other.gameObject.layer == 11) {
            collisionCount += 1;
            UpdateDetection();
        }
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == "Terrain") {
			collisionCount -= 1;
			UpdateDetection();
		} else if (detectEnemies == true && other.gameObject.layer == 11) {
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

    public bool Detected () {
        return detected;
    }
}
