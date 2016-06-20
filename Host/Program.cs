using Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAdress2 = new Uri("http://localhost:2014");
            ServiceHost mojHost2 = new ServiceHost(typeof(Service1), baseAdress2);

            BasicHttpBinding b = new BasicHttpBinding();
            b.TransferMode = TransferMode.Streamed;
            b.MaxReceivedMessageSize = 67108864;
            ServiceEndpoint endpoint = mojHost2.AddServiceEndpoint(typeof(IService1), b, "Service");

            Uri baseAdress3 = new Uri("http://localhost:1014");
            ServiceHost mojHost3= new ServiceHost(typeof(Load), baseAdress3);

            WSDualHttpBinding b2 = new WSDualHttpBinding();
            //b2.TransferMode = TransferMode.Streamed;
            b2.MaxReceivedMessageSize = 67108864;
            ServiceEndpoint endpoint2 = mojHost3.AddServiceEndpoint(typeof(ILoad), b2, "Load");
            
            ServiceMetadataBehavior smb2 = new ServiceMetadataBehavior();
            smb2.HttpGetEnabled = true;
            mojHost3.Description.Behaviors.Add(smb2);

            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            mojHost2.Description.Behaviors.Add(smb);

            try
            {
                mojHost2.Open();
                mojHost3.Open();
                Console.WriteLine("Serwis 2 jest uruchomiony.");
                Console.WriteLine("Nacisnij <ENTER> aby zakonczyc.");
                Console.WriteLine();
                Console.ReadLine();
                mojHost2.Close();
                mojHost3.Close();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("Wystapil wyjatek: {0}", ce.Message);
                mojHost2.Abort();
            }
        }
    }
}
