using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public IDictionary<string, bool> beaconLit = new Dictionary<string, bool>();
    public IDictionary<string, bool> doorOpen = new Dictionary<string, bool>();
    public IDictionary<string, bool> bossDefeated = new Dictionary<string, bool>();
    public IDictionary<string, bool> plotSwitch = new Dictionary<string, bool>();
}
