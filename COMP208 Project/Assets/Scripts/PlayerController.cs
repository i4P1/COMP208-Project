using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    PlayerInput input;
    Animator animator;
    Rigidbody2D rb;

    int direction;
    float speed = 10;
    Vector2 mobility_direction;

    bool dashing;
    float dash_start_time;
    float dash_speed;
    float dash_duration;

    bool teleporting;
    float teleport_distance;
    float step;

    // Start is called before the first frame update
    void Start() {
        rb    = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update() {
        float new_y = rb.position.x + (input.actions["Move"].ReadValue<float>() * speed * Time.deltaTime);
        rb.position = new Vector2(new_y, rb.position.y);
    }

    public void OnJump(InputAction.CallbackContext context) {
        
    }

    public void OnCrouch(InputAction.CallbackContext context) {
        
    }

    public void OnLight(InputAction.CallbackContext context) {
        
    }

    public void OnHeavy(InputAction.CallbackContext context) {
        
    }
}
