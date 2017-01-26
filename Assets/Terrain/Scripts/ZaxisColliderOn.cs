using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZaxisColliderOn : MonoBehaviour {

    public List<float> z_coords_on;

	// Use this for initialization
	void Start () {
        SolidifyTerrain();
	}
	
    void SolidifyTerrain() {
        foreach (Transform child in this.transform) {
            Collider childCol = child.GetComponent<Collider>();
            foreach (float z in z_coords_on) {
                if (childCol.bounds.min.z <= z && z <= childCol.bounds.max.z) {
                    childCol.enabled = true;
                }
            }
        }
    }
}
