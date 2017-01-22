using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBouncer : MonoBehaviour {

    private Rigidbody rb = null;
    private int framesLeft;

    // public and uninitialized for Unity
    public float speed;
    public int MovementFrames;
    public int bounceForce;

    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(speed * -1f, 0f, 0f);

        framesLeft = MovementFrames;
    }
    
    // Update is called once per frame
    void Update () {
        if (framesLeft == 0) {
            turnAround();
            Bounce();
            framesLeft = MovementFrames;
        } else {
            --framesLeft;
        }
    }

    private void turnAround() {
        rb.velocity = new Vector3(rb.velocity.x * -1, rb.velocity.y, rb.velocity.z);
    }

    private void Bounce() {
        rb.AddForce(0, bounceForce, 0);
    }
}
