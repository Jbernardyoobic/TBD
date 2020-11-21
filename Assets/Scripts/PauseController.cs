using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {
    [SerializeField] Canvas pauseObjects;

    private void Awake() {
        hidePaused();
    }

    void Update() {
        if (Input.GetButtonDown("Pause")) {
            if (Time.timeScale == 1) {
                showPaused();
            } else if (Time.timeScale == 0) {
                hidePaused();
            }
        }
    }

    public void showPaused() {
        pauseObjects.enabled = true;
        Time.timeScale = 0;
    }

    public void hidePaused() {
        pauseObjects.enabled = false;
        Time.timeScale = 1;
    }

    public void QuitGame() {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }
}
