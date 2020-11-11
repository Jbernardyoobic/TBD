using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

    float[] timePerLevel;
    float[] timePerMirrorLevel;
    int[] totalCollectiblesPerLevel;
    bool[] secretCollectiblesPerLevel;

    public float[] TimePerLevel { get => timePerLevel; set => timePerLevel = value; }
    public float[] TimePerMirrorLevel { get => timePerMirrorLevel; set => timePerMirrorLevel = value; }
    public int[] TotalCollectiblesPerLevel { get => totalCollectiblesPerLevel; set => totalCollectiblesPerLevel = value; }
    public bool[] SecretCollectiblesPerLevel { get => secretCollectiblesPerLevel; set => secretCollectiblesPerLevel = value; }

    public void InitiateData(int totalLevel) {
        timePerLevel = new float[totalLevel];
        TimePerMirrorLevel = new float[totalLevel];
        totalCollectiblesPerLevel = new int[totalLevel];
        secretCollectiblesPerLevel = new bool[totalLevel];
    }
}
