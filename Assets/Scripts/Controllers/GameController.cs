using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GridController gridController;
    [SerializeField] private TetraminoController tetraminoController;
    [SerializeField] private TetraminoSpawner tetraminoSpawner;
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
        uIController.onStartClick += StartGameLoop;
        uIController.onRestartClick += RestratGameLoop;
        gridController.onLineCompleted += HandleLineCompleated;
        gridController.onLastRowReached += HandleLastRowReached;
        uIController.SetGameloopActiveFlag(false); 
    }

    private void OnDisable()
    {
        uIController.onStartClick -= StartGameLoop;
        uIController.onRestartClick -= RestratGameLoop;
        gridController.onLineCompleted -= HandleLineCompleated;
        gridController.onLastRowReached -= HandleLastRowReached;
        _fallTimer.OnTimeOut -= tetraminoController.TetraminoFall;
    }
    private void SetFallTimer()
    {
        _fallTimer = Instantiate(timerPrefab, this.transform);
        _fallTimer.CountDownTime = _fallTime;
        _fallTimer.IsContinuous = true;
        _fallTimer.OnTimeOut += tetraminoController.TetraminoFall;
    }

    private void StartGameLoop()
    {
        tetraminoController.StartTetraminoFlow();
        uIController.SetGameloopActiveFlag(true);
        _fallTimer.SetActive(true);
    }

    private void RestratGameLoop()
    {
        uIController.SetGameloopActiveFlag(false);
        _fallTimer.SetActive(false);
        tetraminoController.StopTetraminoFlow();
        tetraminoSpawner.ClearSpawner();
        gridController.ClearGrid();
        // Clear player Points;
        _lineCompleted = 0;
        _gameLevel = 0;
        uIController.HandleGameReset();
        
    }

    private void HandleLastRowReached(bool reachedEnd)
    {
        if (reachedEnd)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        uIController.SetGameloopActiveFlag(false);
        tetraminoController.StopTetraminoFlow();
        _fallTimer.SetActive(false);
        uIController.HandleGameOver();
        tetraminoController.HandleGameOver();
    }

    private void HandleLineCompleated()
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
        _fallTime -= 0.15f; //temp
        _fallTimer.CountDownTime = _fallTime;
        uIController.HandleLevelChange(_gameLevel);
    }
}
