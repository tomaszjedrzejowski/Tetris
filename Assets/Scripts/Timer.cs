using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Action onTimeOut;

    public float CountDownTime { get; set; }
    public bool IsContinuous { get; set; }
    private float _currentTime;
    private bool _isActive;

    private void Start()
    {
        SetTimer();
        SetActive(false);
    }

    private void Update()
    {
        if (_isActive) CountDown();
    }

    private void CountDown()
    {
        _currentTime -= Time.deltaTime;
        if (_currentTime <= 0)
        {
            onTimeOut?.Invoke();
            SetTimer();
            if(!IsContinuous) SetActive(false);
        }
    }

    private void SetTimer()
    {
        _currentTime = CountDownTime;
    }

    public void SetActive(bool isActive)
    {
        this._isActive = isActive;
    }
}
