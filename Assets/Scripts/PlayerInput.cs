using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Action onMoveLeftInput;
    public Action onMoveRightInput;
    public Action onMoveDownInput;
    public Action onRotateInput;
    public Action<int> onPowerUpInput;

    private readonly KeyCode moveRightMainKey = KeyCode.D;
    private readonly KeyCode moveRightAlternativeKey = KeyCode.RightArrow;
    private readonly KeyCode moveLeftMainKey = KeyCode.A;
    private readonly KeyCode moveLeftAlternativeKey = KeyCode.LeftArrow;
    private readonly KeyCode moveDownMainKey = KeyCode.S;
    private readonly KeyCode moveDownAlternativeKey = KeyCode.DownArrow;
    private readonly KeyCode rotateMainKey = KeyCode.Space;
    private readonly KeyCode rotateAlternativeKey = KeyCode.RightControl;
    private readonly KeyCode powerUp1MainKey = KeyCode.Alpha1;
    private readonly KeyCode powerUp2MainKey = KeyCode.Alpha2;

    private void Update()
    {
        PlayerInputHandle();
    }

    private void PlayerInputHandle()
    {
        if(Input.GetKeyDown(moveRightMainKey) || Input.GetKeyDown(moveRightAlternativeKey))
        {
            onMoveRightInput?.Invoke();
        }
        else if (Input.GetKeyDown(moveLeftMainKey) || Input.GetKeyDown(moveLeftAlternativeKey))
        {
            onMoveLeftInput?.Invoke();
        }
        else if(Input.GetKeyDown(moveDownMainKey) || Input.GetKeyDown(moveDownAlternativeKey))
        {
            onMoveDownInput?.Invoke();
        }
        else if(Input.GetKeyDown(rotateMainKey) || Input.GetKeyDown(rotateAlternativeKey))
        {
            onRotateInput?.Invoke();
        }
        else if (Input.GetKeyDown(powerUp1MainKey))
        {
            onPowerUpInput?.Invoke(0);
        }
        else if (Input.GetKeyDown(powerUp2MainKey))
        {
            onPowerUpInput?.Invoke(1);
        }
    }
}
