using System;
using System.Data;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LumenWorks.Framework.IO.Csv;


namespace Proftaak_TalkToMe_Bottlestop
{

    class Customer
    {
        public string customerID;
        public string customerName;

        public List<Bottle> bottles = new List<Bottle>();
        public Bottle thisBottle;
        public Subscription thisSubscription;

        
    }
}
