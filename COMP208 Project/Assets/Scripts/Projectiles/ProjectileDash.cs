using UnityEngine;

public class ProjectileDash : Projectile {

    protected void Start() {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = speed * direction;
    }

    protected void Die() {
        Destroy(gameObject);
    }

    public void Deflect() {
        direction *= -1;
        rb.velocity = speed * direction;
    }
}
