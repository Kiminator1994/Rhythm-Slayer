using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomInputActions : MonoBehaviour
{
    public InputActionReference pauseButton;


    void Start()
    {
        pauseButton.action.started += PauseGame;
    }

    private void PauseGame(InputAction.CallbackContext context)
    {
        GameManager.Instance.PauseGame();
    }

}
