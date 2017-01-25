using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBouncer : MonoBehaviour {

    private Rigidbody rb = null;
    private int framesLeft = 0;
    private float lastTime = 0.0f;
    private float thisTime = 0.0f;

    // public and uninitialized for Unity
    public float speed;
    public int MovementFrames;
    public int bounceForce;
    public int movementTime;

    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(speed * -1f, 0f, 0f);

        framesLeft = MovementFrames;

        lastTime = Time.time;
    }
    
    // Update is called once per frame
    void Update () {
        thisTime = Time.time;
        TurnCheck();
    }

    void TurnCheck() {
        if (lastTime + movementTime < Time.time) {
            turnAround();
            Bounce();
            lastTime = Time.time;
        }
    }

    private void turnAround() {
        rb.velocity = new Vector3(rb.velocity.x * -1, rb.velocity.y, rb.velocity.z);
    }

    private void Bounce() {
        rb.AddForce(0, bounceForce, 0);
    }
}
