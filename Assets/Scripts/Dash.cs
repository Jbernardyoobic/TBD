using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dash : MonoBehaviour {
    
    [SerializeField] private float dashPower;

    private float maxDash = 1;
    private DashState dashState = DashState.Ready;
    private float dashTimer = 0;
    private Vector2 savedVelocity;
    private Rigidbody2D m_Rigidbody2D;

    private void Awake() {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    void Update() {
        switch (dashState) {
            case DashState.Ready:
                var isDashKeyDown = Input.GetKeyDown(KeyCode.LeftShift);
                if (isDashKeyDown) {
                    savedVelocity = m_Rigidbody2D.velocity;
                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x * dashPower, m_Rigidbody2D.velocity.y);
                    dashState = DashState.Dashing;
                }
                break;
            case DashState.Dashing:
                dashTimer += Time.deltaTime * 3;
                if (dashTimer >= maxDash) {
                    dashTimer = maxDash;
                    m_Rigidbody2D.velocity = savedVelocity;
                    dashState = DashState.Cooldown;
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
}

public enum DashState {
    Ready,
    Dashing,
    Cooldown
}