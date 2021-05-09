using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid1 : Humanoids
{
    [SerializeField]
    EnemyAttacks ea;

    [SerializeField]
    float agroRange = 14;
    [SerializeField]
    float attackRange;
    [SerializeField]
    float attackCooldown = 1;
    float lastAttackTime;
    [SerializeField]
    Vector2 attackTime;
    [SerializeField]
    float attackDamage;
    [SerializeField]
    GameObject prefabDamage;

    float privVel = 1;

    Coroutine atkSeq;

    #region setup
    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        ea = GetComponent<EnemyAttacks>();
        eaSetUp();
        lockedMovement = false;
        lastAttackTime = -attackCooldown;
    }

    private void eaSetUp() {
        ea.setAttackTime(attackTime);
        ea.setPrefabs(prefabDamage);
    }
    #endregion

    #region attacking
    private void startAttack(float? xMult) {
        ea.startAttack(attackDamage, xMult);
        animator.SetTrigger("attack");
    }

    #endregion

    #region pathfinding

    private void Update() {
        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        if(rb.velocity.x != 0) {
            if(privVel / Mathf.Abs(privVel) != rb.velocity.x / Mathf.Abs(rb.velocity.x))
                flip(animator);
            privVel = rb.velocity.x;
        }
        dir = findPlayer();
        if(checkFloor() && !lockedMovement && atkSeq == null && pc != null && (pc.transform.position - transform.position).magnitude < agroRange) 
            Move(new Vector2(dir.x, 0));
        else 
            Move(Vector2.zero);

        if ((pc.transform.position - transform.position).magnitude < attackRange && (Time.time - lastAttackTime) > attackCooldown + attackTime.x + attackTime.y) {
            lastAttackTime = Time.time;

            float? mult = null;

            if (dir.x > 0.1f) {
                mult = 1;
            } else if (dir.x < -0.1f) {
                mult = -1;
            }

            startAttack(mult);
        } else {
            atkSeq = ea.attackStatus();
        }
    }

    #endregion
}
