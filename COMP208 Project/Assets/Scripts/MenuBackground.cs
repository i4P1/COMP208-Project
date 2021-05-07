using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackground : MonoBehaviour
{
    [SerializeField]
    float bgMoveSpeed;
    public float alpha;
    [SerializeField]
    MenuBackground otherBg;
    [SerializeField]
    string bgLayer;

    private void Start() {
        foreach(SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>()) {
            var color = child.color;
            color.a = alpha;
            child.color = color;
            child.sortingLayerName = bgLayer;
            if(otherBg.alpha > alpha)
                child.sortingOrder = 1;
            else
                child.sortingOrder = 2;
        }
    }

    // Update is called once per frame
    void Update() {
        Vector2 newXY = Mathf.Sin(Time.time) * bgMoveSpeed * Vector2.one;
        transform.position = new Vector3(newXY.x, newXY.y, transform.position.z);
    }
}
