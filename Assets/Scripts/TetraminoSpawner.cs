using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraminoSpawner : MonoBehaviour
{
    public Action<Tetramino> OnTetraminoSpawn;
    public Action<PowerUp> OnPowerUpAdd;
    
    [SerializeField] private List<Tetramino> tetraminoPool = new List<Tetramino>();
    [SerializeField] private List<PowerUp> powerUpPool = new List<PowerUp>();

    private List<Tetramino> _randomizelPool;
    private Tetramino currentTetramino;
    private Tetramino nextTetramino;
    
    public void SpawnTetramino()
    {
        if (_randomizelPool.Count <= 0) CreateRandomPool();
        //if (nextTetramino != null) currentTetramino = nextTetramino;
        //else 
        currentTetramino = Instantiate(_randomizelPool[0]);
        _randomizelPool.Remove(_randomizelPool[0]);
        OnTetraminoSpawn?.Invoke(currentTetramino);

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

    public void AddPowerUpToPool( int powerUpIndex) 
    {        
        _randomizelPool.Insert(0, powerUpPool[powerUpIndex]);
    }
}