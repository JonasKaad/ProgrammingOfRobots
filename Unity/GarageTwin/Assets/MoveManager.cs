using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    private MoveScript[] _movingScripts;
    private GearRotation[] _gearScripts;

    private void Start()
    {
        _movingScripts = (MoveScript[])FindObjectsOfType(typeof(MoveScript));
        _gearScripts = (GearRotation[])FindObjectsOfType(typeof(GearRotation));
    }

    public void MoveObjectsByPercentage(float percentage)
    {
        foreach (var moveScript in _movingScripts)
        {
            moveScript.MoveToPercentage(percentage);
        }

        foreach (var gearScript in _gearScripts)
        {
            StartCoroutine(gearScript.RotateGear());
        }
    }
}
