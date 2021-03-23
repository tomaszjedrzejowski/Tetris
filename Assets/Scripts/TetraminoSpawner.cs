using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraminoSpawner : MonoBehaviour
{
    public Action<Tetramino> OnTetraminoSpawn;
    
    [SerializeField] private List<Tetramino> tetraminoPool = new List<Tetramino>();
    [SerializeField] private List<PowerUp> powerUpPool = new List<PowerUp>();
    [SerializeField] private Transform nextPieceNest;
    private List<Tetramino> _randomizelPool;
    private Tetramino currentTetramino;
    private Tetramino nextTetramino;
    
    public void SpawnTetramino()
    {
        try
        {
            if (_randomizelPool.Count <= 0) CreateRandomPool();
            if (nextTetramino != null) currentTetramino = nextTetramino;
            else currentTetramino = Instantiate(_randomizelPool[0]);
            nextTetramino = Instantiate(_randomizelPool[1], nextPieceNest.position, Quaternion.identity);
            _randomizelPool.Remove(_randomizelPool[0]);
            OnTetraminoSpawn?.Invoke(currentTetramino);
        }
        catch
        {
            CreateRandomPool();
            SpawnTetramino();
        }
    }

    public void CreateRandomPool()
    {
        _randomizelPool = new List<Tetramino>(tetraminoPool);
        for (int i = 0; i < _randomizelPool.Count; i++)
        {
            int random = UnityEngine.Random.Range(i, _randomizelPool.Count);
            Tetramino temp = _randomizelPool[i];
            _randomizelPool[i] = _randomizelPool[random];
            _randomizelPool[random] = temp;
        }
    }

    public void AddPowerUpToPool( int powerUpIndex) // redu
    {
        return; // avoid bugg
        _randomizelPool.Insert(0, powerUpPool[powerUpIndex]);
        nextTetramino = _randomizelPool[0];
    }
}