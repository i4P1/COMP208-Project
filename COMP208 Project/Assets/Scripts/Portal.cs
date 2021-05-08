using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {
    [SerializeField]
    private LayerMask playerLayerMask;

    private void OnTriggerEnter2D(Collider2D other) {
        if (Mathf.Pow(2, other.gameObject.layer) == playerLayerMask.value) {
            if (SceneManager.GetActiveScene().buildIndex == 2) {
                SceneManager.LoadScene(0);
            } else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
