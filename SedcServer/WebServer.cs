using System;
using System.Net;
using System.Net.Sockets;

namespace SedcServer
{
    public class WebServer
    {
        public ushort Port { get; private set; }

        private bool isStarted = false;
        private Socket serverSocket;

        public WebServer(ushort port)
        {
            Port = port;
        }

        public string Start()
        {
            if (isStarted)
            {
                return "Already started!";
            }

            InitializeSocket();
            isStarted = true;
            while (isStarted)
            {
                var requestHandler = new RequestHandler(serverSocket);
                requestHandler.AcceptRequest();
            }

            return "Started...";
        }

        public string Stop()
        {
            return string.Empty;
        }

        private void InitializeSocket()
        {
            serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, Port));
            serverSocket.Listen(10);
            serverSocket.ReceiveTimeout = 10;
            serverSocket.SendTimeout = 10;
        }
    }
}