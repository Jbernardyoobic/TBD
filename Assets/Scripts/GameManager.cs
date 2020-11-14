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

    private bool playerSubmit = false;
    private float levelTimeDiff;

    private void Awake() {
        stopWatch = GameObject.FindObjectOfType<TimerHandler>();
        playerData.InitiateData(levels.Length);
        ui_endGameScreen.enabled = false;
        ui_endLevelScreen.enabled = false;
    }

    private void Update() {
        playerSubmit = Input.GetButton("Submit");
        Debug.Log(playerSubmit);
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

        if (playerData.TimePerLevel[level] != 0) {
            levelTimeDiff = playerData.TimePerLevel[level] - stopWatch.LevelTime;
            playerData.TimePerLevel[level] = stopWatch.LevelTime < playerData.TimePerLevel[level] ? stopWatch.LevelTime : playerData.TimePerLevel[level];
        } else {
            levelTimeDiff = 0;
            playerData.TimePerLevel[level] = stopWatch.LevelTime;
        }
    }

    public void EndGame() {
        ClearLevel();
        ShowEndGameScreen();
        t_timePerLevelRecap.text = "";

        for (int i = 0; i < playerData.TimePerLevel.Length; i++) {
            t_timePerLevelRecap.text += "Level " + (i + 1) + " : " + playerData.TimePerLevel[i].ToString("F2") + "s\n\n";
        }

        t_timePerLevelRecap.text += "Total time: " + playerData.TimePerLevel.Sum().ToString("F2") + "s";
    }

    public void EndLevel(int mapIndex) {
        ClearLevel();
        ShowEndLevelScreen();
        t_endLevelTimeDiff.text = "";

        if (levelTimeDiff != 0) {
            t_endLevelTimeDiff.color = levelTimeDiff > 0 ? Color.green : Color.red;
            t_endLevelTimeDiff.text += levelTimeDiff > 0 ? "-" : "+";
            t_endLevelTimeDiff.text += String.Format("{0:F3}s", Mathf.Abs(levelTimeDiff));
        }
        t_endLevelResume.text = String.Format("Time : {0:F2}s\n\nCollectibles : {1}/{2}\n\nSecret Collectibles : {3}/1",
                                            playerData.TimePerLevel[mapIndex],
                                            playerData.TotalCollectiblesPerLevel[mapIndex],
                                            levels[mapIndex].totalCollectibles,
                                            playerData.SecretCollectiblesPerLevel[mapIndex]);
        //     yield return StartCoroutine(WaitForKeyDown(playerSubmit));
        //     Debug.Log("Done");
        //     NextLevel();
        //     yield return null;
    }

    // IEnumerator WaitForKeyDown(bool done) {
    //     while (!done)
    //         yield return null;
    // }

    public void RestartGame() {
        ShowTimerScreen();
        stopWatch.ResetStopWatch();
        GenerateLevel(0);
    }

    public void NextLevel() {
        ShowTimerScreen();
        stopWatch.ResetStopWatch();
        GenerateLevel(currentLevel + 1);
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
