using UnityEngine;

//Class that store all the component for a single level
[System.Serializable]
public class LevelComponent {
    public GameObject levelPrefab;
    public int totalCollectibles;
    public int secretCollectibles;
    public float developerTime;
    public bool mirroredLevel;
}
