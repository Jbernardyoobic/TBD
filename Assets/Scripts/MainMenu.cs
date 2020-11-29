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

    void Start() {
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
        mainMenu.enabled = false;
        levelSelectionMenu.enabled = true;
    }

    public void OnClickSelectLevel(int levelIndex) {
        PlayerPrefs.SetInt("LevelIndex", levelIndex);
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}