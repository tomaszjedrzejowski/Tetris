using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int WidthPosition { get; set; }
    public int HeightPosition { get; set; }

    public Vector3 CalculateClockwiseRotationPosition(Transform pivotPoint)
    {
        Vector3 localPosition = transform.position - pivotPoint.transform.position;
        Vector3 rotationVector = new Vector3(localPosition.y, -localPosition.x, localPosition.z);
        Vector3 newPositionPoint = rotationVector + pivotPoint.transform.position;
        return newPositionPoint;
    }

    public Vector3 CalculateCounterClockwiseRotationPosition(Transform pivotPoint)
    {
        Vector3 localPos = transform.position - pivotPoint.transform.position;
        Vector3 rotationVector = new Vector3(-localPos.y, localPos.x, localPos.z);
        Vector3 newPosVector = rotationVector + pivotPoint.transform.position;
        return newPosVector;
    }

    public Vector3 CalculateNewPosition(Vector3 direction)
    {
        Vector3 newPosition = transform.position + direction;
        return newPosition;
    }
    public void MoveBlock(Vector3 newPosition)
    {
        transform.position = newPosition;
        WidthPosition = Mathf.RoundToInt(newPosition.x);
        HeightPosition = Mathf.RoundToInt(newPosition.y);
    }

    public virtual void DestroyBlock()
    {
        Destroy(this.gameObject);
    }
}