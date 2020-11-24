﻿using System.ComponentModel;
using System.Collections;
using System;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    public GameObject playerPrefab;
    public LevelComponent[] levels;
    public TimerHandler stopWatch;
    public PlayerData playerData;
    public int currentLevel = -1;

    public Canvas ui_endGameScreen;
    public Canvas ui_endLevelScreen;
    public Canvas ui_timerScreen;
    public Canvas ui_deathScreen;
    public TextMeshProUGUI t_timePerLevelRecap;
    public TextMeshProUGUI t_endLevelResume;
    public TextMeshProUGUI t_endLevelTimeDiff;
    public TextMeshProUGUI t_endLevelButtonText;
    public TextMeshProUGUI t_totalTimeDiff;

    private Transform playerSpawnPoint;

    private bool playerSubmit;
    private float currentLevelTimeDiff;
    private float currentLevelTime;
    private float currentTotalTime;

    private float[] currentTimesPerLevel;


    private void Awake() {
        stopWatch = GameObject.FindObjectOfType<TimerHandler>();
        playerData = SavingSystem.LoadRecords(playerData, levels.Length);
        ui_endGameScreen.enabled = false;
        ui_endLevelScreen.enabled = false;
        ui_deathScreen.enabled = false;
        currentTimesPerLevel = new float[levels.Length];
    }

    private void Start() {
        GenerateLevel(0);
    }

    private void Update() {
        playerSubmit = Input.GetButtonDown("Submit");
        if (playerSubmit && (ui_endLevelScreen.enabled && !ui_endGameScreen.enabled)) {
            NextLevel();
        } else if (playerSubmit && (!ui_endLevelScreen.enabled && ui_endGameScreen.enabled)) {
            RestartGame();
        } else if (playerSubmit && ui_deathScreen.enabled) {
            ui_deathScreen.enabled = false;
            PreviousLevel();
        }
    }

    public void PlayerDeath(GameObject player, int mapIndex) {
        CharacterController2D controller = player.GetComponent<CharacterController2D>();
        controller.CreateDeathEffect();
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<SpriteRenderer>().enabled = false;
        ui_deathScreen.enabled = true;
    }

    public void PreviousLevel() {
        ShowTimerScreen();
        int levelIndex = currentLevel - 1;
        if (levelIndex < 0) {
            GenerateLevel(0);
        } else if (levelIndex == 0) {
            GenerateLevel(currentLevel);
        } else {
            GenerateLevel(levelIndex);
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
        if (levels[mapIndex].mirroredLevel) {
            Instantiate(levels[currentLevel].levelPrefab, gameObject.transform.position, new Quaternion(0f, -180f, 0f, 0f), transform);
        } else {
            Instantiate(levels[currentLevel].levelPrefab, gameObject.transform.position, Quaternion.identity, transform);
        }
        playerSpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity, transform);
        playerData.CurrentGatheredCollectibles = 0;
        playerData.GatheredSecretCollectibles = 0;
        stopWatch.ResetStopWatch();
    }

    public void RegisterTime(int level) {
        stopWatch.EndStopWatch();
        currentLevelTime = stopWatch.LevelTime;
        currentTimesPerLevel[currentLevel] = stopWatch.LevelTime;
        if (playerData.BestTimePerLevel[level] != 0) {
            currentLevelTimeDiff = playerData.BestTimePerLevel[level] - currentLevelTime;
            playerData.BestTimePerLevel[level] = currentLevelTime < playerData.BestTimePerLevel[level] ? currentLevelTime : playerData.BestTimePerLevel[level];
        } else {
            currentLevelTimeDiff = 0;
            playerData.BestTimePerLevel[level] = currentLevelTime;
        }
    }


    public void TimeDiffDisplay(TextMeshProUGUI t_timeDiff, float currentDiff) {
        t_timeDiff.text = "";

        if (currentDiff != 0) {
            t_timeDiff.color = currentDiff > 0 ? Color.green : Color.red;
            t_timeDiff.text += currentDiff > 0 ? "-" : "+";
            t_timeDiff.text += String.Format("{0:F3}s", Mathf.Abs(currentDiff));
        }
    }

    public void EndGame() {
        ClearLevel();
        ShowEndGameScreen();

        currentTotalTime = currentTimesPerLevel.Sum();
        t_timePerLevelRecap.text = "Total time: " + currentTotalTime.ToString("F2") + "s";

        if (playerData.BestTotalTime > 0) {
            TimeDiffDisplay(t_totalTimeDiff, playerData.BestTotalTime - currentTotalTime);
        } else {
            t_totalTimeDiff.text = "";
        }

        playerData.BestTotalTime = playerData.BestTotalTime > currentTotalTime || playerData.BestTotalTime == -1 ? currentTotalTime : playerData.BestTotalTime;
    }

    public void EndLevel(int mapIndex) {
        ClearLevel();
        ShowEndLevelScreen();
        TimeDiffDisplay(t_endLevelTimeDiff, currentLevelTimeDiff);
        t_endLevelResume.text = String.Format("Time : {0:F2}s\n\n     : {1}/{2}\n\n     : {3}/1",
                                            currentLevelTime,
                                            playerData.TotalCollectiblesPerLevel[mapIndex],
                                            levels[mapIndex].totalCollectibles,
                                            playerData.SecretCollectiblesPerLevel[mapIndex]);

        if (mapIndex + 1 > levels.Length - 1) {
            t_endLevelButtonText.text = "End Game";
        } else {
            t_endLevelButtonText.text = "NEXT";
        }
        SavingSystem.SaveRecords(playerData);
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
