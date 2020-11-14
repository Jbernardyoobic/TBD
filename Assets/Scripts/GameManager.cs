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

    public Canvas endGameScreen;
    public Canvas endLevelScreen;
    public Canvas timerScreen;
    public TextMeshProUGUI timePerLevelRecap;
    public TextMeshProUGUI endLevelResume;

    private bool playerSubmit = false;

    private void Awake() {
        stopWatch = GameObject.FindObjectOfType<TimerHandler>();
        playerData.InitiateData(levels.Length);
        endGameScreen.enabled = false;
        endLevelScreen.enabled = false;
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
        playerData.TimePerLevel[level] = stopWatch.LevelTime;
    }

    public void EndGame() {
        ClearLevel();
        ShowEndGameScreen();
        timePerLevelRecap.text = "";

        for (int i = 0; i < playerData.TimePerLevel.Length; i++) {
            timePerLevelRecap.text += "Level " + (i + 1) + " : " + playerData.TimePerLevel[i].ToString("F2") + "s\n\n";
        }

        timePerLevelRecap.text += "Total time: " + playerData.TimePerLevel.Sum().ToString("F2") + "s";
    }

    public void EndLevel(int mapIndex) {
        ClearLevel();
        ShowEndLevelScreen();
        endLevelResume.text = String.Format("Time : {0:F2}s\n\nCollectibles : {1}/{2}\n\nSecret Collectibles : {3}/1",
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
        endLevelScreen.enabled = true;
        timerScreen.enabled = false;
    }

    private void ShowTimerScreen() {
        endGameScreen.enabled = false;
        endLevelScreen.enabled = false;
        timerScreen.enabled = true;
    }

    private void ShowEndGameScreen() {
        endGameScreen.enabled = true;
        timerScreen.enabled = false;
    }
}
