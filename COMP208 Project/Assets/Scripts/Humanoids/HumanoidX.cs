using System.Collections;
using UnityEngine;

public class HumanoidX : Humanoids {
    [SerializeField]
    Vector2 attackTime;
    [SerializeField]
    float attackDamage;
    [SerializeField]
    GameObject prefabDamage;

    EnemyAttacks ea;
    EnemyAI ai;
    Coroutine atkCoro;

    [SerializeField]
    float tpDistance;
    [SerializeField]
    Vector2 tpOffset;

    Coroutine atkSeq;

    #region setup
    // Start is called before the first frame update
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
    #endregion

    #region attacking
    private void startAttack() {
        atkCoro = ea.startAttack(attackDamage);
    }
    #endregion

    #region Movement

    private IEnumerator AttackSequence() {

        #region tp over player
        //Call animator
        Vector2 pos = pc.transform.position;
        yield return new WaitForSeconds(1); //Change to appropriate waiting time
        if(pc != null && (pc.transform.position - transform.position).magnitude < tpDistance)
            transform.position = pos;
        #endregion

        #region dash down
        rb.velocity = new Vector2(0, -1); //Change to appropriate dash speed
        yield return new WaitForSeconds(1); //Change to appropriate waiting time
        rb.velocity = Vector2.zero;
        #endregion
    }

    private void Update() {
        findPlayer();
        if(pc != null && (pc.transform.position - transform.position).magnitude < tpDistance) {
            atkSeq = StartCoroutine(AttackSequence());
        }
    }

    #endregion
}
