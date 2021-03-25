using System.Collections.Generic;
using UnityEngine;

public interface IRotate
{
    List<Vector3> CalculateRotation(int value);
    void UpdateRotationState();
}