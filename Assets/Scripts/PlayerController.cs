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
    private bool releasedJumpButton;
    private bool dash;

    private void Update() {
        playerDirection = Input.GetAxisRaw("Horizontal") * speed;

        if (Input.GetButtonDown("Jump")) {
            jump = true;
            releasedJumpButton = false;
        }
        if (Input.GetButtonUp("Jump")) {
            releasedJumpButton = true;
        }
        if (Input.GetButtonDown("Fire2")) {
            dash = true;
        }
    }

    private void FixedUpdate() {
        controller2D.Move(playerDirection * Time.deltaTime, jump, dash, releasedJumpButton);
        jump = false;
        dash = false;
    }

}
