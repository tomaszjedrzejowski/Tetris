using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Action<TetraminoMoveInstance> OnValidMove;
    public Action<TetraminoMoveInstance> OnInvalidMove;
    public Action<bool> OnLastRowReached;
    public Action OnLineCompleted;
    public Action OnLineCleanUpEnd;

    [SerializeField] private TetraminoController tetraminoController;
    [SerializeField] private PowerUpController powerUpController;

    private BoardGrid _grid;
    private Block[,] _gameGrid;    

    void Start()
    {
        _grid = new BoardGrid();
        _gameGrid = _grid.GetGameGrid();

        tetraminoController.OnTryMovement += ValidateMove;
        tetraminoController.OnTetraminoDestroyed += HandleDestroyedTetramino;
        powerUpController.OnDestroyBombTargets += HandleBombTargets;
    }

    private void OnDisable()
    {
        tetraminoController.OnTryMovement -= ValidateMove;
        tetraminoController.OnTetraminoDestroyed -= HandleDestroyedTetramino;
    }   
    private void HandleDestroyedTetramino(List<Block> tetraminosToGrid)
    {
        List<int> removedLine = new List<int>();
        foreach (var block in tetraminosToGrid)
        {
            _grid.AddToGrid(block);
            block.transform.parent = this.transform;
        }
        GridCleanUp();
        OnLastRowReached?.Invoke(ChcekHeighestLine());
    }    

    private void GridCleanUp()
    {
        var completedLines = LineCompletitionCheck();
        if (completedLines.Count != 0)
        {
            var line = completedLines[0];
            ClearLine(line);
            LowerUpperLines(line + 1);
            GridCleanUp();            
        }
        else return;
        OnLineCleanUpEnd?.Invoke();
    }

    private void HandleBombTargets(List<Vector3> bombTargets)
    {
        foreach (var vector in bombTargets)
        {
            if (Mathf.RoundToInt(vector.x) >= 0 && Mathf.RoundToInt(vector.x) <= 9 && Mathf.RoundToInt(vector.y) >= 0 && Mathf.RoundToInt(vector.y) <= 19)
            {
                if (_gameGrid[Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y)] != null)
                {
                    var block = _gameGrid[Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y)];
                    _grid.RemoveFromGrid(block);
                    block.DestroyBlock();
                }
            }
        }
    }

    private void ValidateMove(TetraminoMoveInstance tetraminoMoveInstance)
    {       
        bool isValid = false;
        foreach (var item in tetraminoMoveInstance.GetDesiredPositions())
        {
            if (!_grid.ChcekGridSlotAvailable(Mathf.RoundToInt(item.x), Mathf.RoundToInt(item.y)))
            {
                isValid = false;
                break;
            }
            else isValid = true;
        }
        if (isValid)
        {
            OnValidMove?.Invoke(tetraminoMoveInstance);
        }
        else OnInvalidMove?.Invoke(tetraminoMoveInstance);
    }
        // lines clean up and row adjustment //
    private List<int> LineCompletitionCheck()
    {        
        List<int> completedLines = new List<int>();
        bool isCompleted = false;
        for (int y = 0; y < _gameGrid.GetLength(1); y++)
        {
            for (int x = 0; x < _gameGrid.GetLength(0); x++)
            {
                if (_gameGrid[x, y] == null)
                {
                    isCompleted = false;
                    break;
                }
                else isCompleted = true;
            }
            if (isCompleted) completedLines.Add(y);
        }
        return completedLines;
    }

    private bool ChcekHeighestLine()
    {
        bool isTaken = false;
        for (int x = 0; x < _gameGrid.GetLength(0); x++)
        {
            if (_gameGrid[x, 19] == null)
            {
                isTaken = false;
            }
            else 
            { 
                isTaken = true; 
                break; 
            }
        }
        return isTaken;
    }

    private void LowerUpperLines(int lineHight)
    {
        for (int i = lineHight; i < _gameGrid.GetLength(1); i++)
        {
            for (int j = 0; j < _gameGrid.GetLength(0); j++)
            {
                var cell = _gameGrid[j, i];
                if (cell != null)
                {
                    _grid.RemoveFromGrid(cell);
                    cell.MoveBlock(cell.CalculateNewPosition(Vector3.down));
                    _grid.AddToGrid(cell);
                }
            }
        }

    }

    private void ClearLine(int lineHight)
    {
        for (int i = 0; i < _gameGrid.GetLength(0); i++)
        {            
            var block = _gameGrid[i, lineHight];
            _grid.RemoveFromGrid(block);
            block.DestroyBlock();
        }
        OnLineCompleted?.Invoke();
    }
}
