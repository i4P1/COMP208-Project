using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour {
    public Transform target;
    public float nextWaypointDistance = 3;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    private Rigidbody2D rb;
    private IMoveable controller;
    [SerializeField]
    private float aggroRange = 14;

    private void Start() {
        seeker     = GetComponent<Seeker>();
        rb         = GetComponent<Rigidbody2D>();
        controller = GetComponent<IMoveable>();

        // Update the path periodically
        InvokeRepeating("UpdatePath", 0, 0.5f);
    }

    // Generate a path to the target with a coroutine
    private void UpdatePath() {
        if (seeker.IsDone()) {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    // Update the path once it's generated
    private void OnPathComplete(Path newPath) {
        if (!newPath.error) {
            path            = newPath;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate() {
        if (path != null) {
            // Check if we've reached the end of the path and stop execution if we have
            if (currentWaypoint >= path.vectorPath.Count) {
                reachedEndOfPath = true;
                return;
            } else {
                reachedEndOfPath = false;
            }

            if (path.GetTotalLength() < aggroRange) {
                // Calculate the direction to the current waypoint and move in it
                Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
                controller.Move(direction);

                // Check if we're close enough to the waypoint to move onto the next one
                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                if (distance < nextWaypointDistance) {
                    currentWaypoint++;
                }
            }
        }
    }
}
