using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour {
    private SceneController sc;

    public Vector3 position;
    public Vector3 velocity;
    public bool usePlayerDirectionForVelocity;
    public bool saveToDisk;

    void Awake () {
    }

	// Use this for initialization
	void Start () {
        sc = GameObject.Find("Scene Controller").GetComponent<SceneController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.name == "Player") {
            Vector3 directionalVelocity = velocity;
            bool flipX = other.gameObject.GetComponent<SpriteRenderer>().flipX;
            if (usePlayerDirectionForVelocity == true && flipX == true) {
                directionalVelocity = new Vector3(directionalVelocity.x * -1f, directionalVelocity.y, directionalVelocity.z);
            }

            sc.SavePosition(SceneManager.GetActiveScene().name, position, directionalVelocity, flipX);

            if (saveToDisk == true && sc.CurrentSaveSlot() >= 0) {
                sc.SaveCurrentGame();
            }
        }
    }
}
