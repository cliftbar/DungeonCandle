using UnityEngine;
using System.Collections;

public class BeaconTorch : MonoBehaviour {
    private GameObject scObject;
    private SceneController sc;
    private CameraController cc;

    private Animator anim;
    private GameObject lightObject;
    public string torchName;
    private bool lit;

    public float lightAnimationTime;
    public float zoomAnimationTime;

    void Awake () {
        anim = GetComponent<Animator>();
        lightObject = GetComponentInChildren<Light>().gameObject;
        cc = GameObject.Find("Main Camera").GetComponent<CameraController>();
    }

    // Use this for initialization
    void Start () {
        scObject = GameObject.Find("Scene Controller");
        sc = scObject.GetComponent<SceneController>();

        lit = sc.InitializeBeacon(torchName);

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

    void StartLightAnimation () {
        lit = true;

        sc.LightBeacon(torchName);
        sc.SaveCurrentGame();

        sc.paused = true;
        cc.FocusCamera(new Vector2(transform.position.x, transform.position.y + 1f), 4f, 0.15f, 0.15f);
        lightObject.SetActive(true);
        lightObject.GetComponent<Light>().intensity = 0f;
        anim.SetTrigger("light");

        StartCoroutine(LightFlame());
        StartCoroutine(ZoomBackOut());
    }

    IEnumerator LightFlame () {
        yield return new WaitForSeconds(lightAnimationTime);

        anim.SetBool("lit", true);
    }

    IEnumerator ZoomBackOut () {
        yield return new WaitForSeconds(zoomAnimationTime);

        sc.paused = false;
        cc.ReleaseCamera();
    }

    void PutOutFlame () {
        lit = false;
        anim.SetBool("lit", false);
        lightObject.SetActive(false);
    }

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.name == "Candle Flame Hitbox") {
            if (lit == false && other.transform.parent.GetComponent<PlayerController>().CandleLit() == true) {
                other.transform.parent.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                StartLightAnimation();
            } else if (lit == true && other.transform.parent.GetComponent<PlayerController>().CandleLit() == false) {
                other.transform.parent.GetComponent<PlayerController>().LightCandle();
            }
        }

        if (other.transform.parent != null) {
            if (other.transform.parent.gameObject.name == "Player Flame Attack" && lit == false) {
                StartLightAnimation();
            }
        }
    }
}
