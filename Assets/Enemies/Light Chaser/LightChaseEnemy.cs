using UnityEngine;
using System.Collections;

public class LightChaseEnemy : MonoBehaviour {
	private Animator anim;
	private Rigidbody rb;
	private SpriteRenderer sr;
	private Vitality vt;
	private TerrainDetector td;

	// Variables for searching for the player:
	private PlayerController pc;
	public LayerMask playerSearchLayers;
	public float playerSearchDistance;

	// Variables for setting z-coordinates:
	public float z;
	public float zSleepOffset;

	// States:
	//  -1 = dead
	//   0 = sleep
	//   1 = waking
	//   2 = awake and charging
	//   3 = pausing before charging again
	//   4 = going back to sleep
	private int state = 0;

	// Variables for wake/charge/sleep behaviour:
	private float wakeTimestamp;
	public float wakeTime;
	private float chargeTimestamp;
	public float chargeTime;
	private float stopTimestamp;
	public float stopTime;
	private float sleepTimestamp;
	public float sleepTime;

	// Movement Variables
	public float accel;
	public float maxSpeed;

	void Awake () {
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		sr = GetComponent<SpriteRenderer>();
		vt = GetComponent<Vitality>();
		pc = GameObject.Find("Player").GetComponent<PlayerController>();
		td = GetComponentInChildren<TerrainDetector>();
	}

	// Use this for initialization
	void Start () {
		transform.position = new Vector3(transform.position.x, transform.position.y, z + zSleepOffset);
	}

	// Update is called once per frame
	void Update () {
		if (vt.currentLife > 0 && state != -1) {
			if (state == 0 && AggroTriggered() == true) {
				Wake();
			} else if (state == 1 && Time.time >= wakeTimestamp + wakeTime) {
				if (AggroTriggered() == true) {
					StartCharge(true);
				} else {
					Sleep();
				}
			} else if (state == 2) {
				if (AggroTriggered() == false) {
					StartSleep();
				} else if (Time.time >= chargeTimestamp + chargeTime || td.detected == true) {
					StopCharge();
				}
			} else if (state == 3 && Time.time >= stopTimestamp + stopTime) {
				StartCharge(false);
			} else if (state == 4 && Time.time >= sleepTimestamp + sleepTime) {
				Sleep();
			}
		} else {
			Die();
		}
	}

	void FixedUpdate () {
		if (state == 2 && AggroTriggered() == true) {
			UpdateVelocity();
			UpdateZ(z, 0.05f);
		} else if (state == 4) {
			SlowDown(0.02f);
			UpdateZ(z + zSleepOffset, 0.02f);
		}
	}

	void SlowDown (float amount) {
		if (Mathf.Abs(rb.velocity.x) <= 0.1f) {
			rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);
		} else {
			if (rb.velocity.x > 0f) {
				amount *= -1f;
			}

			rb.velocity = new Vector3(rb.velocity.x + amount, rb.velocity.y, rb.velocity.z);
		}
	}

	void UpdateZ (float targetZ, float increment) {
		float newZ;
		if (transform.position.z > targetZ) {
			newZ = Mathf.Max(transform.position.z - increment, targetZ);
		} else {
			newZ = Mathf.Min(transform.position.z + increment, targetZ);
		}

		transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
	}

	void UpdateVelocity () {
		float forceX = accel;
		if (sr.flipX == false) {
			forceX *= -1f;
		}

		rb.AddForce(forceX, 0f, 0f);

		if (rb.velocity.x > maxSpeed) {
			rb.velocity = new Vector3(maxSpeed, rb.velocity.y, rb.velocity.z);
		} else if (rb.velocity.x < maxSpeed * -1f) {
			rb.velocity = new Vector3(maxSpeed * -1f, rb.velocity.y, rb.velocity.z);
		}
	}

	void UpdateFacing () {
		if (sr.flipX == false && pc.transform.position.x > transform.position.x) {
			TurnAround();
		} else if (sr.flipX == true && pc.transform.position.x < transform.position.x) {
			TurnAround();
		}
	}

	void TurnAround () {
		rb.velocity = new Vector3(rb.velocity.x * 0.4f, rb.velocity.y, rb.velocity.z);
		sr.flipX = !sr.flipX;

		foreach (Transform child in transform) {
			child.localPosition = new Vector3(-1 * child.localPosition.x, child.localPosition.y, child.localPosition.z);
		}
	}

	bool AggroTriggered () {
		if (pc.Illuminated() == true) {
			if (Vector3.Distance(pc.transform.position, transform.position) < 1.5) {
				return true;
			} else {
				RaycastHit hit;

				if (Physics.Raycast(transform.position, pc.transform.position - transform.position, out hit, playerSearchDistance, playerSearchLayers)) {
					if (hit.collider.gameObject.name == "Player") {
						return true;
					}
				}
			}

		}

		return false;
	}

	void Wake () {
		state = 1;
		wakeTimestamp = Time.time;
		anim.SetTrigger("wake");
	}

	void StartCharge (bool jump) {
		UpdateFacing();
		state = 2;
		if (jump == true) {
			rb.velocity = new Vector3(0f, 3f, 0f);
		}
		anim.SetTrigger("chase");
		chargeTimestamp = Time.time;
	}

	void StopCharge () {
		state = 3;
		rb.velocity = new Vector3(0f, 0f, 0f);
		anim.SetTrigger("wake");
		stopTimestamp = Time.time;
	}

	void StartSleep () {
		state = 4;
		sleepTimestamp = Time.time;
	}

	void CancelSleep () {
		state = 2;
	}

	void Sleep () {
		state = 0;
		anim.SetTrigger("sleep");

		rb.velocity = new Vector3(0f, 0f, 0f);
		transform.position = new Vector3(transform.position.x, transform.position.y, z + zSleepOffset);
	}

	void Die () {
		state = -1;
		rb.velocity = new Vector3(0f, 0f, 0f);
	}
}
