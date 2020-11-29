using System.Diagnostics.Tracing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Canvas mainMenu;
    public Canvas optionsMenu;

    void Start() {
        mainMenu.enabled = true;
        optionsMenu.enabled = false;
    }

    public void onPlayClick() {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void onOptionsClick() {
        mainMenu.enabled = false;
        optionsMenu.enabled = true;
    }

    public void onQuitOptionsClick() {
        optionsMenu.enabled = false;
        mainMenu.enabled = true;
    }

    public void onQuitClick() {
        Application.Quit();
    }
}