using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesScript : MonoBehaviour {

    private PlayerData playerData;
    public bool isSecretCollectibles;

    private void Awake() {
        playerData = GameObject.FindObjectOfType<PlayerData>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Player") {
            if (!isSecretCollectibles) {
                playerData.CurrentGatheredCollectibles += 1;
            } else {
                playerData.GatheredSecretCollectibles += 1;
            }
            Destroy(gameObject);
        }
    }
}
