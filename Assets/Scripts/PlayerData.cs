using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

    float[] bestTimePerLevel;
    float[] bestTimePerMirrorLevel;
    int[] totalCollectiblesPerLevel;
    int[] secretCollectiblesPerLevel;

    int currentGatheredCollectibles = 0;
    int gatheredSecretCollectibles = 0;

    public float[] BestTimePerLevel { get => bestTimePerLevel; set => bestTimePerLevel = value; }
    public float[] BestTimePerMirrorLevel { get => bestTimePerMirrorLevel; set => bestTimePerMirrorLevel = value; }
    public int[] TotalCollectiblesPerLevel { get => totalCollectiblesPerLevel; set => totalCollectiblesPerLevel = value; }
    public int[] SecretCollectiblesPerLevel { get => secretCollectiblesPerLevel; set => secretCollectiblesPerLevel = value; }
    public int CurrentGatheredCollectibles { get => currentGatheredCollectibles; set => currentGatheredCollectibles = value; }
    public int GatheredSecretCollectibles { get => gatheredSecretCollectibles; set => gatheredSecretCollectibles = value; }

    public void InitiateData(int totalLevel) {
        bestTimePerLevel = new float[totalLevel];
        BestTimePerMirrorLevel = new float[totalLevel];
        totalCollectiblesPerLevel = new int[totalLevel];
        secretCollectiblesPerLevel = new int[totalLevel];
    }
}
