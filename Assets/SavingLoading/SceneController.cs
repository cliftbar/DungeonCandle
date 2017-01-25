using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {
    private GameData data;

    void Awake () {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    // ------------------------------- //
    // CREATING AND LOADING SAVE FILES //
    // ------------------------------- //

    public void NewGame () {

    }

    public void LoadGame () {

    }

    public void SaveGame () {

    }

    // -------------- //
    // LOADING SCENES //
    // -------------- //

    public IEnumerator LoadScene () {
        yield return new WaitForSeconds(0.01f);
    }

    // -------------------------- //
    // INITIALIZING SCENE OBJECTS //
    // -------------------------- //

    private void InitializeUI () {

    }

    private void InitializePlayer () {

    }

    private void InitializeCamera () {

    }
}
