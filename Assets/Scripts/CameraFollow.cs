using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Transform player;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;

    // private void Start() {
    //     player = GameObject.FindGameObjectWithTag("Player").transform;
    // }

    private void FixedUpdate() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        } else {
            Vector3 newPos = player.position + offset;
            Vector3 smoothedPostion = Vector3.Lerp(transform.position, newPos, smoothSpeed);
            transform.position = smoothedPostion;
        }
    }
}
