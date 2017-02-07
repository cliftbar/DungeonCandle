using UnityEngine;
using System.Collections;

public class SimplerWalker : MonoBehaviour {
    private SpriteRenderer sr;
    private Rigidbody rb;
    private TerrainDetector cd;
    private TerrainDetector wd;
    private Vitality vt;
    private Pauser pa;

    public bool startTurned;
    public float speed;

    void Awake () {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
        vt = GetComponent<Vitality>();
        pa = GetComponent<Pauser>();

        foreach (Transform child in transform) {
            if (child.gameObject.name == "Cliff Detector") {
                cd = child.gameObject.GetComponent<TerrainDetector>();
            } else if (child.gameObject.name == "Wall Detector") {
                wd = child.gameObject.GetComponent<TerrainDetector>();
            }
        }
    }

    // Use this for initialization
    void Start () {
        if (startTurned == true) {
            TurnAround();
        }
        SetSpeed();
    }

    // Update is called once per frame
    void Update () {
        
    }

    void FixedUpdate () {
        if (pa.Paused() == false) {
            if (vt.currentLife > 0) {
                SetSpeed();
                if (wd.Detected() == true || cd.Detected() == false) {
                    TurnAround();
                }
            } else {
                rb.velocity = new Vector3(0f, 0f, 0f);
            }
        }
    }

    void SetSpeed () {
        if (sr.flipX == true) {
            rb.velocity = new Vector3(speed, 0f, 0f);
        } else {
            rb.velocity = new Vector3(speed * -1f, 0f, 0f);
        }
    }

    void TurnAround () {
        sr.flipX = !sr.flipX;

        foreach (Transform child in transform) {
            child.localPosition = new Vector3(-1 * child.localPosition.x, child.localPosition.y, child.localPosition.z);
        }
    }
}
