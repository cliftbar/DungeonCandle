using UnityEngine;
using System.Collections;

public class BeaconTorch : MonoBehaviour {
	private Animator anim;
	private GameObject lightObject;
	public bool lit;

	void Awake () {
		anim = GetComponent<Animator>();
		lightObject = GetComponentInChildren<Light>().gameObject;
	}

	// Use this for initialization
	void Start () {
		if (lit == true) {
			LightFlame();
		} else {
			PutOutFlame();
		}
	}

	void OnEnable () {
		Start();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void LightFlame () {
		lit = true;
		anim.SetBool("lit", true);
		lightObject.SetActive(true);
	}

	void PutOutFlame () {
		lit = false;
		anim.SetBool("lit", false);
		lightObject.SetActive(false);
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.name == "Candle Hitbox") {
			if (lit == false && other.transform.parent.GetComponent<PlayerController>().CandleLit() == true) {
				LightFlame();
			} else if (lit == true && other.transform.parent.GetComponent<PlayerController>().CandleLit() == false) {
				other.transform.parent.GetComponent<PlayerController>().LightCandle();
			}
		}

		if (other.transform.parent != null) {
			if (other.transform.parent.gameObject.name == "Player Flame Attack" && lit == false) {
				LightFlame();
			}
		}
	}
}
