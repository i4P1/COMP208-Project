using UnityEngine;

public class ProjectileDash : Projectile {

    protected override void Start() {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = speed * direction;
    }

    protected override void Die() {
        Destroy(gameObject);
    }

    public override void Deflect() {
        direction *= -1;
        rb.velocity = speed * direction;
    }
}
