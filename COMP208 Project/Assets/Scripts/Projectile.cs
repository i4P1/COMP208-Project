using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField]
    private LayerMask   playerLayerMask;
    [SerializeField]
    private LayerMask   enemyLayerMask;
    private Rigidbody2D rb;

    private Vector2 direction;
    [SerializeField]
    private float speed   = 100;
    private float damage  = 10;
    private bool  hostile = true;

    public Projectile(Vector2 dir) {
        direction = dir;
    }

    public Projectile(Vector2 dir, float spd) {
        direction = dir;
        speed     = spd;
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = speed * direction;
    }

    // Checks if a layer is in a layermask (or if two layermasks have any layers in common)
    private bool CheckLayer(LayerMask mask1, LayerMask mask2) {
        return (mask1 & mask2) > 0;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        GameObject target = other.gameObject;

        // If the projectile is hitting the player, and if it can damage them.
        if (CheckLayer(target.layer, playerLayerMask) && hostile) {
            target.SendMessage("Damage", damage);
        } // If the projectile is hitting an enemy, and if it can damage them.
        else if (CheckLayer(target.layer, enemyLayerMask) && !hostile) {
            target.SendMessage("Damage", damage);
        } // If it's hitting the wall
        else {
            OnWallHit();
        }
    }

    private void OnWallHit() {
        Die();
    }

    private void Die() {
        Destroy(gameObject);
    }

    public void Deflect() {
        direction *= -1;
        rb.velocity = speed * direction;
    }
}
