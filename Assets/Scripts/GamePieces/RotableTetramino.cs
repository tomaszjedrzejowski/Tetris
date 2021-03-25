using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotableTetramino : Tetramino, IRotate
{
    public int RotationStateID { get; private set; }
    public Block PivotBlock { get; private set; }
    [SerializeField] private Block pivotBlock;
    private enum rotationState { spwan = 0, rotationRight90 = 1, rotationRight180 = 2, rotationLeft90 = 3 };
    private rotationState _rotationState;

    private readonly Vector3[,] kickTests = new Vector3[5, 4] { { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, },
                                                                {new Vector3(-1, 0), new Vector3(1, 0), new Vector3(1, 0), new Vector3(-1, 0) },
                                                                {new Vector3(-1, 1), new Vector3(1, -1), new Vector3(1, 1), new Vector3(-1, -1) },
                                                                {new Vector3(0, -2), new Vector3(0, 2), new Vector3(0, -2), new Vector3(0, 2) },
                                                                {new Vector3(-1, -2), new Vector3(1, 2), new Vector3(1, -2), new Vector3(-1, 2) } };

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
        int tetraminoID = TetraminoID;
        List<Vector3> positionsToCheck = new List<Vector3>();

        foreach (var item in GetBlocks())
        {
            Vector3 desiredPosition = item.CalculateClockwiseRotationPosition(PivotBlock.transform);            
            desiredPosition = ApplyWallKick(desiredPosition, tetraminoID, rotationState, testNumber);
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

    public Vector3 ApplyWallKick(Vector3 desiredPosition, int tetraminoID, int rotationID, int testNumber)
    {
        int _rotationAttempt = testNumber;
        Vector3 wallKickedPosition;
        Vector3 kick = kickTests[_rotationAttempt, rotationID];
        if (_rotationAttempt > 4)
        {
            Debug.Log("attempt to rotate more then 4 times!");
            return desiredPosition;
        }
        wallKickedPosition = desiredPosition + kick;
        return wallKickedPosition;
    }
}
