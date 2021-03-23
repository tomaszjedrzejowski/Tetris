using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Action OnTimeOut;
    public Action OnGameOver;
    public Action<int> OnLineCompleted;
    public Action<int> OnDifficultyChange;

    [SerializeField] private GridController gridController;
    [SerializeField] private TetraminoController tetraminoController;    
    [SerializeField] private UIController uIController;
    [SerializeField] private int lineThreshold;

    private int _gameDifficulty = 0;
    private float _gameSpeed = 1f;
    private float _currentTime;
    private int _lineCompleted = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        _currentTime = _gameSpeed;

        gridController.OnLineCompleted += HandleLineCompleated;
        gridController.OnLastRowReached += HandleNextTurn;
    }


    void Start()
    {
        //StartGameLoop();        
    }

    public void StartGameLoop()
    {
        tetraminoController.StartTetraminoGame();
        OnTimeOut?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        FallTimer();
    }

    private void OnDisable()
    {
        gridController.OnLineCompleted -= HandleLineCompleated;
    }

    private void HandleNextTurn(bool topRowReached)
    {        
        if (topRowReached == false)
        {
            return;
        }
        else
        {            
            OnGameOver?.Invoke();
            Debug.Log("Game Over");
            // show Ui to restart;
        }
    }
    
    private void HandleLineCompleated()
    {
        _lineCompleted++;
        if(_lineCompleted % lineThreshold == 0)
        {
            RiseGameDifficulty();
        }
        OnLineCompleted?.Invoke(_lineCompleted);
    }

    private void RiseGameDifficulty()
    {
        _gameDifficulty++;
        _gameSpeed = _gameSpeed - 0.15f; //temp
        OnDifficultyChange?.Invoke(_gameDifficulty);
    }

    private void FallTimer()
    {
        _currentTime -= Time.deltaTime;
        if (_currentTime <= 0)
        {
            OnTimeOut?.Invoke();
            _currentTime = _gameSpeed;
        }
    }
}
