using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Humanoid1 : Enemy
{
    [SerializeField]
    Vector2 attackTime;
    [SerializeField]
    float attackDamage;
    [SerializeField]
    GameObject prefabDamage;

    EnemyAttacks ea;

    EnemyAI ai;

    Coroutine atkCoro;

    #region setup
    // Start is called before the first frame update
    void Start() {
        ea = GetComponent<EnemyAttacks>();
        eaSetUp();
    }

    private void eaSetUp() {
        ea.setAttackTime(attackTime);
        ea.setPrefabs(prefabDamage);
    }
    #endregion

    #region attacking
    private void startAttack() {
        atkCoro = ea.startAttack(attackDamage);
    }
    #endregion

    #region pathfinding



    #endregion
}
