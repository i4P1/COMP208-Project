using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public PlayerController pc;
    public void OnLevelReset(InputAction.CallbackContext context) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
