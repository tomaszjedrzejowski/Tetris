using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpButton : MonoBehaviour
{
    public Action<int> OnActivePowerUp;

    [SerializeField] private int powerUpIndex;
    [SerializeField] private int powerUpCooldown;
    [SerializeField] private Timer timerPrefab;
    private Timer _timer;
    private bool _isAvailable;


    private void Start()
    {
        _timer = Instantiate(timerPrefab, this.transform);        
        _timer.OnTimeOut += GrantPowerUp;
        _timer.CountDownTime = powerUpCooldown;
        _isAvailable = true;
    }

    private void OnDisable()
    {
        _timer.OnTimeOut -= GrantPowerUp;
    }
    public void ActivatePowerUp()
    {
        if (!_isAvailable) return;
        OnActivePowerUp?.Invoke(powerUpIndex);
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
