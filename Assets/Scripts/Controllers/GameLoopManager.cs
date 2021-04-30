using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public Action onLineCompleted;
    public Action onLastRowReached;

    [SerializeField] private GridController gridController;
    [SerializeField] private TetraminoController tetraminoController;
    [SerializeField] private PowerUpController powerUpController;
    [SerializeField] private TetraminoSpawner tetraminoSpawner;

    private void Start()
    {
        gridController.onLastRowReached += OnLastRowReached;
        gridController.onLineCompleted += OnLineCompleted;
        gridController.onValidMovement += OnValidMovement;
        gridController.onInvalidMovement += OnInvalidMovement;
        tetraminoController.onSettleDownTetramino += OnSettleDownTetramino;
        tetraminoController.onTryMovement += OnTryMovement;
        powerUpController.onBombTrigger += OnBombTrigger;
        powerUpController.onFillerTrigger += OnFillerTrigger;
        tetraminoSpawner.onTetraminoSpawn += OnTetraminoSpawn;
    }


    private void OnDisable()
    {
        gridController.onLastRowReached -= OnLastRowReached;
        gridController.onLineCompleted -= OnLineCompleted;
        gridController.onValidMovement -= OnValidMovement;
        gridController.onInvalidMovement -= OnInvalidMovement;
        tetraminoController.onSettleDownTetramino -= OnSettleDownTetramino;
        tetraminoController.onTryMovement -= OnTryMovement;
        powerUpController.onBombTrigger -= OnBombTrigger;
        powerUpController.onFillerTrigger -= OnFillerTrigger;
        tetraminoSpawner.onTetraminoSpawn -= OnTetraminoSpawn;
    }

    public void StartGame()
    {
        tetraminoSpawner.CreatePool();
        tetraminoSpawner.RandomizePool();
        tetraminoSpawner.SelectActiveTetramino();
        tetraminoController.SetActive(true);
    }
    public void GameOver()
    {
        tetraminoController.StopTetraminoFlow();
        tetraminoController.HandleGameOver();
    }
    public void RestartGame()
    {
        tetraminoController.StopTetraminoFlow();
        tetraminoSpawner.ClearSpawner();
        gridController.ClearGrid();
    }

    public void TetraminoFall()
    {
        tetraminoController.TetraminoFall();
    }


    private void OnInvalidMovement(TetraminoMoveInstance tetraminoMoveInstance)
    {
        tetraminoController.HandleInvalidMove(tetraminoMoveInstance);
    }

    private void OnValidMovement(TetraminoMoveInstance tetraminoMoveInstance)
    {
        tetraminoController.HandleValidMove(tetraminoMoveInstance);
    }

    private void OnTetraminoSpawn(Tetramino tetramino)
    {
        tetraminoController.HandleNewTetramino(tetramino);
        powerUpController.RegisterPowerUp(tetramino);
    }

    private void OnFillerTrigger(Block block)
    {
        gridController.HandleFillerTrigger(block);
    }

    private void OnBombTrigger(List<Vector3> bombTargets)
    {
        gridController.HandleBombTrigger(bombTargets);
    }

    private void OnTryMovement(TetraminoMoveInstance tetraminoMoveInstance)
    {
        gridController.ValidateMove(tetraminoMoveInstance);
    }

    private void OnSettleDownTetramino(List<Block> tetraminoBlocks)
    {        
        gridController.HandleSettleDownTetramino(tetraminoBlocks);
    }

    private void OnLineCompleted()
    {
        onLineCompleted?.Invoke();
        powerUpController.DestroyBombTargets();
    }

    private void OnLastRowReached(bool isReached)
    {
        if(isReached == true)
        {            
            onLastRowReached?.Invoke();
            tetraminoSpawner.ClearSpawner();
            GameOver();
        }
        else
        {
            powerUpController.ContinueFillerMove();           
            tetraminoSpawner.SelectActiveTetramino();
        }
    }
}