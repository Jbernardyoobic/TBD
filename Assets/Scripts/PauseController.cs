using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour {
    [SerializeField] Canvas pauseObjects;

    private void Awake() {
        hidePaused();
    }

    void Update() {
        if (Input.GetButtonDown("Pause")) {
            if (Time.timeScale == 1) {
                Time.timeScale = 0;
                showPaused();
            } else if (Time.timeScale == 0) {
                Time.timeScale = 1;
                hidePaused();
            }
        }
    }

    public void showPaused() {
        pauseObjects.enabled = true;
    }

    public void hidePaused() {
        pauseObjects.enabled = false;
    }
}
