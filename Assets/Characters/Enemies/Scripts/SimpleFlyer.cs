using UnityEngine;
using System.Collections;

public class SimpleFlyer : MonoBehaviour {
    private Rigidbody rb;
    private Vitality vt;
    private SpriteRenderer sr;
    private PlayerController pc;

    private bool triggered = false;
    private float direction;
    public float aggroRadius;
    public float speed;

    public float z;

    void Awake () {
        rb = GetComponent<Rigidbody>();
        vt = GetComponent<Vitality>();
        sr = GetComponentInChildren<SpriteRenderer>();
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Use this for initialization
    void Start () {
    }

    void FixedUpdate () {
        transform.position = new Vector3 (transform.position.x, transform.position.y, z);
        if (vt.currentLife == 0) {
            rb.velocity = new Vector3(0f, 0f, 0f);
        } else if (triggered == true) {
            rb.velocity = new Vector3(speed * direction, 0f, 0f);
        } else if (Vector3.Distance(pc.transform.position, transform.position) <= aggroRadius) {
            triggered = true;
            if (pc.transform.position.x <= transform.position.x) {
                direction = -1f;
            } else {
                TurnAround();
                direction = 1f;
            }
        }
    }

    void TurnAround () {
        sr.flipX = true;
    }
}
