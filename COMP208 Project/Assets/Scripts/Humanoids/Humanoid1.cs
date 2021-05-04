using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Humanoid1 : Humanoids
{
    [SerializeField]
    EnemyAttacks ea;

    [SerializeField]
    float atkRange;
    [SerializeField]
    Vector2 attackTime;
    [SerializeField]
    float attackDamage;
    [SerializeField]
    GameObject prefabDamage;

    Coroutine atkSeq;

    #region setup
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        ea = GetComponent<EnemyAttacks>();
        eaSetUp();
        lockedMovement = false;
    }

    private void eaSetUp() {
        ea.setAttackTime(attackTime);
        ea.setPrefabs(prefabDamage);
    }
    #endregion

    #region attacking
    private void startAttack() {
        atkSeq = ea.startAttack(attackDamage);
    }

    #endregion

    #region pathfinding

    private void Update() {
        dir = findPlayer();
        if(checkFloor() && !lockedMovement && atkSeq == null && pc != null && (pc.transform.position - transform.position).magnitude < atkRange) 
            Move(new Vector2(dir.x, 0));
        else 
            Move(Vector2.zero);
    }

    #endregion
}
