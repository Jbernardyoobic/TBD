using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTraps : MonoBehaviour {

    private GameManager gameManager;

    private void Awake() {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            int level = gameManager.currentLevel > 0 ? gameManager.currentLevel - 1 : 0;
            gameManager.PlayerDeath(other.gameObject, level);
        }
    }
}
