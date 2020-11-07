using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    GameObject[] pauseObjects;

    void Start () {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hidePaused();
    }

	void Update () {
		if (Input.GetKeyDown(KeyCode.P)) {
			if (Time.timeScale == 1) {
				Time.timeScale = 0;
				showPaused();
			} else if (Time.timeScale == 0) {
				Time.timeScale = 1;
				hidePaused();
			}
		}
	}

	public void showPaused(){
		foreach(GameObject g in pauseObjects){
			g.SetActive(true);
		}
	}

	public void hidePaused(){
		foreach(GameObject g in pauseObjects){
			g.SetActive(false);
		}
	}
}
