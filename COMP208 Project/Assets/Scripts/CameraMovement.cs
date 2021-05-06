using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObj;
    Rigidbody2D rb;
    public float cameraSpeed;
    public Vector2 offset = new Vector2(0, 5);
    public float plOffset;
    private Vector2 plOffsetVec;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(playerObj.transform.position.x, playerObj.transform.position.y, transform.position.z);
    }

    private void FixedUpdate() {
        Transform player = playerObj.transform;
        Vector2 cameraMovement = player.position - transform.position;
        Vector2 cmSq = cameraMovement * cameraMovement.magnitude + offset + plOffsetVec;
        rb.velocity = cmSq * cameraSpeed;
    }

    void Update() {
        /*
        if(Input.GetKey(KeyCode.W)) {
            plOffsetVec = new Vector2(0, plOffset);
        }
        else if(Input.GetKey(KeyCode.A)) {
            plOffsetVec = new Vector2(-plOffset, 0);
        }
        else if(Input.GetKey(KeyCode.S)) {
            plOffsetVec = new Vector2(0, -plOffset);
        }
        else if(Input.GetKey(KeyCode.D)) {
            plOffsetVec = new Vector2(plOffset, 0);
        }
        else {
            plOffsetVec = new Vector2(0, 0);
        }
        */
    }
}
