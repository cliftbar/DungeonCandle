using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SceneController : MonoBehaviour {
    private GameData[] savedData;
    private GameData currentData;
    public int saveCount;

    public string startScene;
    public Vector3 startPosition;
    public Vector3 startVelocity;
    public bool startFlipX;

    public string testLoadScene;
    public Vector3 testLoadStartPosition;

    // Pausing variables:
    public bool paused;

    void Awake () {
        DontDestroyOnLoad(transform.gameObject);
        savedData = new GameData[saveCount];
        PreloadSavedGames();
    }

    // Use this for initialization
    void Start () {
        if (testLoadScene != "") {
            StartCoroutine(LoadScene(testLoadScene, testLoadStartPosition, new Vector3(0f, 0f, 0f), false));
        }
    }

    void Update () {

    }

    // ------------------------------- //
    // CREATING AND LOADING SAVE FILES //
    // ------------------------------- //

    private void PreloadSavedGames () {
        FileStream file;
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/data.save")) {
            file = File.Open(Application.persistentDataPath + "/data.save", FileMode.Open);
            savedData = (GameData[])bf.Deserialize(file);
            file.Close();
        } else {
            file = File.Create(Application.persistentDataPath + "/data.save");
            bf.Serialize(file, savedData);
            file.Close();
        }
    }

    public void NewGame (int slot) {
        currentData = new GameData();
        currentData.slot = slot;
        currentData.scene = startScene;
        currentData.positionX = startPosition.x;
        currentData.positionY = startPosition.y;
        currentData.positionZ = startPosition.z;
        currentData.velocityX = startVelocity.x;
        currentData.velocityY = startVelocity.y;
        currentData.velocityZ = startVelocity.z;
        currentData.flipX = startFlipX;

        SaveGame(slot);
        LoadGame(slot);
    }

    public void LoadGame (int slot) {
        currentData = savedData[slot];
        Vector3 loadPosition = new Vector3(currentData.positionX, currentData.positionY, currentData.positionZ);
        Vector3 loadVelocity = new Vector3(currentData.velocityX, currentData.velocityY, currentData.velocityZ);
        StartCoroutine(LoadScene(currentData.scene, loadPosition, loadVelocity, currentData.flipX));
    }

    public void SaveCurrentGame () {
        if (currentData.slot >= 0) {
            SaveGame(currentData.slot);
        }
        Debug.Log("Saved to " + Application.persistentDataPath + " in slot " + currentData.slot);
    }

    public void SaveGame (int slot) {
        savedData[slot] = currentData;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        file = File.Open(Application.persistentDataPath + "/data.save", FileMode.Open);
        bf.Serialize(file, savedData);
        file.Close();
    }

    public bool SaveSlotFilled (int slot) {
        if (savedData[slot] != null) {
            return true;
        } else {
            return false;
        }
    }

    // ------- //
    // TESTING //
    // ------- //

    public void StartTestGame (Vector3 testStartPosition, string scene) {
        currentData = new GameData();

        InitializePlayer(testStartPosition, new Vector3(0f, 0f, 0f), false);
        InitializeCamera();

        currentData.slot = -1;
        currentData.scene = scene;
        currentData.positionX = testStartPosition.x;
        currentData.positionY = testStartPosition.y;
        currentData.positionZ = testStartPosition.z;
        currentData.velocityX = 0f;
        currentData.velocityY = 0f;
        currentData.velocityZ = 0f;
        currentData.flipX = false;
    }

    public int CurrentSaveSlot () {
        return currentData.slot;
    }

    // -------------- //
    // LOADING SCENES //
    // -------------- //

    public void Respawn () {
        Vector3 loadPosition = new Vector3(currentData.positionX, currentData.positionY, currentData.positionZ);
        Vector3 loadVelocity = new Vector3(currentData.velocityX, currentData.velocityY, currentData.velocityZ);
        StartCoroutine(LoadScene(currentData.scene, loadPosition, loadVelocity, currentData.flipX));
    }

    public IEnumerator LoadScene (string newScene, Vector3 startingPosition, Vector3 startingVelocity, bool flipX) {
        SceneManager.LoadScene(newScene, LoadSceneMode.Single);

        yield return new WaitForSeconds(0.01f);

        InitializePlayer(startingPosition, startingVelocity, flipX);
        InitializeCamera();
    }

    // -------------------------- //
    // INITIALIZING SCENE OBJECTS //
    // -------------------------- //

    public void InitializePlayer (Vector3 startingPosition, Vector3 startingVelocity, bool flipX) {
        GameObject player = GameObject.Find("Player");

        player.transform.position = startingPosition;
        player.GetComponent<Rigidbody>().velocity = startingVelocity;
        if (player.GetComponent<SpriteRenderer>().flipX != flipX) {
            player.GetComponent<PlayerController>().TurnAround();
        }
    }

    public void InitializeCamera () {
        GameObject.Find("Main Camera").GetComponent<CameraController>().Initialize();
    }

    // ------------------------- //
    // SAVING PROGRESS TO MEMORY //
    // ------------------------- //

    public void SavePosition (string scene, Vector3 position, Vector3 velocity, bool flipX) {
        currentData.scene = scene;
        currentData.positionX = position.x;
        currentData.positionY = position.y;
        currentData.positionZ = position.z;
        currentData.velocityX = velocity.x;
        currentData.velocityY = velocity.y;
        currentData.velocityZ = velocity.z;
        currentData.flipX = flipX;
    }

    public bool InitializeBeacon (string beaconName) {
        bool beaconValue;
        if (currentData.beaconLit.TryGetValue(beaconName, out beaconValue)) {
            return beaconValue;
        } else {
            currentData.beaconLit.Add(beaconName, false);
            return false;
        }
    }

    public void LightBeacon(string beaconName) {
        if (currentData.beaconLit[beaconName] == true) {
            throw new System.ArgumentException("Beacon for that beaconId was already lit. There may be more than 1 beacon assigned to the same ID.", "beaconId");
        } else {
            currentData.beaconLit[beaconName] = true;
        }
    }

    // ----------------- //
    // CHECKING PROGRESS //
    // ----------------- //

    public bool BeaconLit(string beaconName) {
        return currentData.beaconLit[beaconName];
    }
}
