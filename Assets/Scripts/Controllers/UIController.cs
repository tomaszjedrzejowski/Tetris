using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Action OnStartClick;
    public Action OnRestartClick;

    [SerializeField] private Button startButton;
    [SerializeField] private GameController gameController;
    [SerializeField] private TetraminoSpawner spawner;
    [SerializeField] private TextDisplay levelDisplay;
    [SerializeField] private TextDisplay linesDisplay;
    [SerializeField] private SpriteDisplay spriteDisplay;
    [SerializeField] private PopUp gameOverPopUp;
    [SerializeField] private string startButtonText = "Start";
    [SerializeField] private string restartButtonText = "Restart";
    private Text _startButtonTextComponnent;

    private void Start()
    {
        gameController.OnLineCompleted += HandleLineCompleted;
        gameController.OnLevelChange += HandleLevelChange;
        gameController.OnGameReset += HandleGameReset;
        gameController.OnGameOver += HandleGameOver;
        spawner.OnNextTetraminoSelect += HandleTetraminoListUpdate;
        _startButtonTextComponnent = startButton.GetComponentInChildren<Text>();
        SetPopUpActive(gameOverPopUp, false);
    }


    private void OnDisable()
    {
        gameController.OnLineCompleted -= HandleLineCompleted;
        gameController.OnLevelChange -= HandleLevelChange; 
        spawner.OnNextTetraminoSelect -= HandleTetraminoListUpdate;
    }
    private void HandleGameOver()
    {
        SetPopUpActive(gameOverPopUp, true);
    }

    private void HandleGameReset()
    {
        SetPopUpActive(gameOverPopUp, false);
    }

    private void HandleLineCompleted(int value)
    {
        linesDisplay.UpdateDisplay(value);
    }

    private void HandleLevelChange(int value)
    {
        levelDisplay.UpdateDisplay(value);
    }

    private void HandleTetraminoListUpdate(Tetramino tetramino)
    {
        Sprite sprite = tetramino.GetSprite();
        spriteDisplay.UpdateDisplay(sprite);
    }

    public void OnStartButtonClick()
    {
        if (gameController.IsGameLoopActive == false)
        {
            OnStartClick?.Invoke();            
            _startButtonTextComponnent.text = restartButtonText;
        }
        else if (gameController.IsGameLoopActive == true)
        {
            OnRestartClick?.Invoke();
            _startButtonTextComponnent.text  = startButtonText;        
        }
    }

    private void SetPopUpActive(PopUp popUp, bool isActive)
    {
        popUp.gameObject.SetActive(isActive);
    }
}
