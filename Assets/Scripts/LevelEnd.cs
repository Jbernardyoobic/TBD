using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour {

    private LevelGenerator levelGenerator;

    private void Awake() {
        levelGenerator = GameObject.FindObjectOfType<LevelGenerator>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.name == "Player") {
            int level = levelGenerator.currentLevel + 1;
            levelGenerator.RegisterTime(levelGenerator.currentLevel);
            if (level < levelGenerator.levels.Length) {
                levelGenerator.GenerateLevel(level);
            } else {
                levelGenerator.EndGame();
            }
        }
    }
}
