using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private TextDisplay levelDisplay;
    [SerializeField] private TextDisplay linesDisplay;

    private void Start()
    {
        gameController.OnLineCompleted += HandleLineCompleted;
        gameController.OnDifficultyChange += HandleDifficultyChange;
    }

    private void OnDisable()
    {
        gameController.OnLineCompleted -= HandleLineCompleted;
        gameController.OnDifficultyChange -= HandleDifficultyChange;
    }

    private void HandleLineCompleted(int value)
    {
        linesDisplay.UpdateDisplay(value);
    }

    private void HandleDifficultyChange(int value)
    {
        levelDisplay.UpdateDisplay(value);
    }
}
