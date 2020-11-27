using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    
    public Button play;
    public Button quit;
    public Button quitOptions;
    public Button options;

    private GameObject optionsObject;
    private GameObject[] menuObjects;

    void Start() {
        play.onClick.AddListener(onPlayClick);
        quit.onClick.AddListener(onQuitClick);
        options.onClick.AddListener(onOptionsClick);
        quitOptions.onClick.AddListener(onQuitOptionsClick);

        menuObjects = GameObject.FindGameObjectsWithTag("MainMenu");
        optionsObject = GameObject.FindGameObjectWithTag("Options");      
        optionsObject.SetActive(false);
    }

    void onPlayClick() {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    void onOptionsClick() {
        foreach (GameObject item in menuObjects) {   
            item.SetActive(false);
        }
        optionsObject.SetActive(true);
    }

    void onQuitOptionsClick() {
        optionsObject.SetActive(false);
        foreach (GameObject item in menuObjects) {   
            item.SetActive(true);
        }
    }

    void onQuitClick() {
        Application.Quit();
    }
}