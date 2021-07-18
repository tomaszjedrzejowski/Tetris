using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraminoSpawner : MonoBehaviour
{
    public Action<Tetramino> onTetraminoSpawn;
    public Action<object> onNextTetraminoSelect;

    [SerializeField] private TetraminoFactory tetraminoFactory;
    private List<object> randomizedPool = new List<object>();
    public void SelectActiveTetramino()
    {   
        var newtetramino = tetraminoFactory.GetTetramino((int)randomizedPool[0]);
        newtetramino.SetOnStartPosition();
        onTetraminoSpawn?.Invoke(newtetramino);
        randomizedPool.Remove(randomizedPool[0]);
        if(randomizedPool.Count <= 0)
        {
            randomizedPool = CreatePool();
            randomizedPool = RandomizePool(randomizedPool);
        }       
        var nextTetramino = randomizedPool[0];
        onNextTetraminoSelect?.Invoke(nextTetramino);
    }

    public void CreateRandomizedPool()
    {
        randomizedPool = CreatePool();
        randomizedPool = RandomizePool(randomizedPool);
    }

    public void Clear()
    {
        randomizedPool.Clear();
    }

    private List<object> CreatePool()
    {
        List<object> tetraminoPool = new List<object>();
        foreach (var item in Enum.GetValues(typeof(tetraminos)))
        {
            tetraminoPool.Add(item);
        }
        return tetraminoPool;
    }

    private List<object> RandomizePool(List<object> tetraminoPool)
    {
        for (int i = 0; i < tetraminoPool.Count; i++)
        {
            int random = UnityEngine.Random.Range(i, tetraminoPool.Count);
            var temp = tetraminoPool[i];
            tetraminoPool[i] = tetraminoPool[random];
            tetraminoPool[random] = temp;
        }
        return tetraminoPool;
    }

}
