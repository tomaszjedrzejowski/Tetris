using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BombBlock : Block
{    
    public Action<Vector3, int> onBombDestroyed;

    [SerializeField] private int bombRange = 1;
    
    public override void DestroyBlock()
    {
        onBombDestroyed?.Invoke(transform.position, bombRange);
        base.DestroyBlock();
    }
}
