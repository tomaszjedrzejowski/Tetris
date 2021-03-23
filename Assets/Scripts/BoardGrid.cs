using UnityEngine;
public class BoardGrid
{
    private readonly int _gridWidth = 10;
    private readonly int _gridHight = 22;

    private readonly Block[,] _gameGrid;     

    public BoardGrid()
    {
        _gameGrid = new Block[_gridWidth, _gridHight];
    }

    public void AddToGrid(Block block)
    {
        try
        {
            _gameGrid[block.WidthPosition, block.HeightPosition] = block;
        }
        catch (System.IndexOutOfRangeException)
        {
            return;
        }
    }

    public void RemoveFromGrid(Block block)
    {
        try
        {            
            _gameGrid[block.WidthPosition, block.HeightPosition] = null;
        }
        catch (System.IndexOutOfRangeException)
        {            
            return;
        }
    }

    public bool ChcekGridSlotAvailable(int width, int hight)
    {
        if (width < 0 || width >= _gridWidth || hight < 0 || hight >= _gridHight)
        {
            return false;
        }
        else if (_gameGrid[width, hight] == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Block[,] GetGameGrid()
    {
        return _gameGrid;
    }

    public void DrowDebugGrid()
    {
        Vector3 offset = new Vector3(0.5f, 0.5f, 0);
        for (int x = 0; x < _gameGrid.GetLength(0); x++)
        {
            for (int y = 0; y < _gameGrid.GetLength(1); y++)
            {
                
                Debug.DrawLine(new Vector3(x,y,0) - offset, new Vector3(x, y + 1, 0) - offset, Color.white, 100f);
                Debug.DrawLine(new Vector3(x,y,0) - offset, new Vector3(x + 1, y, 0) - offset, Color.white, 100f);
            }
        }
            Debug.DrawLine(new Vector3(0, _gridHight, 0) - offset, new Vector3(_gridWidth, _gridHight, 0) - offset, Color.white, 100f);
            Debug.DrawLine(new Vector3(_gridWidth, 0, 0) - offset, new Vector3(_gridWidth, _gridHight, 0) - offset, Color.white, 100f);
    }    
}
