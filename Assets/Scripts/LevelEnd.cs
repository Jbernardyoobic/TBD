using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour {

    private GameManager gameManager;

    private void Awake() {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            int level = gameManager.currentLevel + 1;
            gameManager.RegisterTime(gameManager.currentLevel);
            gameManager.RegisterCollectibles(gameManager.currentLevel);
            gameManager.EndLevel(gameManager.currentLevel);
        }
    }
}
