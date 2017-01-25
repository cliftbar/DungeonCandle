using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameData {
    // Save file name
    private string fileName;

    // Player start location:
    private string scene;
    private Vector3 position;
    private Vector3 velocity;
    private bool flipX;

    // Player progress variables:
    private bool[] beaconLit = new bool[100];
    private bool[] doorOpen = new bool[100];
    private bool[] bossDefeated = new bool[100];
    private bool[] plotSwitch = new bool[100];

    // ----------------------------- //
    // SAVE AND LOAD PLAYER POSITION //
    // ----------------------------- //

    public void SaveStartLocation (string newScene, Vector3 newPosition, Vector3 newVelocity, bool newFlipX) {
        scene = newScene;
        position = newPosition;
        velocity = newVelocity;
        flipX = newFlipX;
    }

    // ---------------------------------- //
    // SAVING AND LOADING PLAYER PROGRESS //
    // ---------------------------------- //
    public void LightBeacon (int i) {
        beaconLit[i] = true;
    }

    public bool BeaconLit (int i) {
        return beaconLit[i];
    }

    public void OpenDoor (int i) {
        doorOpen[i] = true;
    }

    public bool DoorOpen (int i) {
        return doorOpen[i];
    }

    public void DefeatBoss (int i) {
        bossDefeated[i] = true;
    }

    public bool BossDefeated (int i) {
        return bossDefeated[i];
    }

    public void SetPlotSwitch (int i, bool switchValue) {
        plotSwitch[i] = switchValue;
    }

    public bool PlotSwitch (int i) {
        return plotSwitch[i];
    }
}
