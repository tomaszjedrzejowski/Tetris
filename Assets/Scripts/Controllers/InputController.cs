using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;    
    [SerializeField] private TetraminoController tetraminoController;
    [SerializeField] private TetraminoSpawner spawner;

    private void Start()
    {        
        playerInput.OnMoveRightInput += HandleMoveRightInput;
        playerInput.OnMoveLeftInput += HandleMoveLeftInput;
        playerInput.OnMoveDownInput += HandleMoveDownInput;
        playerInput.OnRotateInput += HandleRotateInput;
        playerInput.OnPowerUpInput += HandlePowerUpInput;
    }

    private void OnDisable()
    {
        playerInput.OnMoveRightInput -= HandleMoveRightInput;
        playerInput.OnMoveLeftInput -= HandleMoveLeftInput;
        playerInput.OnMoveDownInput -= HandleMoveDownInput;
        playerInput.OnRotateInput -= HandleRotateInput;
        playerInput.OnPowerUpInput -= HandlePowerUpInput;
    }

    private void HandlePowerUpInput(int powerUpIndex)
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
