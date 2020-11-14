using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    public LevelComponent[] levels;
    public TimerHandler stopWatch;
    public PlayerData playerData;
    public int currentLevel = -1;

    public Canvas endScreen;
    public TextMeshProUGUI timePerLevelRecap;
    public Canvas timerScreen;

    private void Awake() {
        stopWatch = GameObject.FindObjectOfType<TimerHandler>();
        playerData.InitiateData(levels.Length);
        endScreen.enabled = false;
    }

    private void Update() {
        if (currentLevel == -1) {
            GenerateLevel(0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            GenerateLevel(0);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            GenerateLevel(1);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3)) {
            GenerateLevel(2);
        }
    }

    void ClearLevel() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    public void GenerateLevel(int mapIndex) {
        ClearLevel();
        currentLevel = mapIndex;
        Instantiate(levels[currentLevel].levelPrefab, gameObject.transform.position, Quaternion.identity, transform);
        playerData.CurrentGatheredCollectibles = 0;
        playerData.GatheredSecretCollectibles = 0;
        stopWatch.ResetStopWatch();
    }

    public void RegisterTime(int level) {
        stopWatch.EndStopWatch();
        playerData.TimePerLevel[level] = stopWatch.LevelTime;
    }

    public void EndGame() {
        ClearLevel();
        endScreen.enabled = true;
        timerScreen.enabled = false;
        timePerLevelRecap.text = "";

        for (int i = 0; i < playerData.TimePerLevel.Length; i++) {
            timePerLevelRecap.text += "Level " + (i + 1) + " : " + playerData.TimePerLevel[i].ToString("F2") + "s\n\n";
        }

        timePerLevelRecap.text += "Total time: " + playerData.TimePerLevel.Sum().ToString("F2") + "s";
    }

    public void RestartGame() {
        endScreen.enabled = false;
        timerScreen.enabled = true;
        stopWatch.ResetStopWatch();
        GenerateLevel(0);
    }

    public void RegisterCollectibles(int mapIndex) {
        levels[mapIndex].totalCollectibles = playerData.CurrentGatheredCollectibles;
        levels[mapIndex].secretCollectibles = playerData.GatheredSecretCollectibles;
        playerData.CurrentGatheredCollectibles = 0;
        playerData.GatheredSecretCollectibles = 0;
    }
}
