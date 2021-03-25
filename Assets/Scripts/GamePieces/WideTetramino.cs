using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideTetramino : Tetramino, IRotate
{
    public int RotationStateID { get; private set; }
    public Block PivotBlock { get; private set; }
    [SerializeField] private Block pivotBlock;
    private enum rotationState { spwan = 0, rotationRight90 = 1, rotationRight180 = 2, rotationLeft90 = 3 };
    private rotationState _rotationState;

    private readonly Vector3[,] _kickTests = new Vector3[5, 4] { { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, },
                                                                        {new Vector3(-2, 0), new Vector3(-1, 0), new Vector3(2, 0), new Vector3(1, 0) },
                                                                        {new Vector3(1, 0), new Vector3(2, 0), new Vector3(-1, 0), new Vector3(-2, 0) },
                                                                        {new Vector3(-2, -1), new Vector3(-1, 2), new Vector3(2, 1), new Vector3(1, -2) },
                                                                        {new Vector3(1, 2), new Vector3(2, -1), new Vector3(-1, -2), new Vector3(-2, 1) } };

    public override void Start()
    {
        _rotationState = rotationState.spwan;
        RotationStateID = (int)_rotationState;
        PivotBlock = pivotBlock;
        base.Start();
    }
    public List<Vector3> CalculateRotation(int testNumber)
    {
        int rotationState = RotationStateID;
        List<Vector3> positionsToCheck = new List<Vector3>();


        foreach (var item in GetBlocks())
        {
            Vector3 desiredPosition = item.CalculateClockwiseRotationPosition(PivotBlock.transform);
            desiredPosition = ApplyRegulationKick(desiredPosition, rotationState);
            desiredPosition = ApplyWallKick(desiredPosition, rotationState, testNumber);
            positionsToCheck.Add(desiredPosition);
        }
        return positionsToCheck;
    }
    public void UpdateRotationState()
    {
        if (RotationStateID == 0) _rotationState = rotationState.rotationRight90;
        else if (RotationStateID == 1) _rotationState = rotationState.rotationRight180;
        else if (RotationStateID == 2) _rotationState = rotationState.rotationLeft90;
        else if (RotationStateID == 3) _rotationState = rotationState.spwan;
        RotationStateID = (int)_rotationState;
    }

    private Vector3 ApplyRegulationKick(Vector3 desiredPosition, int rotationID)
    {
        Vector3 kickedPosition = desiredPosition;
        if (rotationID == 0)
        {
            var kick = Vector3.right;
            kickedPosition = desiredPosition + kick;
        }
        else if (rotationID == 1)
        {
            var kick = Vector3.down;
            kickedPosition = desiredPosition + kick;
        }
        else if (rotationID == 2)
        {
            var kick = Vector3.left;
            kickedPosition = desiredPosition + kick;
        }
        else if (rotationID == 3)
        {
            var kick = Vector3.up;
            kickedPosition = desiredPosition + kick;
        }
        return kickedPosition;
    }
    public Vector3 ApplyWallKick(Vector3 desiredPosition, int rotationID, int testNumber)
    {
        int _rotationAttempt = testNumber;
        Vector3 wallKickedPosition;
        Vector3 kick = _kickTests[_rotationAttempt, rotationID];
        if (_rotationAttempt > 4)
        {
            Debug.Log("attempt to rotate more then 4 times!");
            return desiredPosition;
        }
        wallKickedPosition = desiredPosition + kick;
        return wallKickedPosition;
    }
}
