using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SedcServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ushort port = 8081;
            if (args.Length != 0)
            {
                ushort paramPort;
                if (UInt16.TryParse(args[0], out paramPort))
                {
                    port = paramPort;
                }
            }
            else
            {
                string confPort = System.Configuration.ConfigurationManager.AppSettings["port"];
                if (confPort != null)
                {
                    UInt16.TryParse(confPort, out port);
                }
                else
                {
                    Console.WriteLine("{0}", port);
                }
            }
            bool isAvailable = true;

            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpi in tcpConnArray)
            {
                if (tcpi.LocalEndPoint.Port == port)
                {
                    isAvailable = false;
                    break;
                }
            }

            IPEndPoint[] listeningConnArray = ipGlobalProperties.GetActiveTcpListeners();
            foreach (IPEndPoint conn in listeningConnArray)
            {
                if (conn.Port == port)
                {
                    isAvailable = false;
                    break;
                }
            }

            if (isAvailable)
            {
                WebServer ws = new WebServer(port);
                ws.Start();
            }
            else
            {
                Console.WriteLine("Error! The requsted port is already in use.");
            }
            
            //while (true)
            //{
            //    var command = Console.ReadLine();
            //    if (command == "exit")
            //        ws.Stop();
            //}
            
        }
    }
}
