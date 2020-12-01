using System;
using System.Diagnostics.Tracing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class MainMenu : MonoBehaviour {

    public Canvas mainMenu;
    public Canvas optionsMenu;
    public Canvas levelSelectionMenu;
    public Canvas statsMenu;

    public AudioMixer mixer;
    public Slider volumeSlider;

    public TextMeshProUGUI textStats;

    private PlayerData playerData;

    void Start() {
        playerData = SavingSystem.LoadRecords(playerData, 4);
        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        mainMenu.enabled = true;
        optionsMenu.enabled = false;
        levelSelectionMenu.enabled = false;
        statsMenu.enabled = false;
    }

    public void OnClickPlayGames() {
        PlayerPrefs.SetInt("LevelIndex", -1);
        PlayerPrefs.SetString("selectedMode", "classic");
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void OnClickDisplayOptionMenu() {
        mainMenu.enabled = false;
        optionsMenu.enabled = true;
    }

    public void OnClickReturnToMainMenu() {
        optionsMenu.enabled = false;
        levelSelectionMenu.enabled = false;
        statsMenu.enabled = false;
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
        PlayerPrefs.SetString("selectedMode", "loop");
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void OnClickClearSave() {
        SavingSystem.CleanSave();
        playerData = SavingSystem.LoadRecords(playerData, 4);
    }

    public void OnClickDisplayStats() {
        statsMenu.enabled = true;
        mainMenu.enabled = false;
        textStats.text = "";
        textStats.richText = true;
        for (int index = 0; index < playerData.BestTimePerLevel.Length; index++) {
            String textLevel = index == 0 ? "Tutorial" : "Level " + index;
            textStats.text += String.Format("{0} : {1:F3}s\n        <sprite index=1> {2}\n        <sprite index=0> {3}\n\n",
                                        textLevel,
                                        playerData.BestTimePerLevel[index],
                                        playerData.TotalCollectiblesPerLevel[index],
                                        playerData.SecretCollectiblesPerLevel[index]);
        }
    }

    public void SetLevel(float sliderValue) {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    private void CheckIfLevelIsUnlocked() {
        GameObject[] levelSelectionButtons = GameObject.FindGameObjectsWithTag("LevelSelection");
        for (int index = 0; index < levelSelectionButtons.Length; index++) {
            if (playerData.BestTimePerLevel[index + 1] == 0) {
                levelSelectionButtons[index].GetComponent<Button>().interactable = false;
            }
        }
    }
}