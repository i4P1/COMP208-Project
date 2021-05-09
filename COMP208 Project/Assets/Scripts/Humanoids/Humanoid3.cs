using System.Collections;
using UnityEngine;

public class Humanoid3 : Humanoids
{

    [SerializeField]
    Vector2 attackTime;
    [SerializeField]
    float attackDamage;
    [SerializeField]
    GameObject prefabDamage;

    [SerializeField]
    float dashDelay;
    [SerializeField]
    float dashSpeed;
    [SerializeField]
    float dashTime;
    [SerializeField]
    float attackRange;

    float privVel = 1;

    EnemyAttacks ea;

    Coroutine atkSeq;

    protected override void Start() {
        base.Start();
        ea = GetComponent<EnemyAttacks>();
        rb = GetComponent<Rigidbody2D>();
        eaSetUp();
    }

    private void eaSetUp() {
        ea.setAttackTime(attackTime);
        ea.setPrefabs(prefabDamage);
    }

    IEnumerator attackSequence() {
        lockMovement(true);
        
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(dashDelay/2);

        animator.SetTrigger("attack");
        Vector3 temp = pc.transform.position;
        float angle = Vector2.SignedAngle(transform.position, temp);
        resetFlip(animator);
        if(temp.x-transform.position.x >= 0) {
            transform.rotation = Quaternion.Euler(0, 0, -angle);
            privVel = 1;
        }
        else {
            transform.rotation = Quaternion.Euler(0, 0, angle);
            Debug.Log("flipped");
            flip(animator);
            privVel = -1;
        }

        yield return new WaitForSeconds(dashDelay/2);
        
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        float tempg = rb.gravityScale;
        rb.gravityScale = 0;
        Debug.Log((temp - transform.position).normalized);
        rb.velocity = (temp - transform.position).normalized * dashSpeed;
        yield return new WaitForSeconds(dashTime);
        rb.velocity = Vector2.zero;
        rb.gravityScale = tempg;

        transform.rotation = Quaternion.Euler(0, 0, 0);
        lockMovement(false);

        endAttack();
    }

    private void endAttack() {
        StopCoroutine(atkSeq);
        atkSeq = null;
    }

    // Update is called once per frame
    void Update() {

        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        if(rb.velocity.x != 0) {
            if(privVel / Mathf.Abs(privVel) != rb.velocity.x / Mathf.Abs(rb.velocity.x))
                flip(animator);
            privVel = rb.velocity.x;
        }

        dir = findPlayer();
        if(checkFloor() && atkSeq == null && pc != null && (pc.transform.position - transform.position).magnitude < attackRange) {
            atkSeq = StartCoroutine(attackSequence());
        }
        else if(checkFloor() && !lockedMovement && (pc.transform.position - transform.position).magnitude < aggroRange) {
            Move(new Vector2(dir.x, 0));
        }
        else if(atkSeq == null)
            Move(Vector2.zero);
    }
}
