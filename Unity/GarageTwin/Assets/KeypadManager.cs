using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeypadManager : MonoBehaviour
{
    private Keypad[] _keypads;

    private void Start()
    {
        _keypads = (Keypad[])FindObjectsOfType(typeof(Keypad));
    }

    public void EnterCode(string code)
    {
        _keypads.First().EnterKey(code);
    }

    public void Clear()
    {
        _keypads.First().buttonClear();
    }
    public void Enter()
    { 
        _keypads.First().buttonEnter();
    }
}