using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using GenericParsing;
using LumenWorks.Framework.IO.Csv;



namespace Proftaak_TalkToMe_Bottlestop
{
    class Program
    {
        static void Main()
        {
            //Add Databse
            CustomerDatabase customerDatabase = new CustomerDatabase();
            //Add Fillingstation
            FillingStation thisStation = new FillingStation();
            thisStation.StationLocation = "Eindhoven, Rachelsmolen 1";
            thisStation.StationID = "ST000001";
            //Add RFID sensor
            RFIDSensor stationSensor = new RFIDSensor();
            stationSensor.RFIDSensorID = "SE0000001";
            //Add reservoirs and corresponding pumps and ultra sonic sensors
            Reservoir reservoir1 = new Reservoir();
            reservoir1.ReservoirPosition = 1;
            reservoir1.Drink = "WATER";
            reservoir1.pump = new Pump();
            reservoir1.pump.PumpNumber = 1;
            reservoir1.ussensor = new UltraSonicSensor();
            Reservoir reservoir2 = new Reservoir();
            reservoir2.ReservoirPosition = 2;
            reservoir2.Drink = "COLA";
            reservoir2.pump = new Pump();
            reservoir2.pump.PumpNumber = 2;
            reservoir2.ussensor = new UltraSonicSensor();
            Reservoir reservoir3 = new Reservoir();
            reservoir3.ReservoirPosition = 3;
            reservoir3.Drink = "FANTA";
            reservoir3.pump = new Pump();
            reservoir3.pump.PumpNumber = 3;
            reservoir3.ussensor = new UltraSonicSensor();
            //Add created reservoirs with pumps and ultrasonic sensors to reservoirs list
            thisStation.reservoirs.Add(reservoir1);
            thisStation.reservoirs.Add(reservoir2);
            thisStation.reservoirs.Add(reservoir3);
            thisStation.availableDrinks.Add(reservoir1.Drink);
            thisStation.availableDrinks.Add(reservoir2.Drink);
            thisStation.availableDrinks.Add(reservoir3.Drink);

            //Create a new arduino and connect to it
            Arduino arduino = new Arduino();
            //Load database
            customerDatabase.LoadDatabase();

            //Connect Arduino and start collecting data
            arduino.ConnectToArduino();
            while (arduino.isConnected == true)
            {
                if (arduino.ReceivedData().Length > 0)
                {
                    string receiveddata = arduino.ReceivedData();
                    string datamessage = receiveddata.Substring(0, 5);
                    if (datamessage == "@BOID")
                    {
                        string processedid = receiveddata.Substring(5, (receiveddata.Length-5));
                        //Create new client with subscription and bottle
                        Customer thisCustomer = new Customer();
                        Bottle thisBottle = new Bottle();
                        Subscription thisSubscription = new Subscription();
                        thisCustomer.bottles.Add(thisBottle);
                        thisBottle.BottleID = processedid;
                        //Check if BottleID is present in database
                        if (customerDatabase.DatabaseCheck(thisBottle.BottleID) == true)
                        {
                            //Assign other variables from database by looking for data at the same index as bottleid in the other lists
                            int dbindex = customerDatabase.DataLocation(thisBottle.BottleID);
                            thisBottle.BottleType = customerDatabase.dbbottletype[dbindex];
                            thisCustomer.customerID = customerDatabase.dbcustomerid[dbindex];
                            thisCustomer.customerName = customerDatabase.dbcustomername[dbindex];
                            thisSubscription.subscriptionID = customerDatabase.dbsubscriptionid[dbindex];
                            thisSubscription.subscriptionType = customerDatabase.dbsubscriptiontype[dbindex];
                            Console.WriteLine("Subscription type: " + thisSubscription.subscriptionType);
                            //Measure how much of each drink is available


                            //Create selection of drink to choose from
                            thisStation.DrinkSelection(thisSubscription.subscriptionType);
                            //Wait for selection by customer
                            if (thisStation.AwaitOrder().Length > 0)
                            {
                                Console.WriteLine("Please wait while we fill your bottle with " + thisStation.AwaitOrder());
                                arduino.CommandDrinkChoice(thisStation.AwaitOrder());
                                arduino.CommandFill(thisBottle.BottleType);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Bottle not registered");
                        }
                    }
                    //If Arduino sends that reservoir is too empty give warning
                    else if (datamessage == "@FULL")
                    {
                        string emptyreservoir = receiveddata.Substring(5, 2);
                        if (emptyreservoir == "01")
                        {
                            Console.WriteLine("Not enough water available to fill bottle");
                        }
                        if (emptyreservoir == "02")
                        {
                            Console.WriteLine("Not enough cola available to fill bottle");
                        }
                        if (emptyreservoir == "03")
                        {
                            Console.WriteLine("Not enough fanta available to fill bottle");
                        }
                    }
                }
            }
        }
    }
}
