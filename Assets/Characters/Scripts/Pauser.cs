using UnityEngine;
using System.Collections;

public class Pauser : MonoBehaviour {
    private Rigidbody rb;
    private Animator anim;
    private SceneController sc;

    private bool paused;
    private bool savedGravity;
    private Vector3 savedVelocity;

    void Awake () {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
        sc = GameObject.Find("Scene Controller").GetComponent<SceneController>();
    }
    
    // Update is called once per frame
    void Update () {
        if (sc.paused == true && paused == false) {
            Pause();
        } else if (sc.paused == false && paused == true) {
            Unpause();
        }
    }

    private void Pause () {
        paused = true;

        savedVelocity = rb.velocity;
        savedGravity = rb.useGravity;

        rb.velocity *= 0f;
        rb.useGravity = false;
        rb.isKinematic = true;

        anim.enabled = false;
    }

    private void Unpause () {
        paused = false;

        rb.velocity = savedVelocity;
        rb.useGravity = savedGravity;
        rb.isKinematic = false;

        anim.enabled = true;
    }

    public bool Paused () {
        return paused;
    }
}
