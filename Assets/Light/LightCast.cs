using UnityEngine;
using System.Collections;

public class LightCast : MonoBehaviour {
    private Transform playerTransform;
    private PlayerController pc;

    private bool illum;
    public float radius;

    void Awake () {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        pc = playerTransform.GetComponent<PlayerController>();
    }

    void FixedUpdate () {
        if (pc.CandleLit() == false) {
            UpdateIllum();
        } else {
            RemoveIllum();
        }
    }

    void UpdateIllum () {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, playerTransform.position - transform.position, out hit, radius)) {
            if (hit.collider.gameObject.name == "Player") {
                AddIllum();
            } else {
                RemoveIllum();
            }
        } else {
            RemoveIllum();
        }
    }

    void AddIllum () {
        if (illum == false) {
            illum = true;
            pc.illumCount += 1;
        }
    }

    void RemoveIllum () {
        if (illum == true) {
            illum = false;
            pc.illumCount -= 1;
        }
    }
}
