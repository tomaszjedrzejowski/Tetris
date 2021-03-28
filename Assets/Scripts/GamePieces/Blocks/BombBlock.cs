using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BombBlock : Block
{    
    public Action<Vector3, int> OnBombDestroyed;

    [SerializeField] private int bombRange = 1;
    
    public override void DestroyBlock()
    {
        OnBombDestroyed?.Invoke(transform.position, bombRange);
        base.DestroyBlock();
    }
}
