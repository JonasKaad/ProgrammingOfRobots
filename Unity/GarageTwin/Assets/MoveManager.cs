using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    private MoveScript[] _movingScripts;

    private void Start()
    {
        _movingScripts = (MoveScript[])FindObjectsOfType(typeof(MoveScript));
    }

    public void MoveObjectsByPercentage(float percentage)
    {
        foreach (var moveScript in _movingScripts)
        {
            moveScript.MoveToPercentage(percentage);
        }
    }
}
