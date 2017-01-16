using UnityEngine;
using System.Collections;

public class TimedDeath : MonoBehaviour {
    private float startTimestamp;
    public float deathTime;

    // Use this for initialization
    void Start () {
        startTimestamp = Time.time;
    }
    
    // Update is called once per frame
    void Update () {
        if (Time.time >= startTimestamp + deathTime) {
            Destroy(gameObject);
        }
    }
}
