using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BombBlock : Block
{
    //public Action<Vector3> OnBombDestroyed;
    public Action OnBombDestroyed;
    public override void DestroyBlock()
    {
        //OnBombDestroyed(transform.position);
        OnBombDestroyed?.Invoke();
        base.DestroyBlock();
    }
}
