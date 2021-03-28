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
        playerInput.OnMoveRightInput += HandleMoveRightInput;
        playerInput.OnMoveLeftInput += HandleMoveLeftInput;
        playerInput.OnMoveDownInput += HandleMoveDownInput;
        playerInput.OnRotateInput += HandleRotateInput;
        playerInput.OnPowerUpInput += HandlePowerUpInput;
        foreach (var button in powerUpsButtons)
        {
            button.OnActivePowerUp += HandlePowerUpActivation;
        }
    }

    private void OnDisable()
    {
        playerInput.OnMoveRightInput -= HandleMoveRightInput;
        playerInput.OnMoveLeftInput -= HandleMoveLeftInput;
        playerInput.OnMoveDownInput -= HandleMoveDownInput;
        playerInput.OnRotateInput -= HandleRotateInput;
        playerInput.OnPowerUpInput -= HandlePowerUpInput;
        foreach (var button in powerUpsButtons)
        {
            button.OnActivePowerUp -= HandlePowerUpActivation;
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
