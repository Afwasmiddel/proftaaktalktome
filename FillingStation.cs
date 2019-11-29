using System.Collections.Generic;
using System;

namespace Proftaak_TalkToMe_Bottlestop
{
    class FillingStation
    {
        public string StationLocation;
        public string StationID;
        public List<string> drinkSelection = new List<string>();
        public List<string> availableDrinks = new List<string>();
        public List<Reservoir> reservoirs = new List<Reservoir>();

        public void DrinkSelection(string subscriptiontype)
        {
            if (subscriptiontype == "1")
            {
                drinkSelection.Add(availableDrinks[0]);
                Console.WriteLine("Choose drink from: " + drinkSelection[0]);
            }
            if (subscriptiontype == "2")
            {
                Console.WriteLine("Choose drink from: ");
                for (int i = 0; i < availableDrinks.Count; i++)
                {
                    drinkSelection.Add(availableDrinks[i]);
                    Console.Write(availableDrinks[i]+", ");
                }
                //drinkSelection.Add(availableDrinks[0]);
                //drinkSelection.Add(availableDrinks[1]);
                //drinkSelection.Add(availableDrinks[2]);
                // + drinkSelection[0] + ", " + drinkSelection[1] + ", " + drinkSelection[3]);
            }
        }

        public string AwaitOrder()
        {
            string order = Console.ReadLine();
            //if (drinkSelection.Contains(order))
            //{
                return order.ToUpper();
            //}
            //else
            //{
            //    return null;
            //}
        }
    }
}
