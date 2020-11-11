using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TimerHandler : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI timer;
    private float levelTime;
    private bool isCounting = false;

    public float LevelTime { get => levelTime; set => levelTime = value; }
    public bool IsCounting { get => isCounting; set => isCounting = value; }

    void Update() {
        if (IsCounting) {
            LevelTime += Time.deltaTime;
        }
        timer.SetText("Time : " + LevelTime.ToString("F2"));
    }

    public void ResetStopWatch() {
        LevelTime = 0;
        IsCounting = true;
    }

    public void EndStopWatch() {
        IsCounting = false;
    }
}
