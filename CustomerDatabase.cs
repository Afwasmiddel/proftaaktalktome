using System;
using System.Collections.Generic;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace Proftaak_TalkToMe_Bottlestop
{
    class CustomerDatabase
    {
        public List<string> dbrecords = new List<string>();
        public List<string> dbcustomerid = new List<string>();
        public List<string> dbcustomername = new List<string>();
        public List<string> dbbottleid = new List<string>();
        public List<string> dbbottletype = new List<string>();
        public List<string> dbsubscriptionid = new List<string>();
        public List<string> dbsubscriptiontype = new List<string>();


        public void LoadDatabase()
        {
            //read file
            using (CsvReader csv =
                  new CsvReader(new StreamReader(@"C:\Users\jvdbe\Documents\Fontys ICT\Proftaak\Semester 2 Talk To Me\Database\CustomerDatabase.csv"), true))
            {
                int fieldCount = csv.FieldCount;
                //seperate headers from datarows
                string[] headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    //split each row 
                    for (int i = 0; i < fieldCount; i++)
                    {
                        //Console.Write(string.Format("{0} = {1};", headers[i], csv[i]));
                        //Console.WriteLine();
                        string[] dbrowrecords = csv[i].Split(';');
                        dbcustomerid.Add(dbrowrecords[0]);
                        dbcustomername.Add(dbrowrecords[1]);
                        dbbottleid.Add(dbrowrecords[2]);
                        dbbottletype.Add(dbrowrecords[3]);
                        dbsubscriptionid.Add(dbrowrecords[4]);
                        dbsubscriptiontype.Add(dbrowrecords[5]);
                    }
                }
            }
        }

        //check database with bottleID and find customerID, bottlevolume, remaining fill volume, subscriptiontype
        public bool DatabaseCheck(string bottleid)
        {
            if (dbbottleid.Contains(bottleid))
            {
                Console.WriteLine("Valid BottleID");
                return true;
            }
            else
            {
                return false;
            }
        }

        public int DataLocation(string bottleid)
        {
            int index = dbbottleid.FindIndex(x => x.StartsWith(bottleid));
            return index;
        }
    }
}
