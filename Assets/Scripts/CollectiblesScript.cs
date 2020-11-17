using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesScript : MonoBehaviour {

    private PlayerData playerData;
    public bool isSecretCollectibles;

    [SerializeField] ParticleSystem destroyEffect;

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
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            destroyEffect.Play();
            StartCoroutine(DestroyGameObject());
        }
    }

    IEnumerator DestroyGameObject() {
        yield return new WaitUntil(() => !destroyEffect.isPlaying);
        Destroy(gameObject);
    }
}
