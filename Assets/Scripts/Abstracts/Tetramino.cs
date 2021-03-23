using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Tetramino : MonoBehaviour, IRotate<List<Vector3>>
{

    public Block PivotBlock { get; private set;}
    public int TetraminoID { get; private set; }   
    public int RotationStateID { get; private set; }
    [SerializeField] private int tetraminoID;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private Block blockPrefab;
    [SerializeField] private Block pivotBlock;
    [SerializeField] private List<Block> myBlocks = new List<Block>();
       
    private enum rotationState { spwan = 0, rotationRight90 = 1, rotationRight180 = 2, rotationLeft90 = 3};
    private rotationState _rotationState;

    public virtual void Start()
    {
        _rotationState = rotationState.spwan;
        RotationStateID = (int)_rotationState;

        TetraminoID = tetraminoID;
        PivotBlock = pivotBlock;
    }
    public virtual List<Vector3> CalculateRotation(int value)
    {
        List<Vector3> positionsToCheck = new List<Vector3>();

        foreach (var item in GetTetraminoBlocks())
        {
            Vector3 desiredPosition = item.transform.position;
            positionsToCheck.Add(desiredPosition);
        }
        return positionsToCheck;
    }

    public virtual List<Vector3> CalculateMove(Vector3 direction)
    {
        List<Vector3> positionsToCheck = new List<Vector3>();
        foreach (var item in GetTetraminoBlocks())
        {
            Vector3 desiredPosition = item.CalculateNewPosition(direction);
            positionsToCheck.Add(desiredPosition);
        }
        return positionsToCheck;
    }
    
    public void SetOnStartPosition()
    {
        transform.position = spawnPosition;
    }

    public void UpdateRotationState()
    {
        if (RotationStateID == 0) _rotationState = rotationState.rotationRight90;
        else if (RotationStateID == 1) _rotationState = rotationState.rotationRight180;
        else if (RotationStateID == 2) _rotationState = rotationState.rotationLeft90;
        else if (RotationStateID == 3) _rotationState = rotationState.spwan;
        RotationStateID = (int)_rotationState;
    }   

    public void MoveTetramino(List<Vector3> destinations)
    {
        foreach (var block in myBlocks)
        {            
            block.MoveBlock(destinations[myBlocks.IndexOf(block)]);
        }        
    }

    public List<Block> GetTetraminoBlocks()
    {
        return myBlocks;
    }

    public void DestroyTetramino()
    {
        Destroy(this.gameObject);
    }
}
