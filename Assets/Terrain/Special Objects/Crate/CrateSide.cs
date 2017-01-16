using UnityEngine;
using System.Collections;

public class CrateSide : MonoBehaviour {
	private Rigidbody rb;
	private ParticleSystem ps;

	private float color = 1f;

	void Awake () {
	}

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay (Collider other) {
        if (other.transform.parent != null) {
            if (other.transform.parent.gameObject.name == "Player Flame Attack") {
                Push(other.transform.position);
                Blacken();
            }
        }
    }

    void Push (Vector3 source) {
        rb.velocity = (transform.position - source) * 3 / Vector3.Distance(transform.position, source);
    }

    void Blacken () {
    	color = Mathf.Max(color - 0.3f, 0.1f);
    	foreach (Transform child in transform) {
    		child.GetComponent<MeshRenderer>().material.color = new Color(color, color, color, 1f);
    	}
    }
}
