using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TimerHandler : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI timer;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        timer.SetText("Time : " + System.Math.Round(Time.time, 2) + " s");
    }
}
