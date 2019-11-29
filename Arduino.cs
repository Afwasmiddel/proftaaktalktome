using System;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Proftaak_TalkToMe_Bottlestop
{
    class Arduino
    {
        public bool isConnected = false;
        static SerialPort _serialPort;


        public void ConnectToArduino() //Establish connection with Arduino
        {
            isConnected = true;
            _serialPort = new SerialPort();
            _serialPort.PortName = "COM6"; //Set your board COM
            _serialPort.BaudRate = 9600;
            _serialPort.Open();
            Console.WriteLine("Connection Established");
        }

        public void DisconnectFromArduino() //Close connection with arduino
        {
            isConnected = false;
            _serialPort.Close();
            Console.WriteLine("Connection lost");
        }

        public string ReceivedData() //Start reading incoming messages and respond when a specific message is sent
        {
            string processedmessage = "";
            string incomingmessage = Arduino.PortRead();
            //string incomingmessage = "@BOID9";
            if (incomingmessage.Length > 5)
            {
                string datamessage = incomingmessage.Substring(0, 5);
                if (datamessage == "@BOID")
                {
                    processedmessage = incomingmessage;
                }
            }
            return processedmessage;
        }

        public void CommandFill(string bottletype) //Make message to fill bottle
        {
            string command = "#FILL" + bottletype + "#\n";
            PortWrite(command);
        }

        public void CommandDrinkChoice(string drinkchoice) //Make message to send chosen drink
        {
            string command = "#DRIN" + drinkchoice + "#\n";
            PortWrite(command);
        }

        public void PortWrite(string message) //Send message to Arduino
        {
            if (isConnected == true)
            {
                _serialPort.Write(message);
                Thread.Sleep(200);
            }
        }

        public static string PortRead() //Read message sent from Arduino to C#
        {
            string arduinoConsole = _serialPort.ReadExisting();
            Thread.Sleep(200);
            return arduinoConsole;
        }
    }
}
