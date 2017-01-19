using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {
    private GameObject terrainCubes;
    private GameObject wall;
    private GameObject contents;
    private GameObject persistentContents;
    private Light ambientLight;
    private CameraController cameraController;
    private PlayerController pc;

    private bool occupied = false;
    private bool onlyRoomOccupied = false;
    private float enterTimestamp = -1f;

    // Variables for setting room-dependent lighting:
    private float lightShiftTime = 4f;
    private float startingIntensity;
    public float lightIntensity;
    private Color startingColor;
    public Color lightColor;
    public bool playerLit;

    // Variables for setting room-dependent camera attributes:
    public float cameraMinX;
    public float cameraMaxX;
    public float cameraMinY;
    public float cameraMaxY;

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
            } else if (child.gameObject.name == "Persistent Contents") {
                persistentContents = child.gameObject;
            }
        }
        ambientLight = GameObject.Find("Ambient Light").GetComponent<Light>();
        cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Use this for initialization
    void Start () {
       SetSolidTerrain();
       EnableWall(true);
       EnableContents(false);
       EnablePersistentContents(false);
    }
    
    // Update is called once per frame
    void Update () {
        if (occupied == true && onlyRoomOccupied == false && pc.roomCount == 1) {
            FinishEntering();
        }
        if (onlyRoomOccupied == true) {
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
            StartEntering();
        }
    }

    void StartEntering () {
        occupied = true;
        pc.roomCount += 1;

        if (enterTimestamp < 0f) {
            EnablePersistentContents(true);
        }
        EnableWall(false);
        EnableContents(true);
    }

    void FinishEntering () {
        onlyRoomOccupied = true;
        pc.SetRoomLighting(playerLit);
        startingIntensity = ambientLight.intensity;
        startingColor = ambientLight.color;
        enterTimestamp = Time.time;
        cameraController.cameraMin = new Vector2(cameraMinX, cameraMinY);
        cameraController.cameraMax = new Vector2(cameraMaxX, cameraMaxY);
    }

    void OnTriggerExit (Collider other) {
        if (other.gameObject.name == "Player") {
            occupied = false;
            onlyRoomOccupied = false;

            pc.roomCount -= 1;

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

    void EnablePersistentContents (bool enabled) {
        persistentContents.SetActive(enabled);
    }
}
