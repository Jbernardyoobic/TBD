using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour {
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private float jumpFallMultiplier;
    [SerializeField] private float wallJumpFallMultiplier;
    [SerializeField] private Vector2 wallCheckPos;
    [SerializeField] private Vector2 wallCheckSize;
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [SerializeField] private float m_WallJumpForce;
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private float m_JumpFromWallForce;
    [SerializeField] private float dashPower;

    [SerializeField] private ParticleSystem dustFeet;
    [SerializeField] private ParticleSystem dustBodyRight;
    [SerializeField] private ParticleSystem dustBodyLeft;
    [SerializeField] private ParticleSystem dustOnImpact;
    [SerializeField] private ParticleSystem dashParticle;
    [SerializeField] private ParticleSystem deathParticle;

    [SerializeField] private Animator animator;



    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private bool wasGrouned;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    private bool isTouchingGround;
    private bool isTouchingWallRight;
    private bool isTouchingWallLeft;
    private int isTouchingLeftOrRight;
    private bool canWallJump;
    private bool hasWallJump;

    private DashState dashState = DashState.Ready;
    private Vector2 savedVelocity;


    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake() {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        m_Grounded = false;

        isTouchingGround = Physics2D.OverlapBox(new Vector2(m_GroundCheck.position.x, m_GroundCheck.position.y), groundCheckSize, 0f, m_WhatIsGround);
        isTouchingWallRight = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x + wallCheckPos.x, gameObject.transform.position.y + wallCheckPos.y), wallCheckSize, 0f, m_WhatIsGround);
        isTouchingWallLeft = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x - wallCheckPos.x, gameObject.transform.position.y + wallCheckPos.y), wallCheckSize, 0f, m_WhatIsGround);

        if (isTouchingGround) {
            m_Grounded = true;
            hasWallJump = false;
        }

        if (isTouchingWallLeft) {
            isTouchingLeftOrRight = 1;
        } else if (isTouchingWallRight) {
            isTouchingLeftOrRight = -1;
        }

        if ((isTouchingWallLeft || isTouchingWallRight)) {
            canWallJump = true;
            hasWallJump = false;
        } else {
            canWallJump = false;
        }

        if (!wasGrouned && m_Grounded) {
            CreateDustOnImpact();
        }

        wasGrouned = m_Grounded;
        animator.SetBool("Grounded", m_Grounded);
        animator.SetBool("Dash", dashState == DashState.Cooldown);
        animator.SetFloat("Speed", Mathf.Abs(m_Rigidbody2D.velocity.x));
        animator.SetFloat("FallSpeed", Mathf.Abs(m_Rigidbody2D.velocity.y));
    }

    private void updateDash(bool isDashKeyDown) {
        switch (dashState) {
            case DashState.Ready:
                if (isDashKeyDown) {
                    savedVelocity = m_Rigidbody2D.velocity;
                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x * dashPower * Time.deltaTime, m_Rigidbody2D.velocity.y);
                    dashState = DashState.Cooldown;
                    CreateDashParticle();
                }
                break;
            case DashState.Cooldown:
                if (m_Grounded) {
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

    public void Move(float move, bool jump, bool dash, bool releasedJumpButton) {

        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
        if ((isTouchingWallLeft || isTouchingWallRight) && !m_Grounded) {
            //Wall Slide
            CreateDustOnBody();
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y * wallSlideSpeed);
        } else {
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        }

        // If the input is moving the player right and the player is facing left...
        if ((move > 0 && !m_FacingRight)) {
            Flip();
        } else if ((move < 0 && m_FacingRight)) {
            Flip();
        }

        //Jump
        if (m_Grounded && jump) {
            // Add a vertical force to the player.
            CreateDustOnFeet();
            m_Grounded = false;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce * Time.deltaTime);
        }

        //Wall Jump
        if (!m_Grounded && jump && canWallJump) {
            m_Rigidbody2D.velocity = new Vector2(isTouchingLeftOrRight * m_JumpFromWallForce * Time.deltaTime, m_WallJumpForce * Time.deltaTime);
            hasWallJump = true;
        }

        //Accelerate the fall of the player after a jump or dash
        if (m_Rigidbody2D.velocity.y < 0 || (m_Rigidbody2D.velocity.y > 0 && releasedJumpButton) || dashState == DashState.Cooldown && !hasWallJump) {
            m_Rigidbody2D.velocity += Vector2.up * Physics.gravity.y * (jumpFallMultiplier - 1) * Time.deltaTime;
        }

        //Accelerate the fall of the player after a wall jump
        if (hasWallJump && releasedJumpButton) {
            m_Rigidbody2D.velocity += Vector2.up * Physics.gravity.y * (wallJumpFallMultiplier) * Time.deltaTime;
        } else if (hasWallJump && m_Rigidbody2D.velocity.y > 0 && !releasedJumpButton) {
            m_Rigidbody2D.velocity += Vector2.up * Physics.gravity.y * (wallJumpFallMultiplier - 1) * Time.deltaTime;
        }

        updateDash(dash);
    }

    public void Flip() {
        // Switch the way the player is labelled as facing.
        CreateDustOnFeet();
        m_FacingRight = !m_FacingRight;
        transform.Rotate(0f, 180f, 0f);
        transform.InverseTransformDirection(0f, 180f, 0f);
    }

    public void CreateDustOnFeet() {
        dustFeet.Play();
    }

    public void CreateDustOnBody() {

        if (isTouchingWallRight) {
            if (m_FacingRight) {
                dustBodyRight.Play();
            } else {
                dustBodyLeft.Play();
            }
        } else if (isTouchingWallLeft) {
            if (!m_FacingRight) {
                dustBodyRight.Play();
            } else {
                dustBodyLeft.Play();
            }
        }

    }

    public void CreateDustOnImpact() {
        dustOnImpact.Play();
    }

    public void CreateDashParticle() {
        dashParticle.Play();
    }

    public void CreateDeathEffect() {
        deathParticle.Play();
    }
}

public enum DashState {
    Ready,
    Cooldown
}