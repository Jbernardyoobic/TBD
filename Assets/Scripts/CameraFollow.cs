using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Transform player;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;

    private void FixedUpdate() {
        try {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            Vector3 newPos = player.position + offset;
            Vector3 smoothedPostion = Vector3.Lerp(transform.position, newPos, smoothSpeed);
            transform.position = smoothedPostion;
        } catch (NullReferenceException ex) {
            Debug.Log(ex);
            player = gameObject.transform;
        }
    }
}
