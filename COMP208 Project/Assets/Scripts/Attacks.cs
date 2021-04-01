using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attacks : MonoBehaviour
{
    #region VariableSetting
    /// <summary>
    /// Locks the attack so that no other attack can start
    /// </summary>
    AttackList?[] attackQue;
    /// <summary>
    /// x == delay until damage starts || y == duration of damage
    /// </summary>
    public Vector2 light_time;
    /// <summary>
    /// x == delay until damage starts || y == duration of damage
    /// </summary>
    public Vector2 heavy_time;
    /// <summary>
    /// The current active attack, to be used for cancelling attacks to move to the next ones.
    /// </summary>
    private Coroutine activeAttack;
    /// <summary>
    /// A lock to indicate that no new attack can start.
    /// </summary>
    private bool attackLock;
    /// <summary>
    /// Prefabs of the damage hitbox objects ||
    /// 0 - light1 || 1 - light2 || 2 - Heavy
    /// </summary>
    public GameObject[] prefabsDamage;
    /// <summary>
    /// Prefabs of the deflection hitbox objects ||
    /// 0 - light1 || 1 - light2 || 2 - Heavy
    /// </summary>
    public GameObject[] prefabsDeflect;
    /// <summary>
    /// Layermask for the enemies
    /// </summary>
    public LayerMask enemyLayerMask = 7;
    /// <summary>
    /// Layermask for the projectiles
    /// </summary>
    public LayerMask projectileLayerMask = 6;
    [SerializeField]
    ComboCounter comboCounter;
    //Player player;
    #endregion

    private void Start() {
        attackLock = false;
        attackQue = new AttackList?[] { null, null };
    }

    // Update is called once per frame
    void Update() {
        if(!attackLock) {
            switch(attackQue[0]) {
                case AttackList.light1:
                    activeAttack = StartCoroutine(light1());
                    break;
                case AttackList.light2:
                    activeAttack = StartCoroutine(light2());
                    break;
                case AttackList.heavy:
                    activeAttack = StartCoroutine(heavy());
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator light1() {
        //Call the animator
        yield return new WaitForSeconds(light_time.x);

        
        List<Enemy> collidedCreatures; //TODO Get the correct component
        List<Projectile> collidedBullets; //TODO Get the correct component
        DamageHitbox hitboxDamage = Instantiate(prefabsDamage[0]).GetComponent<DamageHitbox>();
        DamageHitbox hitboxDeflect = Instantiate(prefabsDeflect[0]).GetComponent<DamageHitbox>();
        int totalTime = 0;

        collidedCreatures = getEnemies(hitboxDamage.collidedObjects(enemyLayerMask));
        collidedBullets = getProjectiles(hitboxDamage.collidedObjects(projectileLayerMask));
        foreach(Enemy enemy in collidedCreatures) {
            //Deal damage
        }
        foreach(Projectile projectile in collidedBullets) {
            //Deal damage
        }
        comboCounter.updateComboCount(collidedCreatures.Count + collidedBullets.Count);
        yield return new WaitForEndOfFrame();



        while(totalTime >= light_time.y) {
            List<Enemy> tempEnemies = newEnemies(collidedCreatures, getEnemies(hitboxDamage.collidedObjects(enemyLayerMask)));
            List<Projectile> tempProjectiles = newProjectiles(collidedBullets, getProjectiles(hitboxDamage.collidedObjects(projectileLayerMask)));
            collidedCreatures.AddRange(tempEnemies);
            collidedBullets.AddRange(tempProjectiles);
            foreach(Enemy enemy in tempEnemies) {
                //Deal damage
            }
            foreach(Projectile projectile in tempProjectiles) {
                //Deal damage
            }
            comboCounter.updateComboCount(collidedCreatures.Count + collidedBullets.Count);
            yield return new WaitForEndOfFrame();
        }

        //Increase Combo Counter

    }

    IEnumerator light2() {
        //Call the animator
        yield return new WaitForSeconds(light_time.x);
        DamageHitbox hitboxDamage = Instantiate(prefabsDamage[1]).GetComponent<DamageHitbox>();
        GameObject hitboxDeflect = Instantiate(prefabsDeflect[1]);
        //Increase Combo Counter
    }

    IEnumerator heavy() {
        //Call the animator
        yield return new WaitForSeconds(heavy_time.x);
        DamageHitbox hitboxDamage = Instantiate(prefabsDamage[1]).GetComponent<DamageHitbox>();
        GameObject hitboxDeflect = Instantiate(prefabsDeflect[1]);
        //Reset Combo Counter
    }

    #region Helper Functions
    /// <summary>
    /// Stops any new attacks from starting
    /// </summary>
    /// <param name="waitTime">The duration preventing new attacks</param>
    /// <param name="cancelAttacks">Decides whether to stop currently active attack</param>
    /// <param name="clearQue">Decides whether to clear the attack que</param>
    public void LockAttack(float waitTime, bool cancelAttacks = true, bool clearQue = true) {
        if(clearQue) queClear();
        if(cancelAttacks) StopCoroutine(activeAttack);
        StartCoroutine(lockAttackCR(waitTime));
    }

    /// <summary>
    /// Takes a list of colliders and returns the associated enemy scripts
    /// </summary>
    /// <param name="colliders">The collider list</param>
    /// <returns>List of of the associated enemy scripts</returns>
    private List<Enemy> getEnemies(Collider2D[] colliders) {
        List<Enemy> enemies = new List<Enemy>();
        foreach(Collider2D c in colliders) {
            enemies.Add(c.gameObject.GetComponent<Enemy>());
        }
        return enemies;
    }

    /// <summary>
    /// Takes a list of colliders and returns the associated projectile scripts
    /// </summary>
    /// <param name="colliders">The collider list</param>
    /// <returns>List of of the associated projectile scripts</returns>
    private List<Projectile> getProjectiles(Collider2D[] colliders) {
        List<Projectile> projectiles = new List<Projectile>();
        foreach(Collider2D c in colliders) {
            projectiles.Add(c.gameObject.GetComponent<Projectile>());
        }
        return projectiles;
    }

    IEnumerator lockAttackCR(float waitTime) {
        attackLock = true;
        yield return new WaitForSeconds(waitTime);
        attackLock = false;
    }

    /// <summary>
    /// </summary>
    /// <param name="oldList">The old list</param>
    /// <param name="newList">The new items</param>
    /// <returns>Returns the enemies in the new list but not in the old list</returns>
    private List<Enemy> newEnemies(List<Enemy> oldList, List<Enemy> newList) {
        List<Enemy> temp = new List<Enemy>();
        bool inOldList;
        foreach(Enemy item in newList) {
            inOldList = false;
            foreach(Enemy oldItem in oldList) {
                if(item == oldItem) inOldList = true;
            }
            if(!inOldList) temp.Add(item);
        }
        return temp;
    }

    /// <summary>
    /// </summary>
    /// <param name="oldList">The old list</param>
    /// <param name="newList">The new items</param>
    /// <returns>Returns the projectiles in the new list but not in the old list</returns>
    private List<Projectile> newProjectiles(List<Projectile> oldList, List<Projectile> newList) {
        List<Projectile> temp = new List<Projectile>();
        bool inOldList;
        foreach(Projectile item in newList) {
            inOldList = false;
            foreach(Projectile oldItem in oldList) {
                if(item == oldItem) inOldList = true;
            }
            if(!inOldList) temp.Add(item);
        }
        return temp;
    }

    #region Que Management
    /// <summary>
    /// For cleaner code: Cycles through the attackQue, the que-ed attack to be next, and setting the que-ed attack to be null
    /// </summary>
    private void queCycle() {
        attackQue = new AttackList?[] { attackQue[1], null };
    }

    /// <summary>
    /// Clears the que, and sets the first attack to heavy if needed.
    /// </summary>
    /// <param name="heavy">Checks if first item needs to become heavy attack</param>
    private void queClear(bool heavy = false) {
        if(heavy) attackQue = new AttackList?[] { AttackList.heavy, null };
        else attackQue = new AttackList?[] { null, null };
    }

    /// <summary>
    /// Checks what attacks can be added to the que and returns the attacks that can be added and their position.
    /// </summary>
    /// <returns>
    /// i == position for added atk
    /// atk == null > non can be added
    /// atk == heavy > can add heavy only
    /// atk == light2 > light2/heavy only
    /// atk == light1 > light1/heavy only
    /// </returns>
    private (AttackList? atk, int i) queCheck() {
        if(attackQue[0] == AttackList.heavy) return (null, -1);
        if(attackQue[1] != null) return (AttackList.heavy, -1);
        if(attackQue[1] == null) {
            if(attackQue[0] == null) return (AttackList.light1, 0);
            if(attackQue[0] == AttackList.light1) return (AttackList.light2, 1);
            if(attackQue[0] == AttackList.light2) return (AttackList.light1, 1);
        }
        return (null, -1);
    }
    #endregion
    #endregion

    #region OnPress Calls
    public void OnLight(InputAction.CallbackContext context) {
        (AttackList? atk, int i) check = queCheck();
        if(check.atk == AttackList.light1) {
            attackQue[check.i] = AttackList.light1;
        }
        else if(check.atk == AttackList.light2) {
            attackQue[check.i] = AttackList.light2;
        }
        //Call the animator
    }

    public void OnHeavy(InputAction.CallbackContext context) {
        (AttackList? atk, int i) check = queCheck();
        if(check.atk != null) {
            queClear(true);
        }
        //Call the animator
    }
    #endregion
}

public enum AttackList {
    light1,
    light2,
    heavy
}
