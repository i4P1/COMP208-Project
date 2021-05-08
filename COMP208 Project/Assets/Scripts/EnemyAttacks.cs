using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    public GameObject prefabDamage;
    public GameObject prefabDeflect;

    /// <summary>
    /// Layermask for the enemies
    /// </summary>
    public LayerMask playerLayerMask;
    /// <summary>
    /// Layermask for the projectiles
    /// </summary>
    public LayerMask projectileLayerMask;
    /// <summary>
    /// x == delay until damage starts || y == duration of damage
    /// </summary>
    public Vector2 attackTime;

    bool trackObject;

    Hitbox hitboxDamage;
    Hitbox hitboxDeflect;
    Coroutine attackdmg;

    private void FixedUpdate() {
        if(trackObject && hitboxDamage != null)
            hitboxDamage.setPosAuto(true); //Change to a proper direction
        if(trackObject && hitboxDeflect != null)
            hitboxDeflect.setPosAuto(true); //Change to a proper direction
    }

    public Coroutine attackStatus() {
        return attackdmg;
    }

    public Coroutine startAttack(float damage, float? xMult = null, bool track = true) {
        if(prefabDeflect != null) StartCoroutine(attackDeflect(xMult));
        attackdmg = StartCoroutine(attackDamage(damage, xMult));
        return attackdmg;
    }

    public void stopAttack(Coroutine atk) {
        if(atk == null) return;
        StopAllCoroutines();
        attackdmg = null;
        Destroy(hitboxDamage.gameObject);
        Destroy(hitboxDeflect.gameObject);
    }

    IEnumerator attackDamage(float damage, float? xMult = null) {
        //Call the animator
        yield return new WaitForSeconds(attackTime.x); // Wait specified time before starting the attack
        if(hitboxDamage != null) 
            Destroy(hitboxDamage.gameObject); //Destroy any pre-existing damage hitbox to start this attack.
        List<PlayerController> collidedCreatures;
        hitboxDamage = Instantiate(prefabDamage).GetComponent<Hitbox>();
        hitboxDamage.transform.parent = transform;
        hitboxDamage.setPosAuto(true);

        xMult ??= 1;
        hitboxDamage.transform.localScale = new Vector2((float)xMult * hitboxDamage.transform.localScale.x, hitboxDamage.transform.localScale.y);
        hitboxDamage.transform.localPosition = new Vector2((float)xMult * hitboxDamage.transform.localPosition.x, hitboxDamage.transform.localPosition.y);

        float totalTime = 0;

        collidedCreatures = getPlayers(hitboxDamage.collidedObjects(playerLayerMask));
        foreach(PlayerController pl in collidedCreatures) {
            Debug.Log(pl.gameObject);
            pl.Damage(damage);
        }
        yield return new WaitForEndOfFrame();

        while(totalTime <= attackTime.y) { //Check how long has passed
            totalTime += Time.deltaTime; //Increase the time for the while loop.
            //Debug.Log(Time.deltaTime + ":" + totalTime + ":" + light_time.y);
            List<PlayerController> tempEnemies = newPlayers(collidedCreatures, getPlayers(hitboxDamage.collidedObjects(playerLayerMask))); //Temparory list of the new un-checked enemies
            collidedCreatures.AddRange(tempEnemies); //Adding the new enemies to the total list to make sure no enemy is damaged twice.
            foreach(PlayerController pl in tempEnemies) {
                Debug.Log(pl.gameObject);
                pl.Damage(damage);
            }
            yield return new WaitForEndOfFrame(); //Wait for the end of the frame to act again.
        }
        attackdmg = null;
        Destroy(hitboxDamage.gameObject);
    }

    IEnumerator attackDeflect(float? xMult = null) {
        //Call the animator
        yield return new WaitForSeconds(attackTime.x); // Wait specified time before starting the attack
        if(hitboxDeflect != null) Destroy(hitboxDeflect.gameObject); //Destroy any pre-existing deflection hitbox to start this attack.
        List<Projectile> collidedBullets;
        hitboxDeflect = Instantiate(prefabDeflect).GetComponent<Hitbox>();
        hitboxDeflect.transform.parent = transform;
        hitboxDeflect.setPosAuto(true);

        hitboxDeflect.transform.localScale = new Vector2((float)xMult * hitboxDeflect.transform.localScale.x, hitboxDeflect.transform.localScale.y);
        hitboxDeflect.transform.localPosition = new Vector2((float)xMult * hitboxDeflect.transform.localPosition.x, hitboxDeflect.transform.localPosition.y);

        float totalTime = 0;
        
        collidedBullets = getProjectiles(hitboxDeflect.collidedObjects(projectileLayerMask));
        foreach(Projectile projectile in collidedBullets) {
            projectile.Deflect();
        }

        yield return new WaitForEndOfFrame();

        while(totalTime <= attackTime.y) { //Check how long has passed
            totalTime += Time.deltaTime; //Increase the time for the while loop.
            //Debug.Log(Time.deltaTime + ":" + totalTime + ":" + light_time.y);
            List<Projectile> tempProjectiles = newProjectiles(collidedBullets, getProjectiles(hitboxDamage.collidedObjects(projectileLayerMask))); //Temparory list of the new un-checked projectiles
            collidedBullets.AddRange(tempProjectiles); //Adding the new projectile to the total list to make sure no projectile is deflected twice.
            foreach(Projectile projectile in tempProjectiles) {
                projectile.Deflect();
            }
            yield return new WaitForEndOfFrame(); //Wait for the end of the frame to act again.
        }
        Destroy(hitboxDeflect.gameObject);
    }

    #region helper functions
    /// <summary>
    /// Set the prefab for damage, and the prefab for deflection if one exists.
    /// </summary>
    /// <param name="preDam">The damage prefab</param>
    /// <param name="preDef">The deflection prefab</param>
    public void setPrefabs(GameObject preDam, GameObject preDef = null) {
        prefabDamage = preDam;
        prefabDeflect = preDef;
    }

    #region setAttackTime
    /// <summary>
    /// Set the attack time to the vector given
    /// </summary>
    /// <param name="vec">x = damage delay || y = damage duration</param>
    public void setAttackTime(Vector2 vec) {
        attackTime = vec;
    }

    /// <summary>
    /// Set the attack time to the 2 floats
    /// </summary>
    /// <param name="x">damage delay</param>
    /// <param name="y">damage duration</param>
    public void setAttackTime(float x, float y) {
        attackTime = new Vector2(x, y);
    }

    /// <summary>
    /// Set the damage duration to the vector given, and the delay to 0.
    /// </summary>
    /// <param name="y">damage duration</param>
    public void setAttackTime(float y) {
        attackTime = new Vector2(0f, y);
    }

    #endregion

    /// <summary>
    /// Takes a list of colliders and returns the associated enemy scripts
    /// </summary>
    /// <param name="colliders">The collider list</param>
    /// <returns>List of of the associated enemy scripts</returns>
    private List<PlayerController> getPlayers(Collider2D[] colliders) {
        List<PlayerController> enemies = new List<PlayerController>();
        foreach(Collider2D c in colliders) {
            if(c.GetComponent<PlayerController>() != null) enemies.Add(c.GetComponent<PlayerController>());
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
            if(c.GetComponent<Projectile>() != null) projectiles.Add(c.GetComponent<Projectile>());
        }
        return projectiles;
    }

    /// <summary>
    /// </summary>
    /// <param name="oldList">The old list</param>
    /// <param name="newList">The new items</param>
    /// <returns>Returns the enemies in the new list but not in the old list</returns>
    private List<PlayerController> newPlayers(List<PlayerController> oldList, List<PlayerController> newList) {
        List<PlayerController> temp = new List<PlayerController>();
        bool inOldList;
        foreach(PlayerController item in newList) {
            inOldList = false;
            foreach(PlayerController oldItem in oldList) {
                if(item == oldItem) inOldList = true;
            }
            if(!inOldList && item != null) temp.Add(item);
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
    #endregion
}
