using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour {

    private LevelGenerator levelGenerator;

    private void Awake() {
        levelGenerator = GameObject.FindObjectOfType<LevelGenerator>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("GameObject1 collided with " + col.name);
        if (col.name == "Player") {
            int level = levelGenerator.currentLevel + 1 < levelGenerator.levels.Length ? levelGenerator.currentLevel + 1 : 0;
            levelGenerator.GenerateLevel(level);
        }
    }
}
