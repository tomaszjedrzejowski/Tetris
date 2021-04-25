using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PowerUpController : MonoBehaviour
{
    public Action<List<Vector3>> onBombTrigger;
    public Action<Block> onFillerTrigger;

    private List<Vector3> _bombCosualties = new List<Vector3>();
    private BombBlock _bombBlock;
    private Block _fillerBlock;

    public void RegisterPowerUp(Tetramino newTetramino)
    {
        if (newTetramino is PowerUp)
        {
            var powerUpBlock = newTetramino.GetBlocks()[0];
            if (powerUpBlock is BombBlock)
            {
                _bombBlock = (BombBlock)powerUpBlock;
                _bombBlock.onBombDestroyed += FindBombTargets;
            }
            else if (powerUpBlock is FillerBlock)
            {               
                _fillerBlock = (FillerBlock)powerUpBlock;
            }
        }
    }

    public void ContinueFillerMove()
    {
        if(_fillerBlock != null)
        {
            onFillerTrigger?.Invoke(_fillerBlock);
            _fillerBlock = null;
        } 
    }

    private void FindBombTargets(Vector3 position, int range)
    {
        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                Vector3 coordinatesToCheck = new Vector3(position.x + x, position.y + y);
                _bombCosualties.Add(coordinatesToCheck);
            }
        }
    }

    public void DestroyBombTargets()
    {
        if (_bombCosualties.Count <= 0) return;
        onBombTrigger?.Invoke(_bombCosualties);
        _bombCosualties.Clear();
        _bombBlock.onBombDestroyed -= FindBombTargets;
    }
}