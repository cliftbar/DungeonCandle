using UnityEngine;
using System.Collections;

public class Message : MonoBehaviour {
    private UIController ui;
    private CameraController cc;
    private GameObject button;
    private int triggered;
    // triggered = 0 : not triggered
    //           = 1 : triggered and currently displaying text
    //           = 2 : triggered and not displaying text

    public string[] message;

    void Awake () {
        triggered = 0;
        ui = GameObject.Find("UI Canvas").GetComponent<UIController>();
        cc = GameObject.Find("Main Camera").GetComponent<CameraController>();

        gameObject.AddComponent<InteractButton>();

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
        if (triggered == 2 && Input.GetButtonDown("Interact")) {
            StartMessage();
        }
    }

    void StartMessage () {
        triggered = 1;
        button.SetActive(false);
        ui.DisplayText(this, message);
        cc.FocusCamera(new Vector2(cc.transform.position.x, cc.transform.position.y - 1f), -1f * cc.transform.position.z, 0.02f);
    }

    public IEnumerator FinishMessage () {
        cc.ReleaseCamera();

        yield return new WaitForSeconds(0.5f);

        if (triggered == 1) {
            triggered = 2;
            button.SetActive(true);
        }
    }

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.name == "Player") {
            triggered = 2;
        }
    }

    void OnTriggerExit (Collider other) {
        if (other.gameObject.name == "Player") {
            triggered = 0;
        }
    }
}
