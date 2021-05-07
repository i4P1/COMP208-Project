using UnityEngine;

public class Orb : Enemy, IMoveable {
    [SerializeField]
    private float damage = 40;
    [SerializeField]
    private float knockback = 800;
    private float speed = 400;
    private Rigidbody2D rb;
    [SerializeField]
    private LayerMask playerLayerMask;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction) {
        rb.AddForce(speed * direction * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == playerLayerMask.value) {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            player.Damage(damage);
            rb.AddForce(rb.velocity.normalized * -knockback);
        }
    }
}
