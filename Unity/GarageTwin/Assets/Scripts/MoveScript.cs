using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    [SerializeField] private Transform[] Positions;
    private int _posIndex = 0;

    private Transform currentTransform;
    // Start is called before the first frame update
    void Start()
    {
        currentTransform = gameObject.transform;
        SetPositionAndRotation(_posIndex);
        for (int i = 0; i < Positions.Length; i++)
        {
            Positions[i] = Positions[i].transform;
        }
    }

    private void SetPositionAndRotation(int index)
    {
        var transformReference = Positions[index];
        currentTransform.position = transformReference.position;
        currentTransform.rotation = transformReference.rotation;
        _posIndex = index;
    }

    public void MoveToNextPosition()
    {
        var nextPosIndex = _posIndex + 1;
        if (nextPosIndex >= Positions.Length)
        {
            return;
        } 
        SetPositionAndRotation(nextPosIndex);
    }

    public void MoveToPrevPosition()
    {
        var nextPosIndex = _posIndex - 1;
        if (nextPosIndex < 0)
        {
            return;
        }
        SetPositionAndRotation(nextPosIndex);

    }

    public void MoveToPercentage(float percentage)
    {
        var nextPosIndex = (int)Math.Round((Positions.Length-1) * percentage);
        SetPositionAndRotation(nextPosIndex);
    }
}
