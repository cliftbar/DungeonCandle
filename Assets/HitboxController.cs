using UnityEngine;
using System.Collections;

public class HitboxController : MonoBehaviour {
    private bool processing;
    private float startTimestamp;
    private bool[] hitboxStarted;
    private bool[] hitboxFinished;
    private int hitboxFinishedCount;

    public float[] hitboxEnableTime;
    public float[] hitboxDisableTime;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    void FixedUpdate () {
        if (processing == true) {
            for (int i = 0; i < hitboxStarted.Length; i++) {
                if (hitboxStarted[i] == false && Time.time > startTimestamp + hitboxEnableTime[i]) {
                    hitboxStarted[i] = true;
                    EnableHitbox(i);
                } else if (hitboxFinished[i] == false && Time.time > startTimestamp + hitboxDisableTime[i]) {
                    hitboxFinished[i] = true;
                    DisableHitbox(i);
                    hitboxFinishedCount += 1;
                    if (hitboxFinishedCount >= hitboxStarted.Length) {
                        processing = false;
                    }
                }
            }
        }
    }

    public void StartHitboxProcess () {
        processing = true;
        startTimestamp = Time.time;
        hitboxStarted = new bool[hitboxEnableTime.Length];
        hitboxFinished = new bool[hitboxEnableTime.Length];
        hitboxFinishedCount = 0;
    }

    public void EnableHitbox (int index) {
        string hitboxName = "Hitbox " + index.ToString();
        foreach (Transform child in transform) {
            if (child.gameObject.name == hitboxName) {
                child.gameObject.SetActive(true);
            }
        }
    }

    public void DisableHitbox (int index) {
        string hitboxName = "Hitbox " + index.ToString();
        foreach (Transform child in transform) {
            if (child.gameObject.name == hitboxName) {
                child.gameObject.SetActive(false);
            }
        }
    }
}
