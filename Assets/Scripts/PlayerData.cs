using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

    float[] timePerLevel;
    float[] timePerMirrorLevel;
    int[] totalCollectiblesPerLevel;
    int[] secretCollectiblesPerLevel;

    int currentGatheredCollectibles = 0;
    int gatheredSecretCollectibles = 0;

    public float[] TimePerLevel { get => timePerLevel; set => timePerLevel = value; }
    public float[] TimePerMirrorLevel { get => timePerMirrorLevel; set => timePerMirrorLevel = value; }
    public int[] TotalCollectiblesPerLevel { get => totalCollectiblesPerLevel; set => totalCollectiblesPerLevel = value; }
    public int[] SecretCollectiblesPerLevel { get => secretCollectiblesPerLevel; set => secretCollectiblesPerLevel = value; }
    public int CurrentGatheredCollectibles { get => currentGatheredCollectibles; set => currentGatheredCollectibles = value; }
    public int GatheredSecretCollectibles { get => gatheredSecretCollectibles; set => gatheredSecretCollectibles = value; }

    public void InitiateData(int totalLevel) {
        timePerLevel = new float[totalLevel];
        TimePerMirrorLevel = new float[totalLevel];
        totalCollectiblesPerLevel = new int[totalLevel];
        secretCollectiblesPerLevel = new int[totalLevel];
    }
}
