using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Hitbox : MonoBehaviour {
    /// <summary>
    /// The position relative to the player, automatically checks for player facing and handles that.
    /// </summary>
    [SerializeField]
    Vector2 pos;
    /// <summary>
    /// This is the player
    /// </summary>
    public GameObject pl;
    /// <summary>
    /// The direction of the player
    /// </summary>
    private bool playerDir = true;
    Collider2D hitBoxCollider;

    /// <summary>
    /// Will be used to automatically set the position of the hitbox
    /// </summary>
    /// <param name="playerDir"> The player, given so that we can find it's direction</param>
    public void setPosAuto(bool playerDir) {
        if(playerDir) transform.localPosition = pos;
        else transform.localPosition = pos * -1;
    }

    private void FixedUpdate() {
        setPosAuto(playerDir);
    }

    public Collider2D[] collidedObjects(LayerMask layer) {
        hitBoxCollider = GetComponent<Collider2D>();
        Collider2D[] collidedObjects = new Collider2D[128];
        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = layer;
        int collidedObjectsCount = hitBoxCollider.OverlapCollider(filter, collidedObjects);
        collidedObjects = collidedObjects.Take(collidedObjectsCount).ToArray();
        List<Collider2D> temp = new List<Collider2D>();
        for(int i = 0; i < collidedObjects.Length; i++) {
            Collider2D col = collidedObjects[i];
            if(col != null) temp.Add(col);
        }
        return temp.ToArray();
    }
}
