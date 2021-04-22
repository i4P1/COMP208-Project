using UnityEngine;

public interface IMoveable {
    public void Move(Vector2 direction);
}

public class Enemy : MonoBehaviour {
    private float health;

    public void Damage(float amount) {
        health -= amount;

        if (health <= 0) {
            Die();
        }
        //Call animator if not dead
    }

    private void Die() {
        // Kill yourself -> call animator
    }
}
