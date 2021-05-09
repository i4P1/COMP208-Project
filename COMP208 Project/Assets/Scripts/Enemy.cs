using UnityEngine;

public interface IMoveable {
    public void Move(Vector2 direction);
}

public class Enemy : MonoBehaviour {
    private float health;
    protected bool lockedMovement;

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
}
