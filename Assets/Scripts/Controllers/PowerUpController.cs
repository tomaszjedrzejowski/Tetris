using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PowerUpController : MonoBehaviour
{
    public Action<List<Vector3>> OnBombTrigger;
    public Action<Block> OnFillerTrigger;

    [SerializeField] private GridController gridController;
    [SerializeField] private TetraminoSpawner tetraminoSpawner;
    [SerializeField] private TetraminoController tetraminoController;

    private List<Vector3> _bombCosualties = new List<Vector3>();
    private BombBlock _bombBlock;
    private Block _fillerBlock;

    private void Start()
    {
        tetraminoSpawner.OnTetraminoSpawn += RegisterPowerUp;
        tetraminoController.OnSettleDownTetramino += ContinueFillerMove;
        gridController.OnLineCleanUpEnd += DestroyBombTargets;
    }

    private void RegisterPowerUp(Tetramino newTetramino)
    {
        if (newTetramino is PowerUp)
        {
            var powerUpBlock = newTetramino.GetBlocks()[0];
            if (powerUpBlock is BombBlock)
            {
                _bombBlock = (BombBlock)powerUpBlock;
                _bombBlock.OnBombDestroyed += FindBombTargets;
            }
            else if (powerUpBlock is FillerBlock)
            {               
                _fillerBlock = (FillerBlock)powerUpBlock;
            }
        }
    }

    private void ContinueFillerMove(List<Block> blocks)
    {
        if(_fillerBlock != null)
        {
            OnFillerTrigger?.Invoke(_fillerBlock);
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

    private void DestroyBombTargets()
    {
        if (_bombCosualties.Count <= 0) return;
        OnBombTrigger?.Invoke(_bombCosualties);
        _bombCosualties.Clear();
        _bombBlock.OnBombDestroyed -= FindBombTargets;
    }
}