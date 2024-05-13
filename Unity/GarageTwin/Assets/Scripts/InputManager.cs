using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private MoveScript[] movingScripts;
    private GearRotation[] _gearScripts;
    private InputAction forwardAction;
    private InputAction backwardAction;
    private InputAction switchCamAction;
    private InputAction percentAction;
    [SerializeField] [Range(0.0f, 1.0f)] private float percentage = 0.0f;
    [SerializeField] private Camera[] _cameras;
    private int _currentCamIndex = 0;
    private void Start()
    {
        for (int i = 1; i < _cameras.Length; i++)
        {
            _cameras[i].enabled = false;
        }
        forwardAction = InputSystem.ListEnabledActions().Find(action => action.name == "Forward");
        backwardAction = InputSystem.ListEnabledActions().Find(action => action.name == "Backward");
        switchCamAction = InputSystem.ListEnabledActions().Find(action => action.name == "SwitchCam");
        percentAction = InputSystem.ListEnabledActions().Find(action => action.name == "percent");
        _gearScripts = (GearRotation[])FindObjectsOfType(typeof(GearRotation));
    }

    // Update is called once per frame
    void Update()
    {
        if(forwardAction.WasPressedThisFrame())
        {
            foreach (var moveScript in movingScripts)
            {
                moveScript.MoveToNextPosition();
            }
            MoveGears();
        }

        if (backwardAction.WasPressedThisFrame())
        {
            foreach (var moveScript in movingScripts)
            {
                moveScript.MoveToPrevPosition();
            }
            MoveGears();
        }
        
        if (switchCamAction.WasPressedThisFrame())
        {
            _cameras[_currentCamIndex].enabled = false;
            _currentCamIndex = _currentCamIndex + 1 < _cameras.Length ? _currentCamIndex+1 : 0;
            _cameras[_currentCamIndex].enabled = true;
        }

        if (percentAction.WasPressedThisFrame())
        {
            Debug.Log("Setting to " + percentage);
            foreach (var moveScript in movingScripts)
            {
                moveScript.MoveToPercentage(percentage);
            }
            MoveGears();
        }
    }

    void MoveGears()
    {
        foreach (var gearScript in _gearScripts)
        {
            StartCoroutine(gearScript.RotateGear());
        }
    }
}
