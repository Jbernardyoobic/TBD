using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    
    public Button play;
    public Button quit;

    void Start() {
        play.onClick.AddListener(onPlayClick);
        quit.onClick.AddListener(onQuitClick);
    }

    void onPlayClick() {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    void onQuitClick() {
        Application.Quit();
    }
}