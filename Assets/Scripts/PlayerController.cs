using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {
    [SerializeField] private CharacterController2D controller2D;
    [SerializeField] private float speed = 100f;

    private Rigidbody2D playerRigidbody;
    private float playerDirection;
    private bool jump;
    private bool dash;

    private void Update() {
        playerDirection = Input.GetAxisRaw("Horizontal") * speed;

        if (Input.GetButtonDown("Jump")) {
            jump = true;
        }
        if (Input.GetButtonDown("Fire2")) {
            dash = true;
        }
    }

    private void FixedUpdate() {
        controller2D.Move(playerDirection * Time.deltaTime, jump, dash);
        jump = false;
        dash = false;
    }

}
