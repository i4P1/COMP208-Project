using UnityEngine;

public class ProjectileRicochet : Projectile {
    [SerializeField]
    private int ricochetsRemaining {get; set;}

    private void OnWallHit() {
        if (ricochetsRemaining > 0) {
            Deflect();
        } else {
            Die();
        }
    }
}
