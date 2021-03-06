using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraminoController : MonoBehaviour
{
    public Action<TetraminoMoveInstance> onTryMovement;
    public Action<List<Block>> onSettleDownTetramino;

    private Tetramino _activeTetramino;
    private int _rotationAttempt = 0;
    private bool _isActive = false;

    public void TryRotate()
    {
        try
        {            
            if (!_isActive) return;
            if (_activeTetramino is IRotate)
            {               
                var positionsToCheck = ((IRotate)_activeTetramino).CalculateRotation(_rotationAttempt);
                TetraminoMoveInstance tetraminoMoveInstance = new TetraminoMoveInstance(positionsToCheck, true);
                onTryMovement?.Invoke(tetraminoMoveInstance);
            }
        }
        catch(NullReferenceException)
        {            
            return;
        }
    }
    public void TryMove(Vector3 direction)
    {
        try
        {
            if (!_isActive) return;
            List<Vector3> positionsToCheck = new List<Vector3>();
            positionsToCheck = _activeTetramino.CalculateMove(direction);
            TetraminoMoveInstance tetraminoMoveInstance = new TetraminoMoveInstance(positionsToCheck, direction);
            onTryMovement?.Invoke(tetraminoMoveInstance);
        }
        catch(NullReferenceException)
        {
            return;
        }
    }

    public void StopTetraminoFlow()
    {
        _isActive = false;
        if (_activeTetramino != null)
        {
            _activeTetramino.DestroyTetramino();
            _activeTetramino = null;
        }
    }

    public void SetActive(bool isActive)
    {
        _isActive = isActive;
    }

    public void HandleGameOver()
    {
        _isActive = false;
    }

    public void TetraminoFall()
    {
        if (!_isActive && _activeTetramino != null) return;
        TryMove(Vector3.down);
    }

    public void HandleNewTetramino(Tetramino newTetramino)
    {
        _activeTetramino = newTetramino;
        _activeTetramino.SetOnStartPosition();        
    }

    public void HandleValidMove(TetraminoMoveInstance tetraminoMoveInstance)
    {
        _activeTetramino.MoveTetramino(tetraminoMoveInstance.GetDesiredPositions());
        if (tetraminoMoveInstance.GetIsRotation() && _activeTetramino is IRotate)
        {
            _rotationAttempt = 0;
            ((IRotate)_activeTetramino).UpdateRotationState();
        }        
    }

    public void HandleInvalidMove(TetraminoMoveInstance tetraminoMoveInstance)
    {
        if (tetraminoMoveInstance.GetIsRotation() && _rotationAttempt < 4)
        {
            _rotationAttempt++;
            TryRotate();
        }
        else if (!tetraminoMoveInstance.GetIsRotation())
        {
            Vector3 direction = tetraminoMoveInstance.GetMoveDirection();
            if (direction == Vector3.down) SettleDownTetramino();
            else return;
        }
    }

    private void SettleDownTetramino()
    {
        List<Block> blocksToGrid = _activeTetramino.GetBlocks();
        _activeTetramino.DestroyTetramino();
        _activeTetramino = null;
        onSettleDownTetramino?.Invoke(blocksToGrid);
    }    
}

public class TetraminoMoveInstance
{    
    private readonly List<Vector3> _desiredPositions;
    private readonly Vector3 _moveDirection;
    private readonly bool _isRotation;   
    public bool IsReachedHeighestRow { get; set; }

    public TetraminoMoveInstance(List<Vector3> positions, Vector3 direction)
    {
        this._desiredPositions = positions;
        this._moveDirection = direction;        
    }

    public TetraminoMoveInstance(List<Vector3> positions, bool isRotation)
    {
        this._desiredPositions = positions;
        this._isRotation = isRotation;
    }

    public Vector3 GetMoveDirection()
    {
        return _moveDirection;
    }
    public List<Vector3> GetDesiredPositions()
    {
        return _desiredPositions;
    }
    public bool GetIsRotation() 
    {
        return _isRotation; 
    }
}