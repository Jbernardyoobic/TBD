using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTraps : MonoBehaviour {

    private GameManager levelGenerator;

    private void Awake() {
        levelGenerator = GameObject.FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Player") {
            int level = levelGenerator.currentLevel > 0 ? levelGenerator.currentLevel - 1 : 0;
            levelGenerator.GenerateLevel(level);
        }
    }
}
