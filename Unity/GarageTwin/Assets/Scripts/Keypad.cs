using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Keypad : MonoBehaviour
{
    public TMP_InputField inputPrompt;
    public TMP_InputField tMP_InputField;
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject button4;
    public GameObject button5;
    public GameObject button6;
    public GameObject button7;
    public GameObject button8;
    public GameObject button9;
    public GameObject button0;
    public GameObject enterButton;
    public GameObject clearButton;
    private InputAction zeroAction;
    private InputAction oneAction;
    private InputAction twoAction;
    private InputAction threeAction;
    private InputAction fourAction;
    private InputAction fiveAction;
    private InputAction sixAction;
    private InputAction sevenAction;
    private InputAction eightAction;
    private InputAction nineAction;
    private InputAction enterAction;
    private InputAction clearAction;
    private UDPManager _udpManager;
    private bool closed = true;



    private string password = "1111";
    // Start is called before the first frame update
    void Start()
    {
        zeroAction = InputSystem.ListEnabledActions().Find(action => action.name == "Zero");
        oneAction = InputSystem.ListEnabledActions().Find(action => action.name == "One");
        twoAction = InputSystem.ListEnabledActions().Find(action => action.name == "Two");
        threeAction = InputSystem.ListEnabledActions().Find(action => action.name == "Three");
        fourAction = InputSystem.ListEnabledActions().Find(action => action.name == "Four");
        fiveAction = InputSystem.ListEnabledActions().Find(action => action.name == "Five");
        sixAction = InputSystem.ListEnabledActions().Find(action => action.name == "Six");
        sevenAction = InputSystem.ListEnabledActions().Find(action => action.name == "Seven");
        eightAction = InputSystem.ListEnabledActions().Find(action => action.name == "Eight");
        nineAction = InputSystem.ListEnabledActions().Find(action => action.name == "Nine");
        enterAction = InputSystem.ListEnabledActions().Find(action => action.name == "Enter");
        clearAction = InputSystem.ListEnabledActions().Find(action => action.name == "Clear");
        inputPrompt.text = "ENTER PASSWORD:";
        tMP_InputField.characterLimit = 16;
        inputPrompt.characterLimit = 16;
        _udpManager = FindObjectOfType<UDPManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if(oneAction.WasPressedThisFrame())
        {
            buttonOne();
        }

        if(twoAction.WasPressedThisFrame())
        {
            buttonTwo();
        }

        if(threeAction.WasPressedThisFrame())
        {
            buttonThree();
        }

        if(fourAction.WasPressedThisFrame())
        {
            buttonFour();
        }

        if(fiveAction.WasPressedThisFrame())
        {
            buttonFive();
        }

        if(sixAction.WasPressedThisFrame())
        {
            buttonSix();
        }

        if(sevenAction.WasPressedThisFrame())
        {
            buttonSeven();
        }

        if(eightAction.WasPressedThisFrame())
        {
            buttonEight();
        }

        if(nineAction.WasPressedThisFrame())
        {
            buttonNine();
        }

        if(zeroAction.WasPressedThisFrame())
        {
            buttonZero();
        }

        if(enterAction.WasPressedThisFrame())
        {
            keypadEnter();
        }

        if(clearAction.WasPressedThisFrame())
        {
            keypadClear();
        }
    }
    public void buttonOne(){
        SendMessage("1");
        tMP_InputField.text += "1";
    }
    public void buttonTwo(){
        SendMessage("2");
        tMP_InputField.text += "2";
    }
    public void buttonThree(){
        SendMessage("3");
        tMP_InputField.text += "3";
    }
    public void buttonFour(){
        SendMessage("4");
        tMP_InputField.text += "4";
    }
    public void buttonFive(){
        SendMessage("5");
        tMP_InputField.text += "5";
    }
    public void buttonSix(){
        SendMessage("6");
        tMP_InputField.text += "6";
    }
    public void buttonSeven(){
        SendMessage("7");
        tMP_InputField.text += "7";
    }
    public void buttonEight(){
        SendMessage("8");
        tMP_InputField.text += "8";
    }
    public void buttonNine(){
        SendMessage("9");
        tMP_InputField.text += "9";
    }
    public void buttonZero(){
        SendMessage("0");
        tMP_InputField.text += "0";
    }

    public void keypadClear() {
        SendMessage("-2");
        tMP_InputField.text = null;
    }
    public void buttonClear() {
        tMP_InputField.text = null;
    }

    public void keypadEnter()
    {
        SendMessage("-1");
        if (tMP_InputField.text == password)
        {
            StartCoroutine(PincodeInput("Correct"));
        }
        else
        {
            StartCoroutine(PincodeInput("Wrong"));
        }
    }
    public void buttonEnter()
    {
        if (tMP_InputField.text == password)
        {
            StartCoroutine(PincodeInput("Correct"));
        }
        else
        {
            StartCoroutine(PincodeInput("Wrong"));
        }
    }

    public new void SendMessage(string message)
    {
        _udpManager.SendUDPMessage(message, _udpManager.GarageIPAddress, _udpManager.GaragePort);
    }
    
    public void EnterKey(string key)
    {
        tMP_InputField.text += key;
    }

    private IEnumerator PincodeInput(string input)
    {
        inputPrompt.text = input;        
        tMP_InputField.text = null;
        yield return new WaitForSeconds (3);
        inputPrompt.text = "ENTER PASSWORD:";
    }
}
