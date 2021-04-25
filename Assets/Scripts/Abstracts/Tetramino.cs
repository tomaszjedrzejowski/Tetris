using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Tetramino : MonoBehaviour
{
    [SerializeField] private Sprite tetraminoIcon;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private List<Block> myBlocks = new List<Block>();    

    public virtual void Awake()
    {
        ShowBlocks(false);
    }

    private void ShowBlocks(bool isEnabled)
    {       
        foreach (var block  in myBlocks)
        {
            var blockRenderer = block.GetComponent<SpriteRenderer>();
            blockRenderer.enabled = isEnabled;
        }
    }

    public void SetOnStartPosition()
    {
        transform.position = spawnPosition;
        ShowBlocks(true);
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

    public Sprite GetSprite()
    {
        return tetraminoIcon;
    }

    public void DestroyTetramino()
    {
        Destroy(this.gameObject);
    }
}
