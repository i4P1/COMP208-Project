using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DamageHitbox : MonoBehaviour {
    /// <summary>
    /// The position relative to the player, automatically checks for player facing and handles that.
    /// </summary>
    [SerializeField]
    Vector2 pos;
    Collider2D hitBoxCollider;


    private void Start() {
        hitBoxCollider = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Will be used to automatically set the position of the hitbox
    /// </summary>
    /// <param name="playerDir"> The player, given so that we can find it's direction</param>
    public void setPosAuto(bool playerDir) {
        if(playerDir) transform.localPosition = pos;
        else transform.localPosition = pos * -1;
    }

    public Collider2D[] collidedObjects(LayerMask layer) {
        Collider2D[] collidedObjects = new Collider2D[128];
        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = layer;
        int collidedObjectsCount = hitBoxCollider.OverlapCollider(filter, collidedObjects);
        return collidedObjects.Take(collidedObjectsCount).ToArray();
    }
}
