using System.Collections;
using UnityEngine;

public class Humanoid2 : Humanoids
{
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

    [SerializeField]
    float tpDashDelay;

    [SerializeField]
    float dashSpeed;
    [SerializeField]
    float dashTime;

    float privVel = 1;

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
        ea.setAttackTime(dashTime);
        ea.setPrefabs(prefabDamage);
    }
    #endregion

    #region attacking
    private void startAttack() {
        atkCoro = ea.startAttack(attackDamage);
    }
    #endregion

    #region Movement

    private IEnumerator attackSequence() {
        
        lockedMovement = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        #region tp over player
        yield return new WaitForSeconds(1.5f); //Change to appropriate waiting time
        //Call animator
        Vector2 pos = pc.transform.position + (Vector3)tpOffset;
        yield return new WaitForSeconds(0.5f); //Change to appropriate waiting time
        transform.position = pos;
        #endregion

        animator.SetTrigger("attack");
        transform.rotation = Quaternion.Euler(0, 0, -90);
        privVel = 1;
        resetFlip(animator);


        yield return new WaitForSeconds(tpDashDelay); //Change to appropriate waiting time
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        ea.startAttack(attackDamage);

        #region dash down
        float temp = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(0, -dashSpeed); //Change to appropriate dash speed
        yield return new WaitForSeconds(dashTime); //Change to appropriate waiting time
        rb.velocity = Vector2.zero;
        rb.gravityScale = temp;
        #endregion

        transform.rotation = Quaternion.Euler(0, 0, 0);

        lockedMovement = false;
        endAttack();
    }

    private void endAttack() {
        StopCoroutine(atkSeq);
        atkSeq = null;
    }

    private void Update() {

        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        if(rb.velocity.x != 0) {
            if(privVel / Mathf.Abs(privVel) != rb.velocity.x / Mathf.Abs(rb.velocity.x))
                flip(animator);
            privVel = rb.velocity.x;
        }

        dir = findPlayer();
        if(checkFloor() && atkSeq == null && pc != null && (pc.transform.position - transform.position).magnitude < tpDistance) {
            atkSeq = StartCoroutine(attackSequence());
        }
        else if(pc != null && checkFloor() && !lockedMovement && (pc.transform.position - transform.position).magnitude < aggroRange)
            Move(new Vector2(dir.x, 0));
        else if(atkSeq == null)
            Move(Vector2.zero);
    }

    #endregion
}
