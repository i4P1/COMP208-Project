using UnityEngine;

public class Orb : Enemy, IMoveable {
    private float speed = 400;
    private Rigidbody2D rb;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction) {
        rb.AddForce(speed * direction * Time.deltaTime);
    }
}
