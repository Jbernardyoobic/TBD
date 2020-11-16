using System.Collections;
using System;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    public LevelComponent[] levels;
    public TimerHandler stopWatch;
    public PlayerData playerData;
    public int currentLevel = -1;

    public Canvas ui_endGameScreen;
    public Canvas ui_endLevelScreen;
    public Canvas ui_timerScreen;
    public TextMeshProUGUI t_timePerLevelRecap;
    public TextMeshProUGUI t_endLevelResume;
    public TextMeshProUGUI t_endLevelTimeDiff;
    public TextMeshProUGUI t_endLevelButtonText;

    private bool playerSubmit;
    private float currentLevelTimeDiff;
    private float currentLevelTime;

    private void Awake() {
        stopWatch = GameObject.FindObjectOfType<TimerHandler>();
        playerData.InitiateData(levels.Length);
        ui_endGameScreen.enabled = false;
        ui_endLevelScreen.enabled = false;
    }

    private void Start() {
        GenerateLevel(0);
    }

    private void Update() {
        playerSubmit = Input.GetButtonDown("Submit");
        if (playerSubmit && (ui_endLevelScreen.enabled && !ui_endGameScreen.enabled)) {
            NextLevel();
            playerSubmit = false;
        } else if (playerSubmit && (!ui_endLevelScreen.enabled && ui_endGameScreen.enabled)) {
            RestartGame();
            playerSubmit = false;
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
        currentLevelTime = stopWatch.LevelTime;
        if (playerData.BestTimePerLevel[level] != 0) {
            currentLevelTimeDiff = playerData.BestTimePerLevel[level] - currentLevelTime;
            playerData.BestTimePerLevel[level] = currentLevelTime < playerData.BestTimePerLevel[level] ? currentLevelTime : playerData.BestTimePerLevel[level];
        } else {
            currentLevelTimeDiff = 0;
            playerData.BestTimePerLevel[level] = currentLevelTime;
        }
    }

    public void EndGame() {
        ClearLevel();
        ShowEndGameScreen();
        t_timePerLevelRecap.text = "";

        for (int i = 0; i < playerData.BestTimePerLevel.Length; i++) {
            t_timePerLevelRecap.text += "Level " + (i + 1) + " : " + playerData.BestTimePerLevel[i].ToString("F2") + "s\n\n";
        }

        t_timePerLevelRecap.text += "Total time: " + playerData.BestTimePerLevel.Sum().ToString("F2") + "s";
    }

    public void EndLevel(int mapIndex) {
        ClearLevel();
        ShowEndLevelScreen();
        t_endLevelTimeDiff.text = "";

        if (currentLevelTimeDiff != 0) {
            t_endLevelTimeDiff.color = currentLevelTimeDiff > 0 ? Color.green : Color.red;
            t_endLevelTimeDiff.text += currentLevelTimeDiff > 0 ? "-" : "+";
            t_endLevelTimeDiff.text += String.Format("{0:F3}s", Mathf.Abs(currentLevelTimeDiff));
        }
        t_endLevelResume.text = String.Format("Time : {0:F2}s\n\nCollectibles : {1}/{2}\n\nSecret Collectibles : {3}/1",
                                            currentLevelTime,
                                            playerData.TotalCollectiblesPerLevel[mapIndex],
                                            levels[mapIndex].totalCollectibles,
                                            playerData.SecretCollectiblesPerLevel[mapIndex]);

        if (mapIndex + 1 > levels.Length - 1) {
            t_endLevelButtonText.text = "End Game";
        } else {
            t_endLevelButtonText.text = "NEXT";
        }
    }

    public void RestartGame() {
        ShowTimerScreen();
        stopWatch.ResetStopWatch();
        GenerateLevel(0);
    }

    public void NextLevel() {
        ShowTimerScreen();
        if (currentLevel + 1 > levels.Length - 1) {
            EndGame();
        } else {
            GenerateLevel(currentLevel + 1);
        }
    }

    public void RegisterCollectibles(int mapIndex) {
        playerData.TotalCollectiblesPerLevel[mapIndex] = playerData.CurrentGatheredCollectibles;
        playerData.SecretCollectiblesPerLevel[mapIndex] = playerData.GatheredSecretCollectibles;
        playerData.CurrentGatheredCollectibles = 0;
        playerData.GatheredSecretCollectibles = 0;
    }

    private void ShowEndLevelScreen() {
        ui_endLevelScreen.enabled = true;
        ui_timerScreen.enabled = false;
    }

    private void ShowTimerScreen() {
        ui_endGameScreen.enabled = false;
        ui_endLevelScreen.enabled = false;
        ui_timerScreen.enabled = true;
    }

    private void ShowEndGameScreen() {
        ui_endGameScreen.enabled = true;
        ui_timerScreen.enabled = false;
    }
}
