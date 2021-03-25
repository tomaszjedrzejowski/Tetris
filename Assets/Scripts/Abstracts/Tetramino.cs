using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Tetramino : MonoBehaviour
{
    
    public int TetraminoID { get; private set; }   
    
    [SerializeField] private int tetraminoID;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private List<Block> myBlocks = new List<Block>();
       
    

    public virtual void Start()
    {
        TetraminoID = tetraminoID;
    }
    public void SetOnStartPosition()
    {
        transform.position = spawnPosition;
    }    

    public virtual List<Vector3> CalculateMove(Vector3 direction)
    {
        List<Vector3> positionsToCheck = new List<Vector3>();
        foreach (var item in GetBlocks())
        {
            Vector3 desiredPosition = item.CalculateNewPosition(direction);
            positionsToCheck.Add(desiredPosition);
        }
        return positionsToCheck;
    }

    public void MoveTetramino(List<Vector3> destinations)
    {
        foreach (var block in myBlocks)
        {            
            block.MoveBlock(destinations[myBlocks.IndexOf(block)]);
        }        
    }

    public virtual List<Block> GetBlocks()
    {
        return myBlocks;
    }

    public void DestroyTetramino()
    {
        Destroy(this.gameObject);
    }
}
