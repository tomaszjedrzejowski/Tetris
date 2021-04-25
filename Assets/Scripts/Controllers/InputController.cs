using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;    
    [SerializeField] private TetraminoController tetraminoController;
    [SerializeField] private TetraminoSpawner spawner;
    [SerializeField] private List<PowerUpButton> powerUpsButtons;

    private void Start()
    {        
        playerInput.onMoveRightInput += HandleMoveRightInput;
        playerInput.onMoveLeftInput += HandleMoveLeftInput;
        playerInput.onMoveDownInput += HandleMoveDownInput;
        playerInput.onRotateInput += HandleRotateInput;
        playerInput.onPowerUpInput += HandlePowerUpInput;
        foreach (var button in powerUpsButtons)
        {
            button.onActivePowerUp += HandlePowerUpActivation;
        }
    }

    private void OnDisable()
    {
        playerInput.onMoveRightInput -= HandleMoveRightInput;
        playerInput.onMoveLeftInput -= HandleMoveLeftInput;
        playerInput.onMoveDownInput -= HandleMoveDownInput;
        playerInput.onRotateInput -= HandleRotateInput;
        playerInput.onPowerUpInput -= HandlePowerUpInput;
        foreach (var button in powerUpsButtons)
        {
            button.onActivePowerUp -= HandlePowerUpActivation;
        }
    }

    private void HandlePowerUpInput(int buttonIndex)
    {
        powerUpsButtons[buttonIndex].ActivatePowerUp();
    }

    private void HandlePowerUpActivation(int powerUpIndex)
    {
        spawner.AddPowerUpToPool(powerUpIndex);
    }

    private void HandleRotateInput()
    {
        tetraminoController.TryRotate();
    }

    private void HandleMoveDownInput()
    {
        tetraminoController.TryMove(Vector3.down);
    }

    private void HandleMoveLeftInput()
    {
        tetraminoController.TryMove(Vector3.left);
    }

    private void HandleMoveRightInput()
    {
        tetraminoController.TryMove(Vector3.right);
    }
}
