using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearRotation : MonoBehaviour
{
    [SerializeField] private float rotationAmount = 50.0f;
    [SerializeField] [Range(-1, 1)]
    private int rotationDirection = 1;

    public IEnumerator RotateGear()
    {
        var eulerAngles = transform.localEulerAngles;
        var appliedRotation = rotationAmount * rotationDirection;
        var newRotation = eulerAngles.z + appliedRotation;
        newRotation = newRotation < 0 ? newRotation + 360 : newRotation % 360;
        var rotation = new Vector3(eulerAngles.x, eulerAngles.y, newRotation);
        while(Math.Abs(transform.localEulerAngles.z - Math.Abs(rotation.z)) > 1){
            transform.Rotate(new Vector3(0,0, appliedRotation) * Time.deltaTime);
            yield return null;
        }
    }
}
