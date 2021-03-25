using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PowerUpController : MonoBehaviour
{
    public Action<List<Vector3>> OnDestroyBombTargets;

    [SerializeField] private GridController gridController;
    [SerializeField] private TetraminoSpawner tetraminoSpawner;

    private List<Vector3> _bombCosualties = new List<Vector3>();
    private BombBlock _bombBlock;
    private Block _fillBlock;

    private void Start()
    {
        tetraminoSpawner.OnTetraminoSpawn += RegisterPowerUp;
        gridController.OnLineCleanUpEnd += DestroyBombTargets;
    }

    private void OnDisable()
    {
        _bombBlock.OnBombDestroyed -= FindBombTargets; 
    }

    private void RegisterPowerUp(Tetramino newTetramino)
    {
        if (newTetramino is PowerUp)
        {
            if(newTetramino.GetBlocks()[0] is BombBlock)
            _bombBlock = (BombBlock)newTetramino.GetBlocks()[0];
            _bombBlock.OnBombDestroyed += FindBombTargets;
        }
    }

    private void FindBombTargets(Vector3 bombPosition)
    {
        Debug.Log("finding bomb targets");
        int bombRange = 1;
        for (int x = -bombRange; x <= bombRange; x++)
        {
            for (int y = -bombRange; y <= bombRange; y++)
            {
                Vector3 coordinatesToCheck = new Vector3(bombPosition.x + x, bombPosition.y + y);
                _bombCosualties.Add(coordinatesToCheck);
            }
        }
    }

    private void DestroyBombTargets()
    {
        if (_bombCosualties.Count <= 0) return;
        OnDestroyBombTargets?.Invoke(_bombCosualties);
        _bombCosualties.Clear();        
    }
}