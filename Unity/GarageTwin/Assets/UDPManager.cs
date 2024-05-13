using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UDPManager : MonoBehaviour
{
    // Static variable that holds the instance
    public static UDPManager Instance { get; private set; }

    // UDP Settings
    [Header("UDP Settings")]
    [SerializeField] private int UDPPort = 50195;
    [SerializeField] private bool displayUDPMessages = true;
    [SerializeField] private string garageIPAddress = "192.168.125.102";
    [SerializeField] private int garagePort = 3002;

    public string GarageIPAddress => garageIPAddress;
    public int GaragePort => garagePort;
    private UdpClient udpClient;
    private IPEndPoint endPoint;
    
    // Move manager
    private MoveManager _moveManager;

    // Keypad handler
    private KeypadManager _keypadManager;
    
    // ESP32 Sensor
    public float potentiometerValue { get; private set; } = 0;
    public string keypadCode { get; private set; } = "";
    public string KeypadInteraction { get; private set; } = "";
    private string prevKeypadCode = "";
    private float prevPotVal = -1;

    void Awake()
    {
        // Assign the instance to this instance, if it is the first one
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _moveManager = FindObjectOfType<MoveManager>();
        _keypadManager = FindObjectOfType<KeypadManager>();
        
        //Get IP Address
        DisplayIPAddress();

        // UDP begin
        endPoint = new IPEndPoint(IPAddress.Any, UDPPort);
        udpClient = new UdpClient(endPoint);
        udpClient.BeginReceive(ReceiveCallback, null);
        if(displayUDPMessages)
            Debug.Log("UDP - Start receiving. On port: " + UDPPort);
    }

    private void Update()
    {
        if (Math.Abs(prevPotVal - potentiometerValue) > 0.0001)
        {
            _moveManager.MoveObjectsByPercentage(potentiometerValue);
            prevPotVal = potentiometerValue;
        }

        
        
        if (prevKeypadCode != keypadCode)
        {
                _keypadManager.EnterCode(""+keypadCode.LastOrDefault());
                prevKeypadCode = keypadCode;
            
        }
        
        switch (KeypadInteraction)
        {
            case "-1": 
                _keypadManager.Enter();
                KeypadInteraction = "";
                StartCoroutine(WaitSeconds());
                keypadCode = "";
                break;
            case "-2": _keypadManager.Clear();
                KeypadInteraction = "";
                break;
        }
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        
        byte[] receivedBytes = udpClient.EndReceive(result, ref endPoint);
        string receivedData = Encoding.UTF8.GetString(receivedBytes);

        // Log UDP message
        if (displayUDPMessages)
        {
            Debug.Log("Received data from " + endPoint.Address.ToString() + ": " + receivedData);
        }

        // Splitting the receivedData string by the '|' character
        string[] parts = receivedData.Split('|');

        if (parts.Length == 2)
        {
            if (parts[0] == "pot")
            {
                string sensorID = parts[0];
                float value;
                if (float.TryParse(parts[1], out value))
                {
                    potentiometerValue = value;
                }
                else
                {
                    Debug.LogError("Failed to parse the value as an integer.");
                }
            }
            else if (parts[0] == "key")
            {
                int value;
                if (int.TryParse(parts[1], out value))
                {
                    if (value == -1)
                    {
                        KeypadInteraction = "" + value;
                    }
                    else if (value == -2)
                    {
                        KeypadInteraction = "" + value;
                        keypadCode = "";
                    }
                    else
                    {
                        keypadCode += value;
                    }
                }
                else
                {
                    Debug.LogError("Failed to parse the value as an integer.");
                }
            }
            else
            {
                Debug.LogError("non-supported command type");
            }
        }
        else
        {
            Debug.LogError("Received data is not in the expected format.");
        }

        udpClient.BeginReceive(ReceiveCallback, null);
    }

    public void SendUDPMessage(string message, string ipAddress, int port)
    {
        UdpClient client = new UdpClient();
        try
        {
            // Convert the message string to bytes
            byte[] data = Encoding.UTF8.GetBytes(message);

            // Send the UDP message
            client.Send(data, data.Length, ipAddress, port);
            Debug.Log("UDP message sent: " + message);
        }
        catch (Exception e)
        {
            Debug.LogError("Error sending UDP message: " + e.Message);
        }
        finally
        {
            client.Close();
        }
    }

    void DisplayIPAddress()
    {
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    Debug.Log(ip.ToString());
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error fetching local IP address: " + ex.Message);
        }
    }
    private IEnumerator WaitSeconds()
    {
        yield return new WaitForSecondsRealtime(2);
    }
    
    private void OnDestroy()
    {
        if (udpClient != null)
        {
            udpClient.Close();
        }
    }
}
