using UnityEngine;
using System.Collections;

public class PlayerFlameAttack : MonoBehaviour {
	private SpriteRenderer sr;
	private HitboxController hc;
	private Transform playerTransform;
	private PlayerController pc;
	private float createTimestamp;

	public float lifetime;

	void Awake () {
		sr = GetComponent<SpriteRenderer>();
		hc = GetComponent<HitboxController>();
	}

	// Use this for initialization
	void Start () {
		createTimestamp = Time.time;
		hc.StartHitboxProcess();
	}

	// Called by PlayerController when PlayerFlameAttack is created
	public void Initialize(Transform t) {
		playerTransform = t;
		pc = t.GetComponent<PlayerController>();
		pc.illumCount += 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= createTimestamp + lifetime) {
			pc.illumCount -= 1;
			Destroy(gameObject);
		}
	}

	void FixedUpdate () {
		if (Time.time <= createTimestamp + lifetime / 2) {
			if (sr.flipX == false && transform.position.x < playerTransform.position.x) {
				transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
			} else if (sr.flipX == true && transform.position.x > playerTransform.position.x) {
				transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
			}
		}
	}

	public void TurnAround () {
		sr.flipX = !sr.flipX;

		foreach (Transform child in transform) {
			child.localPosition = new Vector3(-1 * child.localPosition.x, child.localPosition.y, child.localPosition.z);
		}
	}
}
