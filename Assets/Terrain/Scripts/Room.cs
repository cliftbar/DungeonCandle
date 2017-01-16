using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {
    private GameObject terrainCubes;
    private GameObject wall;
    private GameObject contents;
    private Light ambientLight;

    private bool currentRoom = false;
    private float enterTimestamp;

    // Variables for setting room-dependent lighting:
    private float lightShiftTime = 4f;
    private float startingIntensity;
    public float lightIntensity;
    private Color startingColor;
    public Color lightColor;
    public bool playerLit;

    // Z-coordinates for which terrain is actually solid:
    public float[] solidZ;

    void Awake () {
        foreach (Transform child in transform) {
            if (child.gameObject.name == "Wall") {
                wall = child.gameObject;
            } else if (child.gameObject.name == "Terrain Cubes") {
                terrainCubes = child.gameObject;
            } else if (child.gameObject.name == "Contents") {
                contents = child.gameObject;
            }
        }
        ambientLight = GameObject.Find("Ambient Light").GetComponent<Light>();
    }

	// Use this for initialization
	void Start () {
	   SetSolidTerrain();
	}
	
	// Update is called once per frame
	void Update () {
        if (currentRoom == true) {
            ambientLight.intensity = Mathf.Lerp(startingIntensity, lightIntensity, Time.time / lightShiftTime - enterTimestamp / lightShiftTime);
            ambientLight.color = Color.Lerp(startingColor, lightColor, Time.time / lightShiftTime - enterTimestamp / lightShiftTime);
        }
	}

    void SetSolidTerrain () {
        foreach (Transform child in terrainCubes.transform) {
            foreach (float z in solidZ) {
                if (Mathf.Abs(child.position.z - z) <= 0.3f) {
                    child.GetComponent<BoxCollider>().enabled = true;
                }
            }
        }
    }

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.name == "Player") {
            currentRoom = true;
            startingIntensity = ambientLight.intensity;
            startingColor = ambientLight.color;

            EnableWall(false);
            EnableContents(true);
            other.gameObject.GetComponent<PlayerController>().SetRoomLighting(playerLit);
        }
    }

    void OnTriggerExit (Collider other) {
        if (other.gameObject.name == "Player") {
            enterTimestamp = Time.time;
            currentRoom = false;
            EnableWall(true);
            EnableContents(false);           
        }
    }

    void EnableWall (bool enabled) {
        wall.SetActive(enabled);
    }

    void EnableContents (bool enabled) {
        contents.SetActive(enabled);
    }
}
