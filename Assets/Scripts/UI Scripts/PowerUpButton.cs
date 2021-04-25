using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpButton : MonoBehaviour
{
    public Action<int> onActivePowerUp;

    [SerializeField] private int powerUpIndex;
    [SerializeField] private int powerUpCooldown;
    [SerializeField] private Timer timerPrefab;
    private Timer _timer;
    private bool _isAvailable;


    private void Start()
    {
        _timer = Instantiate(timerPrefab, this.transform);        
        _timer.onTimeOut += GrantPowerUp;
        _timer.CountDownTime = powerUpCooldown;
        _isAvailable = true;
    }

    private void OnDisable()
    {
        _timer.onTimeOut -= GrantPowerUp;
    }
    public void ActivatePowerUp()
    {
        if (!_isAvailable) return;
        onActivePowerUp?.Invoke(powerUpIndex);
        SetOnCooldown();
    }

    private void SetOnCooldown()
    {
        _timer.SetActive(true);
        _isAvailable = false;
    }

    private void GrantPowerUp()
    {
        _isAvailable = true;
    }
}   
