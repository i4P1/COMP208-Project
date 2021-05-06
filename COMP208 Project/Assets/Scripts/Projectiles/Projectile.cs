using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField]
    protected LayerMask   playerLayerMask;
    [SerializeField]
    protected LayerMask   enemyLayerMask;
    protected Rigidbody2D rb;

    [SerializeField]
    protected Vector2 direction;
    [SerializeField]
    protected float speed  {get; set;} = 1;
    [SerializeField]
    protected float damage {get; set;} = 10;
    protected bool  hostile = true;

    protected virtual void Start() {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = speed * direction;
    }

    // Checks if a layer is in a layermask (or if two layermasks have any layers in common)
    protected virtual bool CheckLayer(LayerMask mask1, LayerMask mask2) {
        return (mask1 & mask2) > 0;
    }

    protected virtual void OnCollisionEnter2D(Collision2D other) {
        GameObject target = other.gameObject;

        // If the projectile is hitting the player, and if it can damage them.
        if (CheckLayer(target.layer, playerLayerMask) && hostile) {
            target.SendMessage("Damage", damage);
            Die();
        } // If the projectile is hitting an enemy, and if it can damage them.
        else if (CheckLayer(target.layer, enemyLayerMask) && !hostile) {
            target.SendMessage("Damage", damage);
            Die();
        } // If it's hitting the wall
        else {
            OnWallHit();
        }
    }

    protected virtual void OnWallHit() {
        Die();
    }

    protected virtual void Die() {
        Destroy(gameObject);
    }

    public virtual void Deflect() {
        direction *= -1;
        rb.velocity = speed * direction;
    }
}
