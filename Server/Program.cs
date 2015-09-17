using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // http://www.codeproject.com/Articles/742260/Simple-Web-Server-in-csharp
            WebServer ws = new WebServer();

            ws.Start(IPAddress.Loopback, 666, @"C:\Source\Github\SEDC-DataServer\Site");
        }
    }
}
