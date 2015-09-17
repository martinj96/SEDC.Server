using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SedcServer
{
    public class RequestHandler
    {
        public Socket ServerSocket { get; private set; }

        public RequestHandler(Socket socket)
        {
            ServerSocket = socket;
        }

        internal async void AcceptRequest()
        {
            Socket clientSocket = null;
            try
            {
                // Create new thread to handle the request and continue to listen the socket.
                clientSocket = ServerSocket.Accept();
                clientSocket.ReceiveTimeout = ServerSocket.ReceiveTimeout;
                clientSocket.SendTimeout = ServerSocket.SendTimeout;

                //await Task.Run(() => HandleTheRequest(clientSocket));
                var requestHandler = new Thread(() => HandleTheRequest(clientSocket));
                requestHandler.Start();

                //HandleTheRequest(clientSocket);
            }
            catch
            {
                Console.WriteLine("Error in accepting client request");
                Console.ReadLine();
                if (clientSocket != null)
                    clientSocket.Close();
            }
        }

        private void HandleTheRequest(Socket clientSocket)
        {
            //Thread.Sleep(5000);
            //Console.WriteLine("Server request at {0}", DateTime.UtcNow);
            string requestString = DecodeRequest(clientSocket);
            RequestParser parser = new RequestParser(requestString);

            Console.WriteLine("Received request {0}", requestString);

            byte[] byteResponse = new byte[0];
            string statusCode = "200";
            string contentType = "text/html";
            switch (parser.Kind)
            {
                case RequestKind.Action:
                    switch (parser.Action.ToLower())
                    {
                        case "sayhello":
                            {
                                string response = "<h1>Hello " + parser.Parameter + "</h1>";
                                byteResponse = Encoding.UTF8.GetBytes(response);
                                break;
                            }
                        default:
                            {
                                string response = "<h1>Unsupported action " + parser.Action + "</h1>";
                                byteResponse = Encoding.UTF8.GetBytes(response);
                                break;
                            }
                    } 
                    break;
                case RequestKind.File:
                    if (File.Exists(parser.FileName))
                    {
                        switch (parser.Extension)
                        {
                            case "ico":
                            {
                                byteResponse = File.ReadAllBytes(parser.FileName);
                                contentType = "image/ico";
                                break;
                            }
                            case "html":
                            {
                                byteResponse = File.ReadAllBytes(parser.FileName);
                                break;
                            }
                            default:
                            {
                                byteResponse = File.ReadAllBytes(parser.FileName);
                                contentType = "text/plain";
                                break;
                            }
                        }
                    }
                    else
                    {
                        statusCode = "404";
                    }
                    break;
                case RequestKind.Error:
                    statusCode = "501";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }



            byte[] byteHeader = CreateHeader(statusCode, byteResponse.Length, contentType);
            clientSocket.Send(byteHeader);
            clientSocket.Send(byteResponse);
            clientSocket.Close();
        }

        public void StopClientSocket(Socket clientSocket)
        {
            if (clientSocket != null)
                clientSocket.Close();
        }

        private string DecodeRequest(Socket clientSocket)
        {
            var receivedBufferlen = 0;
            var buffer = new byte[10240];
            try
            {
                receivedBufferlen = clientSocket.Receive(buffer);
            }
            catch (Exception)
            {
                //Console.WriteLine("buffer full");
                Console.ReadLine();
            }
            return Encoding.UTF8.GetString(buffer, 0, receivedBufferlen);
        }

        private byte[] CreateHeader(string responseCode, int contentLength, string contentType)
        {
            return Encoding.UTF8.GetBytes("HTTP/1.1 " + responseCode + "\r\n"
                                  + "Server: SEDC Data Web Server\r\n"
                                  + "Content-Length: " + contentLength + "\r\n"
                                  + "Connection: close\r\n"
                                  + "Content-Type: " + contentType + "\r\n\r\n");
        }
    }
}
