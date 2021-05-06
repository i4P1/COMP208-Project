using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    GameObject Camera;
    [SerializeField]
    float bgMoveSpeed;
    public float alpha;
    [SerializeField]
    Background otherBg;
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
        transform.position = Camera.transform.position * bgMoveSpeed;
    }
}
