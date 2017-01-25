using UnityEngine;
using System.Collections;

public class CandleSnuffer : MonoBehaviour {
    private PlayerController pc;

    void Awake () {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.name == "Candle Hitbox") {
            pc.PutCandleOut();
        }
    }
}
