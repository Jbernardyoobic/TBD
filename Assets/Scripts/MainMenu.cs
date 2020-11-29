using System.Diagnostics.Tracing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Canvas mainMenu;
    public Canvas optionsMenu;
    public Canvas levelSelectionMenu;

    private PlayerData playerData;

    void Start() {
        playerData = SavingSystem.LoadRecords(playerData, 4);
        mainMenu.enabled = true;
        optionsMenu.enabled = false;
        levelSelectionMenu.enabled = false;
    }

    public void OnClickPlayGames() {
        PlayerPrefs.SetInt("LevelIndex", -1);
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void OnClickDisplayOptionMenu() {
        mainMenu.enabled = false;
        optionsMenu.enabled = true;
    }

    public void OnClickReturnToMainMenu() {
        optionsMenu.enabled = false;
        levelSelectionMenu.enabled = false;
        mainMenu.enabled = true;
    }

    public void OnClickQuitGames() {
        Application.Quit();
    }

    public void OnClickDisplayLevelSelection() {
        CheckIfLevelIsUnlocked();
        mainMenu.enabled = false;
        levelSelectionMenu.enabled = true;
    }

    public void OnClickSelectLevel(int levelIndex) {
        PlayerPrefs.SetInt("LevelIndex", levelIndex);
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void OnClickClearSave() {
        SavingSystem.CleanSave();
        playerData = SavingSystem.LoadRecords(playerData, 4);
    }

    private void CheckIfLevelIsUnlocked() {
        GameObject[] levelSelectionButtons = GameObject.FindGameObjectsWithTag("LevelSelection");
        for (int index = 0; index < levelSelectionButtons.Length; index++) {
            if (playerData.BestTimePerLevel[index] == 0) {
                levelSelectionButtons[index].GetComponent<Button>().interactable = false;
            }
        }
    }
}