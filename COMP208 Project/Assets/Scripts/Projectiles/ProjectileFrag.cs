using UnityEngine;

public class ProjectileFrag : Projectile {
    [SerializeField]
    private int numFragments = 5;
    [SerializeField]
    private int splitsRemaining {get; set;} = 1;
    [SerializeField]
    private int spread = 45;

    protected override void OnWallHit() {
        Deflect();

        if (splitsRemaining > 0) {
            splitsRemaining--;
            rb.position += direction * 2;

            for (float angle = -spread/2; angle < spread/2; angle += spread / numFragments) {
                ProjectileFrag newFragment = Instantiate(gameObject).GetComponent<ProjectileFrag>();

                newFragment.direction       = (direction + Vector2FromAngle(angle)).normalized;
                newFragment.splitsRemaining = splitsRemaining;
            }
        }

        base.OnWallHit();
    }

    private Vector2 Vector2FromAngle(float angle) {
        angle *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}
