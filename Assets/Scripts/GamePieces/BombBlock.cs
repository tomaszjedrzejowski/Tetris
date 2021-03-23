using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBlock : Block
{
    public static System.Action<Vector3> OnBombDestroy;
    public override void DestroyBlock()
    {
        OnBombDestroy(transform.position);
        base.DestroyBlock();
    }
}
