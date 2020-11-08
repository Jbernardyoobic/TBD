using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("GameObject1 collided with " + col.name);
    }
}
