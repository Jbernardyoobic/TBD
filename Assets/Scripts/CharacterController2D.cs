using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour {
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowFallMultiplier;
    [SerializeField] private float wallCheckPos;
    [SerializeField] private Vector2 wallCheckSize;
    [SerializeField] private float m_WallJumpForce;

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    private bool isTouchingWallRight;
    private bool isTouchingWallLeft;
    private int isTouchingLeftOrRight;
    private bool canWallJump;



    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake() {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject) {
                m_Grounded = true;
            }
        }

        isTouchingWallRight = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x + wallCheckPos, gameObject.transform.position.y), wallCheckSize, 0f, m_WhatIsGround);
        isTouchingWallLeft = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x - wallCheckPos, gameObject.transform.position.y), wallCheckSize, 0f, m_WhatIsGround);

        if (isTouchingWallLeft) {
            isTouchingLeftOrRight = 1;
        } else if (isTouchingWallRight) {
            isTouchingLeftOrRight = -1;
        }

        if ((isTouchingWallLeft || isTouchingWallRight) && !m_Grounded) {
            canWallJump = true;
        } else {
            canWallJump = false;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x + wallCheckPos, gameObject.transform.position.y), wallCheckSize);
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x - wallCheckPos, gameObject.transform.position.y), wallCheckSize);
    }

    public void Move(float move, bool jump) {

        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        // If the input is moving the player right and the player is facing left...
        if ((move > 0 && !m_FacingRight)) {
            Flip();
        } else if ((move < 0 && m_FacingRight)) {
            Flip();
        }

        if (m_Grounded && jump) {
            // Add a vertical force to the player.
            m_Grounded = false;
            // m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            m_Rigidbody2D.velocity = new Vector2(0f, m_JumpForce);
        }

        if (!m_Grounded && jump && canWallJump) {
            m_Rigidbody2D.velocity = new Vector2(isTouchingLeftOrRight * 10f, m_WallJumpForce);
        }

        //Accelerate the fall of the player to get a better jump feeling
        if (m_Rigidbody2D.velocity.y < 0) {
            m_Rigidbody2D.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (m_Rigidbody2D.velocity.y > 0 && !Input.GetButton("Jump")) {
            m_Rigidbody2D.velocity += Vector2.up * Physics.gravity.y * (lowFallMultiplier - 1) * Time.deltaTime;
        }
    }

    public void Flip() {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        transform.Rotate(0f, 180f, 0f);
        transform.InverseTransformDirection(0f, 180f, 0f);
    }
}