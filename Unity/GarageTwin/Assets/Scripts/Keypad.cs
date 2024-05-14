using System.Collections;

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
    private InputAction _zeroAction;
    private InputAction _oneAction;
    private InputAction _twoAction;
    private InputAction _threeAction;
    private InputAction _fourAction;
    private InputAction _fiveAction;
    private InputAction _sixAction;
    private InputAction _sevenAction;
    private InputAction _eightAction;
    private InputAction _nineAction;
    private InputAction _enterAction;
    private InputAction _clearAction;
    private UDPManager _udpManager;

    private string _password = "1111";
    // Start is called before the first frame update
    void Start()
    {
        _zeroAction = InputSystem.ListEnabledActions().Find(action => action.name == "Zero");
        _oneAction = InputSystem.ListEnabledActions().Find(action => action.name == "One");
        _twoAction = InputSystem.ListEnabledActions().Find(action => action.name == "Two");
        _threeAction = InputSystem.ListEnabledActions().Find(action => action.name == "Three");
        _fourAction = InputSystem.ListEnabledActions().Find(action => action.name == "Four");
        _fiveAction = InputSystem.ListEnabledActions().Find(action => action.name == "Five");
        _sixAction = InputSystem.ListEnabledActions().Find(action => action.name == "Six");
        _sevenAction = InputSystem.ListEnabledActions().Find(action => action.name == "Seven");
        _eightAction = InputSystem.ListEnabledActions().Find(action => action.name == "Eight");
        _nineAction = InputSystem.ListEnabledActions().Find(action => action.name == "Nine");
        _enterAction = InputSystem.ListEnabledActions().Find(action => action.name == "Enter");
        _clearAction = InputSystem.ListEnabledActions().Find(action => action.name == "Clear");
        inputPrompt.text = "ENTER PASSWORD:";
        tMP_InputField.characterLimit = 16;
        inputPrompt.characterLimit = 16;
        _udpManager = FindObjectOfType<UDPManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if(_oneAction.WasPressedThisFrame())
        {
            ButtonOne();
        }

        if(_twoAction.WasPressedThisFrame())
        {
            ButtonTwo();
        }

        if(_threeAction.WasPressedThisFrame())
        {
            ButtonThree();
        }

        if(_fourAction.WasPressedThisFrame())
        {
            ButtonFour();
        }

        if(_fiveAction.WasPressedThisFrame())
        {
            ButtonFive();
        }

        if(_sixAction.WasPressedThisFrame())
        {
            ButtonSix();
        }

        if(_sevenAction.WasPressedThisFrame())
        {
            ButtonSeven();
        }

        if(_eightAction.WasPressedThisFrame())
        {
            ButtonEight();
        }

        if(_nineAction.WasPressedThisFrame())
        {
            ButtonNine();
        }

        if(_zeroAction.WasPressedThisFrame())
        {
            ButtonZero();
        }

        if(_enterAction.WasPressedThisFrame())
        {
            KeypadEnter();
        }

        if(_clearAction.WasPressedThisFrame())
        {
            KeypadClear();
        }
    }
    public void ButtonOne(){
        SendMessage("1");
        tMP_InputField.text += "1";
    }
    public void ButtonTwo(){
        SendMessage("2");
        tMP_InputField.text += "2";
    }
    public void ButtonThree(){
        SendMessage("3");
        tMP_InputField.text += "3";
    }
    public void ButtonFour(){
        SendMessage("4");
        tMP_InputField.text += "4";
    }
    public void ButtonFive(){
        SendMessage("5");
        tMP_InputField.text += "5";
    }
    public void ButtonSix(){
        SendMessage("6");
        tMP_InputField.text += "6";
    }
    public void ButtonSeven(){
        SendMessage("7");
        tMP_InputField.text += "7";
    }
    public void ButtonEight(){
        SendMessage("8");
        tMP_InputField.text += "8";
    }
    public void ButtonNine(){
        SendMessage("9");
        tMP_InputField.text += "9";
    }
    public void ButtonZero(){
        SendMessage("0");
        tMP_InputField.text += "0";
    }

    public void KeypadClear() {
        SendMessage("-2");
        tMP_InputField.text = null;
    }
    public void ButtonClear() {
        tMP_InputField.text = null;
    }

    public void KeypadEnter()
    {
        SendMessage("-1");
        if (tMP_InputField.text == _password)
        {
            StartCoroutine(PincodeInput("Correct"));
        }
        else
        {
            StartCoroutine(PincodeInput("Wrong"));
        }
    }
    public void ButtonEnter()
    {
        if (tMP_InputField.text == _password)
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
