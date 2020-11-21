using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesScript : MonoBehaviour {

    private GameManager gameManager;
    public bool isSecretCollectibles;

    [SerializeField] ParticleSystem destroyEffect;

    private void Awake() {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            if (!isSecretCollectibles) {
                gameManager.playerData.CurrentGatheredCollectibles += 1;
            } else {
                gameManager.playerData.GatheredSecretCollectibles += 1;
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
