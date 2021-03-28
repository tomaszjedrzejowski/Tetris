using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Action OnGameOver;
    public Action<int> OnLineCompleted;
    public Action<int> OnLevelChange;
    public Action OnFallTetramino;
    public Action OnGameReset;

    public bool IsGameLoopActive { get; private set; }    

    [SerializeField] private GridController gridController;
    [SerializeField] private TetraminoController tetraminoController;
    [SerializeField] private TetraminoSpawner tetraminoSpawner;
    [SerializeField] private UIController uIController;
    [SerializeField] private int lineThreshold;
    [SerializeField] private Timer timerPrefab;

    private Timer _fallTimer;
    private int _gameLevel = 0;
    private float _fallTime = 1f;
    private int _lineCompleted = 0;

    private void Awake()
    {
        SetFallTimer();
        uIController.OnStartClick += StartGameLoop;
        uIController.OnRestartClick += RestratGameLoop;
        gridController.OnLineCompleted += HandleLineCompleated;
        gridController.OnLastRowReached += HandleLastRowReached;
        IsGameLoopActive = false;
    }


    private void OnDisable()
    {
        uIController.OnStartClick -= StartGameLoop;
        uIController.OnRestartClick -= RestratGameLoop;
        gridController.OnLineCompleted -= HandleLineCompleated;
        gridController.OnLastRowReached -= HandleLastRowReached;
        _fallTimer.OnTimeOut += () => OnFallTetramino?.Invoke();
    }
    private void SetFallTimer()
    {
        _fallTimer = Instantiate(timerPrefab, this.transform);
        _fallTimer.CountDownTime = _fallTime;
        _fallTimer.IsContinuous = true;
        _fallTimer.OnTimeOut += () => OnFallTetramino?.Invoke();
    }

    private void StartGameLoop()
    {
        tetraminoController.StartTetraminoFlow();
        IsGameLoopActive = true;
        _fallTimer.SetActive(true);
    }

    private void RestratGameLoop()
    {
        IsGameLoopActive = false;
        _fallTimer.SetActive(false);
        tetraminoController.StopTetraminoFlow();
        tetraminoSpawner.ClearSpawner();
        gridController.ClearGrid();
        // Clear player Points;
        _lineCompleted = 0;
        _gameLevel = 0;
        OnGameReset?.Invoke();
        
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
        IsGameLoopActive = false;
        tetraminoController.StopTetraminoFlow();
        _fallTimer.SetActive(false);
        OnGameOver?.Invoke();
    }

    private void HandleLineCompleated()
    {
        _lineCompleted++;
        if(_lineCompleted % lineThreshold == 0)
        {
            RiseGameLevel();
        }
        OnLineCompleted?.Invoke(_lineCompleted);
    }

    private void RiseGameLevel()
    {
        _gameLevel++;
        _fallTime = _fallTime - 0.15f; //temp
        _fallTimer.CountDownTime = _fallTime;
        OnLevelChange?.Invoke(_gameLevel);
    }
}
