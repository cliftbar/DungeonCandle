using UnityEngine;
using System.Collections;

public class SimplerWalker : MonoBehaviour {
    private SpriteRenderer sr;
    private Rigidbody rb;
    private TerrainDetector cd;
    private TerrainDetector wd;
    private Vitality vt;

    public float speed;

    void Awake () {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
        vt = GetComponent<Vitality>();

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
        rb.velocity = new Vector3(speed * -1f, 0f, 0f);
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    void FixedUpdate () {
        if (vt.currentLife > 0) {
            if (wd.detected == true || cd.detected == false) {
                TurnAround();
            }
        } else {
            rb.velocity = new Vector3(0f, 0f, 0f);
        }
    }

    void TurnAround () {
        rb.velocity = new Vector3(rb.velocity.x * -1f, rb.velocity.y, 0f);
        sr.flipX = !sr.flipX;

        foreach (Transform child in transform) {
            child.localPosition = new Vector3(-1 * child.localPosition.x, child.localPosition.y, child.localPosition.z);
        }
    }
}
