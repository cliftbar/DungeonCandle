using UnityEngine;
using System.Collections;

public class InteractButton : MonoBehaviour {
    private GameObject button;

    void Awake () {
        foreach (Transform child in transform) {
            if (child.gameObject.name == "Button") {
                button = child.gameObject;
            }
        }
    }

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.name == "Player") {
            button.SetActive(true);
        }
    }

    void OnTriggerExit (Collider other) {
        if (other.gameObject.name == "Player") {
            button.SetActive(false);
        }
    }
}
