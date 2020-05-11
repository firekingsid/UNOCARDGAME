using System;
using UnityEngine;

[System.Serializable]
public class GameConfig : MonoBehaviour {
    [Header ("")]
    public string androidPackageID;
    public string iosAppID;

    public static GameConfig instance;
    private void Awake () {
        instance = this;
    }
}