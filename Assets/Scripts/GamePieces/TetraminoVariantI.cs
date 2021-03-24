using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraminoVariantI : Tetramino
{
    private readonly Vector3[,] _kickTests = new Vector3[5, 4] { { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, },
                                                                        {new Vector3(-2, 0), new Vector3(-1, 0), new Vector3(2, 0), new Vector3(1, 0) },
                                                                        {new Vector3(1, 0), new Vector3(2, 0), new Vector3(-1, 0), new Vector3(-2, 0) },
                                                                        {new Vector3(-2, -1), new Vector3(-1, 2), new Vector3(2, 1), new Vector3(1, -2) },
                                                                        {new Vector3(1, 2), new Vector3(2, -1), new Vector3(-1, -2), new Vector3(-2, 1) } };
       
    public override List<Vector3> CalculateRotation(int testNumber)
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
