using UnityEngine;
using System.Collections;

public class DeathRegion : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.name == "Player") {
            other.gameObject.GetComponent<PlayerController>().StartDeath(true);
        }
    }
}
