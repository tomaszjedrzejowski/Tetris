using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BombBlock : Block
{
    //public Action<Vector3> OnBombDestroyed;
    public Action<Vector3> OnBombDestroyed;
    
    public override void DestroyBlock()
    {
        OnBombDestroyed?.Invoke(transform.position);
        base.DestroyBlock();
    }
}
