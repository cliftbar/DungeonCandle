using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameData {
    public int slot;

    // Player start location:
    public string scene;
    public float positionX;
    public float positionY;
    public float positionZ;
    public float velocityX;
    public float velocityY;
    public float velocityZ;
    public bool flipX;

    // Player progress variables:
    public bool[] beaconLit = new bool[100];
    public bool[] doorOpen = new bool[100];
    public bool[] bossDefeated = new bool[100];
    public bool[] plotSwitch = new bool[100];
}
