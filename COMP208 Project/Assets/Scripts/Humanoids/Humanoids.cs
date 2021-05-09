using UnityEngine;

public class Humanoids : Enemy
{

    protected Rigidbody2D rb;
    [SerializeField]
    protected float speed;
    protected Vector2 dir;
    protected Vector2 raycastOffset;
    protected PlayerController pc;
    protected float xScale;

    /// <summary>
    /// Layermask for the player
    /// </summary>
    [SerializeField]
    protected LayerMask playerLayerMask;
    /// <summary>
    /// Layermask for the ground
    /// </summary>
    [SerializeField]
    protected LayerMask groundLayerMask;

    protected virtual void Start() {
        xScale = GetComponent<RectTransform>().localScale.x;
    }

    protected void Move(Vector2 direction) {
        if(direction == Vector2.zero)
            rb.velocity = new Vector2(0, rb.velocity.y);
        else
            rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
    }

    protected bool checkFloor() {
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 castOrigin = new Vector2(transform.position.x + dir.normalized.x * (raycastOffset.x + rt.rect.width / 2), transform.position.y);
        if(Physics2D.Raycast(castOrigin, Vector2.down, rt.rect.height /2 + 0.1f, groundLayerMask).collider != null) return true;
        return false;
    }

    protected Vector2 findPlayer() {
        LayerMask enemyLayer = 1 << gameObject.layer;
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 100 /* Change */, Vector2.one, 1, playerLayerMask);
        pc = hit.transform.gameObject.GetComponent<PlayerController>();
        if(hit.transform == null) return Vector2.zero;
        Vector2 direction = (hit.transform.position - transform.position).normalized;
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, direction, 100, ~enemyLayer);
        if(hit.transform.gameObject == hit2.transform.gameObject)
            return direction;
        else
            return Vector2.zero;
    }

}
