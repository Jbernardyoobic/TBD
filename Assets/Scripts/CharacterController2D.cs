using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour {
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowFallMultiplier;
    [SerializeField] private Vector2 wallCheckPos;
    [SerializeField] private Vector2 wallCheckSize;
    [SerializeField] private float m_WallJumpForce;
    [SerializeField] private float m_JumpFromWallForce;
    [SerializeField] private float dashPower;


    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    private bool isTouchingGround;
    private bool isTouchingWallRight;
    private bool isTouchingWallLeft;
    private int isTouchingLeftOrRight;
    private bool canWallJump;

    private DashState dashState = DashState.Ready;
    private float dashTimer = 0;
    private Vector2 savedVelocity;


    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake() {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        m_Grounded = false;

        isTouchingGround = Physics2D.OverlapBox(new Vector2(m_GroundCheck.position.x, m_GroundCheck.position.y), groundCheckSize, 0f, m_WhatIsGround);
        isTouchingWallRight = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x + wallCheckPos.x, gameObject.transform.position.y + wallCheckPos.y), wallCheckSize, 0f, m_WhatIsGround);
        isTouchingWallLeft = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x - wallCheckPos.x, gameObject.transform.position.y + wallCheckPos.y), wallCheckSize, 0f, m_WhatIsGround);

        if (isTouchingGround) {
            m_Grounded = true;
        }

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

        this.updateDash();
    }

    private void updateDash() {
        switch (dashState) {
            case DashState.Ready:
                var isDashKeyDown = Input.GetKeyDown(KeyCode.LeftShift);
                if (isDashKeyDown) {
                    savedVelocity = m_Rigidbody2D.velocity;
                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x * dashPower, m_Rigidbody2D.velocity.y);
                    dashState = DashState.Cooldown;
                    dashTimer += Time.deltaTime * 3;
                }
                break;
            case DashState.Cooldown:
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0) {
                    dashTimer = 0;
                    dashState = DashState.Ready;
                }
                break;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x + wallCheckPos.x, gameObject.transform.position.y + wallCheckPos.y), wallCheckSize);
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x - wallCheckPos.x, gameObject.transform.position.y + wallCheckPos.y), wallCheckSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(new Vector2(m_GroundCheck.position.x, m_GroundCheck.position.y), groundCheckSize);
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
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }

        if (!m_Grounded && jump && canWallJump) {
            m_Rigidbody2D.velocity = new Vector2(isTouchingLeftOrRight * m_JumpFromWallForce, m_WallJumpForce);
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

public enum DashState {
    Ready,
    Cooldown
}