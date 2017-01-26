using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour {
    private GameObject player = null;
    private Vector3 chasePoint;
    private bool PlayerFound = false;
    private bool Chasing = false;
    private Rigidbody rb = null;
    private Vector3 StartPos;
    private float PlayerDist = float.PositiveInfinity;
    private bool IsBouncing;
    private float lastTime = 0;

    // Unity Vars
    public float ChaseDistance;
    public float ChaseSpeed;
    public float BounceDistance;
    public float BounceFreq;
    public float BounceForce;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        rb = this.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        // print("Player: " + this.PlayerFound.ToString() + ", Chasing: " + this.Chasing.ToString());
        CheckForPlayer();
        CheckBounceForPlayer();

        if (Chasing) {
            CheckStopChase();
        }
        //if (IsBouncing) {
        //    print("test");
        //    Bounce();
        //}
	}

    private void Bounce() {
        if ((Time.time - lastTime) > BounceFreq) {
            rb.AddForce(0, BounceForce, 0);
            lastTime = Time.time;
        }
    }

    private void StartChase() {
        this.Chasing = true;
        StartPos = this.transform.position;
        int dir = Math.Sign((chasePoint - StartPos).x);
        rb.velocity = new Vector3(ChaseSpeed * dir, rb.velocity.y, rb.velocity.z);
    }

    private void CheckStopChase() {
        Vector3 chaseToStart = chasePoint - StartPos;
        Vector3 currToStart = this.transform.position - StartPos;

        if (Vector3.Magnitude(currToStart) >= Vector3.Magnitude(chaseToStart)) {
            rb.velocity = new Vector3(0.0f, rb.velocity.y, rb.velocity.z);
            this.Chasing = false;
        }
    }

    private void CheckForPlayer() {
        Vector3 playPos = this.player.transform.position;

        this.PlayerDist = playPos.x - this.transform.position.x;
        int dir = Math.Sign(PlayerDist);

        // print((distanceHoriz * dir).ToString() + ", " + ChaseDistance.ToString());

        if (PlayerDist * dir < ChaseDistance) {
            this.PlayerFound = true;
            chasePoint = this.player.transform.position;
            StartChase();
        } else {
            this.PlayerFound = false;
        }
    }

    private void CheckBounceForPlayer() {
        //print("test2" + Math.Abs(PlayerDist).ToString());

        if (!IsBouncing && (Math.Abs(PlayerDist) <= BounceDistance)){
            IsBouncing = true;
            Bounce();
        } else {
            IsBouncing = false;
        }
    }
}
