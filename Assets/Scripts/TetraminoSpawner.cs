using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraminoSpawner : MonoBehaviour
{
    public Action<Tetramino> onTetraminoSpawn;
    public Action<Tetramino> onNextTetraminoSelect;    

    [SerializeField] private List<Tetramino> tetraminoPool = new List<Tetramino>();
    [SerializeField] private List<PowerUp> powerUpPool = new List<PowerUp>();    

    private List<Tetramino> _randomizedPool = new List<Tetramino>();
    private Tetramino currentTetramino;
    private Tetramino _nextTetramino;

    public void SelectActiveTetramino()
    {
        if (_randomizedPool.Count <= 1) CreatePool();        
        currentTetramino = _randomizedPool[0];
        _nextTetramino = _randomizedPool[1];
        _randomizedPool.Remove(_randomizedPool[0]);
        onNextTetraminoSelect?.Invoke(_nextTetramino);
        onTetraminoSpawn?.Invoke(currentTetramino);
    }

    public void RandomizePool()
    {
        if (_randomizedPool.Count <= 0) return;
        for (int i = 0; i < _randomizedPool.Count; i++)
        {
            int random = UnityEngine.Random.Range(i, _randomizedPool.Count);
            Tetramino temp = _randomizedPool[i];
            _randomizedPool[i] = _randomizedPool[random];
            _randomizedPool[random] = temp;            
        }
    }

    public void CreatePool()
    {        
        foreach (var tetramino in tetraminoPool)
        {
            var newTetramino = Instantiate(tetramino);
            _randomizedPool.Add(newTetramino);
        }
    }

    public void AddPowerUpToPool( int powerUpIndex) 
    {
        var newPowerUp = Instantiate(powerUpPool[powerUpIndex]);
        _randomizedPool.Insert(0,newPowerUp); 
        onNextTetraminoSelect?.Invoke(newPowerUp);
    }

    public void ClearSpawner()
    {
        foreach (var tetramino in _randomizedPool)
        {
            tetramino.DestroyTetramino();
        }
        _randomizedPool.Clear();
    }
}
