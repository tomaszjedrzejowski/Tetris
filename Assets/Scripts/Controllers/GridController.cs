using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Action<TetraminoMoveInstance> OnValidMove;
    public Action<TetraminoMoveInstance> OnInvalidMove;
    public Action OnLineCompleted;
    public Action<bool> OnLastRowReached;

    [SerializeField] private TetraminoController tetraminoController;

    private BoardGrid _grid;
    private Block[,] _gameGrid;
    private List<Vector3> _bombCosualties = new List<Vector3>();

    void Start()
    {
        _grid = new BoardGrid();
        _gameGrid = _grid.GetGameGrid();

        tetraminoController.OnTryMovement += ValidateMove;
        tetraminoController.OnTetraminoDestroyed += HandleDestroyedTetramino;

        BombBlock.OnBombDestroy += HandleBombPowerUp; // TODO static event - refctor
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

    private void HandleBombPowerUp(Vector3 bombPosition)
    {
        int bombRange = 1;
        for (int x = -bombRange; x <= bombRange; x++)
        {
            for (int y = -bombRange; y <= bombRange; y++)
            {
                Vector3 coordinatesToCheck = new Vector3(bombPosition.x + x, bombPosition.y + y);
                _bombCosualties.Add(coordinatesToCheck);
                Debug.Log(coordinatesToCheck);
            }
        }
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
        foreach (var item in _bombCosualties)
        {
            if (Mathf.RoundToInt(item.x) >= 0 && Mathf.RoundToInt(item.x) <= 9 && Mathf.RoundToInt(item.y) >= 0 && Mathf.RoundToInt(item.y) <= 19)
            {
                if (_gameGrid[Mathf.RoundToInt(item.x), Mathf.RoundToInt(item.y)] != null)
                {
                    var block = _gameGrid[Mathf.RoundToInt(item.x), Mathf.RoundToInt(item.y)];
                    _grid.RemoveFromGrid(block);
                    block.DestroyBlock();
                    Debug.Log("Destroing!");
                }
            }

        }
        _bombCosualties.Clear();
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
