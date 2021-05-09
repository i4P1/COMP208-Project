using UnityEngine;

public interface IMoveable {
    public void Move(Vector2 direction);
}

public class Enemy : MonoBehaviour {
    [SerializeField]
    private float health;
    protected bool lockedMovement;
    [SerializeField]
    private LayerMask killBoxLayerMask;

    public void lockMovement(bool state) {
        lockedMovement = state;
    }

    public void Damage(float amount) {
        health -= amount;

        if (health <= 0) {
            Die();
        }
        //Call animator if not dead
    }

    private void Die() {
        // Call animator
        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (Mathf.Pow(2, other.gameObject.layer) == killBoxLayerMask.value) {
            Die();
        }
    }
}
