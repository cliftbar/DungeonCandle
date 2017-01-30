using UnityEngine;
using System.Collections;

public class BeaconTorch : MonoBehaviour {
    private GameObject scObject;
    private SceneController sc;

    private Animator anim;
    private GameObject lightObject;
    public int id;
    private bool lit;

    void Awake () {
        anim = GetComponent<Animator>();
        lightObject = GetComponentInChildren<Light>().gameObject;
    }

    // Use this for initialization
    void Start () {
        scObject = GameObject.Find("Scene Controller");
        if (scObject == null) {
            lit = false;
        } else {
            sc = scObject.GetComponent<SceneController>();
            if (sc.BeaconLit(id) == true) {
                lit = true;
            } else {
                lit = false;
            }
        }

        if (lit == false) {
            PutOutFlame();
        }
    }

    void OnEnable () {
        Start();
    }

    
    // Update is called once per frame
    void Update () {
    }

    void LightFlame () {
        lit = true;
        anim.SetBool("lit", true);
        lightObject.SetActive(true);

        sc.LightBeacon(id);
        sc.SaveCurrentGame();
    }

    void PutOutFlame () {
        lit = false;
        anim.SetBool("lit", false);
        lightObject.SetActive(false);
    }

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.name == "Candle Flame Hitbox") {
            if (lit == false && other.transform.parent.GetComponent<PlayerController>().CandleLit() == true) {
                LightFlame();
            } else if (lit == true && other.transform.parent.GetComponent<PlayerController>().CandleLit() == false) {
                other.transform.parent.GetComponent<PlayerController>().LightCandle();
            }
        }

        if (other.transform.parent != null) {
            if (other.transform.parent.gameObject.name == "Player Flame Attack" && lit == false) {
                LightFlame();
            }
        }
    }
}
