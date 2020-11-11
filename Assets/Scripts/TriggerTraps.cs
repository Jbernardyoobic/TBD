using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTraps : MonoBehaviour {

    private LevelGenerator levelGenerator;

    private void Awake() {
        levelGenerator = GameObject.FindObjectOfType<LevelGenerator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Player") {
            Debug.Log("is touching player");
            int level = levelGenerator.currentLevel > 0 ? levelGenerator.currentLevel - 1 : 0;
            levelGenerator.GenerateLevel(level);
        }
    }
}
