using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlData;
using System.Xml.Serialization;
using System.IO;

namespace LoadXml
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageHelper msgHlpr = MessageHelper.GetInstance();
            int countMessages = msgHlpr.Items.Count;
            IList<MessageData> lstData = msgHlpr.Items;
            for (int i = 0; i < countMessages; i++)
            {
                Console.WriteLine("Id= {0}", lstData[i].Id);
                Console.WriteLine("Type= {0}", lstData[i].Type);
                Console.WriteLine("Description= {0}", lstData[i].Description);
                Console.WriteLine("------------------------------");
            }


            MessageHelper msgHlpr2 = MessageHelper.GetInstance();
            int countMessages2 = msgHlpr2.Items.Count;
            IList<MessageData> lstData2 = msgHlpr2.Items;
            for (int i = 0; i < countMessages2; i++)
            {
                Console.WriteLine("Id= {0}", lstData2[i].Id);
                Console.WriteLine("Type= {0}", lstData2[i].Type);
                Console.WriteLine("Description= {0}", lstData2[i].Description);
                Console.WriteLine("------------------------------");
            }

            Console.ReadLine();
        }

    }
}
