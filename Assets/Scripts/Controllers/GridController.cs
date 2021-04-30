using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Action<bool> onLastRowReached;
    public Action onLineCompleted;
    public Action onGridCleanUpEnds;
    public Action<TetraminoMoveInstance> onValidMovement;
    public Action<TetraminoMoveInstance> onInvalidMovement;

    private BoardGrid _grid;
    private Block[,] _gameGrid;    

    void Start()
    {
        _grid = new BoardGrid();
        _gameGrid = _grid.GetGameGrid();
    }

    public void HandleSettleDownTetramino(List<Block> tetraminosToGrid)
    {
        List<int> removedLine = new List<int>();
        foreach (var block in tetraminosToGrid)
        {
            _grid.AddToGrid(block);
            block.transform.parent = this.transform;
        }
        GridCleanUp();
        onLastRowReached?.Invoke(ChcekHeighestLine());
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
        onGridCleanUpEnds?.Invoke();
    }

    public void HandleBombTrigger(List<Vector3> bombTargets)
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

    public void HandleFillerTrigger(Block filler)
    {
        _grid.RemoveFromGrid(filler);
        for (int y = filler.HeightPosition; y >= 0; y--)
        {
            if (_gameGrid[filler.WidthPosition, y] == null) 
            {
                filler.MoveBlock(new Vector3(filler.WidthPosition, y));
            }
        }
        _grid.AddToGrid(filler);
        // fix - not :P
        GridCleanUp();
        onLastRowReached?.Invoke(ChcekHeighestLine());
    }

    public void ValidateMove(TetraminoMoveInstance tetraminoMoveInstance)
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
            onValidMovement?.Invoke(tetraminoMoveInstance);
        }
        else onInvalidMovement?.Invoke(tetraminoMoveInstance);
    }
     
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
        onLineCompleted?.Invoke();
    }

    public void ClearGrid()
    {
        foreach (var item in _gameGrid)
        {
            if (item != null)
            {
                _grid.RemoveFromGrid(item);
                item.DestroyBlock();
            }
        }
    }
}
