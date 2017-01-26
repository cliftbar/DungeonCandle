using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TestSceneController : MonoBehaviour {
    private SceneController sceneController;
    public GameObject sceneControllerPrefab;

    public Vector3 testStartPosition;

    void Awake () {
        if (GameObject.Find("Scene Controller") == null) {
            sceneController = Instantiate(sceneControllerPrefab).GetComponent<SceneController>();
            sceneController.gameObject.name = "Scene Controller";
            sceneController.StartTestGame(testStartPosition, SceneManager.GetActiveScene().name);
        }

        Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {

    }
}
