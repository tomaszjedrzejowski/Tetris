using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTester : MonoBehaviour
{    
    private BoardGrid grid;
    [SerializeField] private Block blockPrefab;
    [SerializeField] private List<Vector2> testBlocksPositions;
    private List<Block> testBlocks = new List<Block>();

    // Start is called before the first frame update
    void Start()
    {
        grid = new BoardGrid();
        CreateTestBlocks();
        AddTest();
    }    

    private void CreateTestBlocks()
    {
        foreach (var item in testBlocksPositions)
        {
            Block testBlock = Instantiate(blockPrefab, this.transform);
            (testBlock.WidthPosition, testBlock.HeightPosition) = (Mathf.RoundToInt(item.x), Mathf.RoundToInt(item.y));
            testBlocks.Add(testBlock);
        }
    }

    private void AddTest()
    {
        foreach (var item in testBlocks)
        {
            Debug.Log("-------- " + testBlocks.IndexOf(item) + " -------");
            Debug.Log("Slot is Available: " + grid.ChcekGridSlotAvailable(item.WidthPosition, item.HeightPosition));
            grid.AddToGrid(item);
            Debug.Log("Slot is Available: " + grid.ChcekGridSlotAvailable(item.WidthPosition, item.HeightPosition));
        }
    }
}
