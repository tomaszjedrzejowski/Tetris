using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameLoopManager gameLoopManager;
    [SerializeField] private UIController uIController;
    [SerializeField] private Timer timerPrefab;
    [SerializeField] private int lineThreshold;

    private Timer _fallTimer;
    private int _gameLevel = 0;
    private float _fallTime = 1f;
    private int _lineCompleted = 0;

    private void Awake()
    {
        SetFallTimer();
        uIController.onStartClick += OnStartClick;
        uIController.onRestartClick += OnRestratClick;
        gameLoopManager.onLineCompleted += OnLineCompleated;
        gameLoopManager.onLastRowReached += OnLastRowReached;
        uIController.SetGameloopActiveFlag(false); 
    }

    private void OnDisable()
    {
        uIController.onStartClick -= OnStartClick;
        uIController.onRestartClick -= OnRestratClick;
        gameLoopManager.onLineCompleted -= OnLineCompleated;
        gameLoopManager.onLastRowReached -= OnLastRowReached;
        _fallTimer.onTimeOut -= gameLoopManager.TetraminoFall;
    }
    private void SetFallTimer()
    {
        _fallTimer = Instantiate(timerPrefab, this.transform);
        _fallTimer.CountDownTime = _fallTime;
        _fallTimer.IsContinuous = true;
        _fallTimer.onTimeOut += gameLoopManager.TetraminoFall;
    }

    private void OnStartClick()
    {
        gameLoopManager.StartGame();
        uIController.SetGameloopActiveFlag(true);
        _fallTimer.SetActive(true);
    }

    private void OnRestratClick()
    {
        _fallTimer.SetActive(false);
        gameLoopManager.RestartGame();
        uIController.SetGameloopActiveFlag(false);
        // Clear player Points;
        _lineCompleted = 0;
        _gameLevel = 0;
        uIController.HandleGameReset();
        
    }

    private void OnLastRowReached()
    {
        GameOver();
    }

    private void GameOver()
    {
        _fallTimer.SetActive(false);
        uIController.HandleGameOver();
    }

    private void OnLineCompleated()
    {
        _lineCompleted++;
        if(_lineCompleted % lineThreshold == 0)
        {
            RiseGameLevel();
        }
        uIController.HandleLineCompleted(_lineCompleted);
    }

    private void RiseGameLevel()
    {
        _gameLevel++;
        _fallTime -= 0.15f;
        _fallTimer.CountDownTime = _fallTime;
        uIController.HandleLevelChange(_gameLevel);
    }
}