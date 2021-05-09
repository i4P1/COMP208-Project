using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private HealthBar healthbar;
    [SerializeField]
    private float maxHealth = 100;
    private float health;

    private const int MAX_JUMPS = 1;

    [SerializeField]
    private LayerMask groundLayerMask;
    [SerializeField]                                  
    private LayerMask playerLayerMask;

    private float playerSize;
    private PlayerInput input;
    private Rigidbody2D rb;

    private float direction;
    private int jumpsLeft = MAX_JUMPS;
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float jumpSpeed = 40;
    private float distToGround;

    private bool dashing = false;
    private bool canDash = true;
    [SerializeField]
    private float dashSpeed = 40;
    [SerializeField]
    private float dashDuration = 0.5f;
    private float dashStartTime;
    private Vector2 dashDirection;

    private float teleportStartTime;
    [SerializeField]
    private float teleportDistance = 10;
    private float step = 0.1f;
    [SerializeField]
    private float teleportCooldown = 3;

    private float hoverStartTime;
    private float hoverDuration;
    [SerializeField]
    private float floatTime;

    // Start is called before the first frame update
    private void Start() {
        Vector3 colliderExtents = GetComponent<BoxCollider2D>().bounds.extents;

        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        distToGround = colliderExtents.y;
        playerSize = Math.Max(colliderExtents.x, colliderExtents.y);
        health = maxHealth;
        direction = 1;
        animator.SetBool("dashing", false);
    }

    private void FixedUpdate() {
        // Stop dashing after its duration
        if((Time.time - dashStartTime) > dashDuration && dashing) {
            dashing = false;
            animator.SetBool("dashing", false);
            rb.velocity = Vector2.zero;
        }

        // Set the player's speed
        if(dashing) {
            rb.velocity = dashDirection.normalized * dashSpeed;
        }
    }

    private void Update() {
        if((Time.time - hoverStartTime) < hoverDuration) {
            rb.velocity = Vector2.zero;
        }
        else {
            // Stop dashing after its duration
            if((Time.time - dashStartTime) > dashDuration && dashing) {
                dashing = false;
                animator.SetBool("dashing", false);
                Hover(floatTime);
            }

            // Set the player's speed
            if(dashing) {
                rb.velocity = dashDirection.normalized * dashSpeed;
            }
            else {
                float xVel = input.actions["Move"].ReadValue<float>() * speed;
                rb.velocity = new Vector2(xVel, rb.velocity.y);
                if(xVel != 0) {
                    if(direction / Mathf.Abs(direction) != xVel / Mathf.Abs(xVel))
                        flip();
                    direction = xVel;
                }
            }
        }

        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        if(Grounded()) {
            animator.SetBool("airborn", false);
            if (jumpsLeft < MAX_JUMPS) {
                jumpsLeft = MAX_JUMPS;
            }
        }
        else {
            animator.SetBool("airborn", true);
        }
    }

    private void flip() {
        Transform t = animator.transform;
        t.localPosition = new Vector3(-t.localPosition.x, t.localPosition.y, t.localPosition.z);
        t.localScale = new Vector3(-t.localScale.x, t.localScale.y, t.localScale.z);
    }

    // Checks if the player is touching the ground
    private bool Grounded() {
        bool raycastHit = Physics2D.Raycast(transform.position, Vector2.down, distToGround + 0.2f, groundLayerMask);

        if(raycastHit) {
            canDash = true;
        }

        return raycastHit;
    }

    private void Jump() {
        // Sets the y velocity to the jump speed
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }

    public void OnJump(InputAction.CallbackContext context) {
        if(context.action.triggered) {
            bool canJump = false;

            // Resets the jumps left and lets the player jump if they're grounded
            if(Grounded()) {
                jumpsLeft = MAX_JUMPS;
                canJump = true;
            } // Lets the player jump if they have jumps left and decrements it
            else if(jumpsLeft-- > 0) {
                canJump = true;
            }

            if(canJump) {
                Jump();
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context) {
        Grounded();
        // If we can dash we set the relevent variables
        if(context.action.triggered && canDash) {
            dashStartTime = Time.time;
            dashing = true;
            animator.SetBool("dashing", true);
            dashDirection = GetComponent<PlayerInput>().actions["Aim"].ReadValue<Vector2>();
            canDash = false;
        }
    }

    private void Teleport() {
        Vector2 direction = GetComponent<PlayerInput>().actions["Aim"].ReadValue<Vector2>();
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        Vector2 destination = origin + (teleportDistance * direction);

        for(float distance = teleportDistance;distance > 0;distance -= step) {
            destination = origin + (direction * distance);

            // If the destination is empty then break from the loop to move there
            if(Physics2D.CircleCast(destination, playerSize, Vector2.up, 0, ~playerLayerMask).collider == null) {
                break;
            }
        }

        transform.position = destination;
    }

    public void OnTeleport(InputAction.CallbackContext context) {
        // If we can teleport we set the relevent variables and call the teleport function
        if(context.action.triggered && (teleportStartTime + teleportCooldown) < Time.time) {
            teleportStartTime = Time.time;
            Teleport();
            Hover(floatTime);
        }
    }

    public void ResetDash() {
        canDash = true;
    }

    public void Hover(float duration) {
        hoverStartTime = Time.time;
        hoverDuration = duration;
    }

    public void Damage(float amount) {
        health = Math.Min((health - amount), maxHealth);
        healthbar.SetHealth(health);

        if(amount > 0)
            animator.SetTrigger("takeDamage");

        if (health <= 0) {
            Die();
        }
    }

    public void Die() {
        SceneManager.LoadScene(0);
    }
}
