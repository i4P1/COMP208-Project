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
        
        Vector3 temp = pc.transform.position;
        
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
        endAttack();
    }

    private void endAttack() {
        StopCoroutine(atkSeq);
        atkSeq = null;
    }

    // Update is called once per frame
    void Update() {
        dir = findPlayer();
        if(checkFloor() && atkSeq == null && pc != null && (pc.transform.position - transform.position).magnitude < attackRange) {
            atkSeq = StartCoroutine(attackSequence());
        }
        else if(checkFloor() && !lockedMovement) {
            Move(new Vector2(dir.x, 0));
        }
        else if(atkSeq == null)
            Move(Vector2.zero);
    }
}
