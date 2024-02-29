using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Management;
using Unity.VisualScripting;

public class Arduino : MonoBehaviour
{
    SerialPort serialPort;
    public string portName = "COM3";
    public int baudRate = 9600;

    void Start()
    {
        // Initialize the SerialPort
        serialPort = new SerialPort(portName, baudRate);
        try
        {
            // Open the serial port
            serialPort.Open();
            Debug.Log("Serial port opened.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error opening serial port: " + e.Message);
        }

        //StartCoroutine(FindArdy());
    }

    private void Update()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                // Read data from the serial port
                string message = serialPort.ReadLine();
                Debug.Log("Message from Arduino: " + message);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error reading from serial port: " + e.Message);
            }
        }
    }

    private void OnDisable()
    {
        if(serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Serial port closed.");
        }
    }

    /* // Start is called before the first frame update
     IEnumerator FindArdy()
     {
         // Get all available COM ports
         string[] portNames = SerialPort.GetPortNames();

         foreach (string portName in portNames)
         {
             using (SerialPort port = new SerialPort(portName, 9600)) // Adjust baud rate as per your Arduino's settings
             {
                 try
                 {
                     port.Open();
                     port.Write("identify\r"); // Send a command to identify the device
                     port.BaseStream.Flush(); // Flush the stream to ensure the command is sent

                     // Yield outside of the try block
                 }
                 catch (System.Exception ex)
                 {
                     Debug.LogWarning("Error communicating with " + portName + ": " + ex.Message);
                 }
             }
         }

         yield return new WaitForSeconds(1); // Wait for response after sending the command

         foreach (string portName in portNames)
         {
             try
             {
                 using (SerialPort port = new SerialPort(portName, 9600)) // Adjust baud rate as per your Arduino's settings
                 {
                     port.Open();
                     string response = port.ReadExisting(); // Read response

                     if (!string.IsNullOrEmpty(response))
                     {
                         Debug.Log(portName + " responded: " + response);
                         // Analyze the response to determine if it's from an Arduino
                         if (response.Contains("Arduino") ||  response.Contains("CH340") || response.Contains("FTDI"))
                         {
                             Debug.Log(portName + " is likely connected to an Arduino.");
                         }
                         else
                         {
                             Debug.Log(portName + " is not recognized as an Arduino.");
                         }
                     }
                     else
                     {
                         Debug.Log(portName + " did not respond.");
                     }

                     port.Close();
                 }
             }
             catch (System.Exception ex)
             {
                 Debug.LogWarning("Error communicating with " + portName + ": " + ex.Message);
             }
         }
     }*/
}
