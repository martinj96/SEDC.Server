using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class WebServer
    {
        // check for already running
        private bool _running = false;
        private int _timeout = 5;
        private Encoding _charEncoder = Encoding.UTF8;
        private Socket _serverSocket;

        // Directory to host our contents
        private string _contentPath;

        //create socket and initialization
        private void InitializeSocket(IPAddress ipAddress, int port, string contentPath) //create socket
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(new IPEndPoint(ipAddress, port));
            _serverSocket.Listen(10);    //no of request in queue
            _serverSocket.ReceiveTimeout = _timeout;
            _serverSocket.SendTimeout = _timeout;
            _running = true; //socket created
            _contentPath = contentPath;
        }

        public void Start(IPAddress ipAddress, int port, string contentPath)
        {
            try
            {
                InitializeSocket(ipAddress, port, contentPath);
            }
            catch
            {
                Console.WriteLine("Error in creating server socket");
                Console.ReadLine();

            }
            while (_running)
            {
                var requestHandler = new RequestHandler(_serverSocket, contentPath);
                requestHandler.AcceptRequest();
            }
        }

        public void Stop()
        {
            _running = false;
            try
            {
                _serverSocket.Close();
            }
            catch
            {
                Console.WriteLine("Error in closing server or server already closed");
                Console.ReadLine();

            }
            _serverSocket = null;
        }

    }
}
