using UnityEngine;

public interface IMoveable {
    public void Move(Vector2 direction);
}

public class Enemy : MonoBehaviour {
    [SerializeField]
    private float health = 100;
    protected bool lockedMovement;

    [SerializeField]
    protected Animator animator;
    [SerializeField]
    private BoxCollider2D killbox;
    [SerializeField]
    protected float aggroRange = 14;

    public void lockMovement(bool state) {
        lockedMovement = state;
    }

    protected void resetFlip(Animator animator) {
        Transform t = animator.transform;
        t.localPosition = new Vector2(-Mathf.Abs(t.localPosition.x), t.localPosition.y);
        t.localScale = new Vector2(Mathf.Abs(t.localScale.x), t.localScale.y);
    }

    protected void flip(Animator animator) {
        Transform t = animator.transform;
        t.localPosition = new Vector3(-t.localPosition.x, t.localPosition.y, t.localPosition.z);
        t.localScale = new Vector3(-t.localScale.x, t.localScale.y, t.localScale.z);
    }

    public void Damage(float amount) {
        health -= amount;
        animator.SetTrigger("damaged");
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
        Debug.Log("Enemy trigger");
        if (other == killbox) {
            Debug.Log("Enemy dead");
            Die();
        }
    }
}
