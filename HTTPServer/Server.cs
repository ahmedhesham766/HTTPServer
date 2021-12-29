using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;
        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            this.LoadRedirectionRules(redirectionMatrixPath);
            //TODO: initialize this.serverSocket
            IPEndPoint hostEnd = new IPEndPoint(IPAddress.Any, portNumber);
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(hostEnd);

        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            Console.WriteLine("start listening");

            serverSocket.Listen(100);
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.
                Socket clientSocket = this.serverSocket.Accept();
                Console.WriteLine("New Client Accepted: {0}", clientSocket.RemoteEndPoint);
                Thread thread = new Thread(new ParameterizedThreadStart(HandleConnection));
                thread.Start(clientSocket);
            }
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket
            Socket clientSocket = (Socket)obj;

            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            clientSocket.ReceiveTimeout = 0;
            // TODO: receive requests in while true until remote client closes the socket.
            while (true)
            {
                try
                {
                    // TODO: Receive request
                    byte[] recieve_Data = new byte[100000];
                    int receivedLength = clientSocket.Receive(recieve_Data);
                    string data = Encoding.ASCII.GetString(recieve_Data);
                    // TODO: break the while loop if receivedLen==0
                    if (receivedLength == 0)
                    {
                        Console.WriteLine("Client: {0} ended the connection", clientSocket.RemoteEndPoint);
                        break;
                    }
                    // TODO: Create a Request object using received request string
                    Request request = new Request(data);//to make data in format request
                    // TODO: Call HandleRequest Method that returns the response
                    
                    Response server_Resp = HandleRequest(request);
                    string res = server_Resp.ResponseString;
                    byte[] respon = Encoding.ASCII.GetBytes(res);
                    // TODO: Send Response back to client
                    clientSocket.Send(respon);
                    
                    
                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);//obj from logger through on file
                }
            }

            // TODO: close client socket
            clientSocket.Close();
        }

        Response HandleRequest(Request request)
        {
            //throw new NotImplementedException();
            string physical_path;
            string content = "";
            StatusCode code;
            try
            {
                Response re = null;
                //TODO: check for bad request
                if (!request.ParseRequest())
                {
                    code = StatusCode.BadRequest;
                    content = LoadDefaultPage("BadRequest.html");
                    re = new Response(code, "text/html", content, "");
                    return re;
                }
                if (request.relativeURI == "/")
                {

                    code = StatusCode.OK;
                    physical_path = Configuration.RootPath + '\\' + "main.html";
                    content = LoadDefaultPage("main.html");
                    re = new Response(code, "text/html", content, "");
                    return re;
                }
                //TODO: map the relativeURI in request to get the physical path of the resource.

                string[] uri = request.relativeURI.Split('/');

                //fixed it

                physical_path = Configuration.RootPath + '\\' + uri[1];


                //TODO: check for redirect

                if (GetRedirectionPagePathIFExist(request.relativeURI) != String.Empty)
                {
                    
                    code = StatusCode.Redirect;
                    content = LoadDefaultPage(GetRedirectionPagePathIFExist(request.relativeURI));
                    re = new Response(code, "text/html", content, GetRedirectionPagePathIFExist(request.relativeURI));
                    return re;

                }
                //}
                //TODO: check file exists
                if (!File.Exists(physical_path))
                {

                    code = StatusCode.NotFound;
                    content = LoadDefaultPage("NotFound.html");
                    re = new Response(code, "text/html", content, "");
                    return re;

                }
                //TODO: read the physical file
                else
                {

                    code = StatusCode.OK;
                    content = File.ReadAllText(physical_path);
                    re = new Response(code, "text/html", content, "");
                    return re;

                }
                // Create OK response

                //return to it
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error.
                code = StatusCode.InternalServerError;
                content = LoadDefaultPage("InternalServerError.html");
                Response res = new Response(code, "text/html", content, "");
                return res;

            }
        }


        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            for (int i = 0; i < Configuration.RedirectionRules.Count; i++)
            {
                if ('/' + Configuration.RedirectionRules.Keys.ElementAt(i).ToString() == relativePath)
                {
                    string redirected_path = Configuration.RedirectionRules.Values.ElementAt(i).ToString();
                    return redirected_path;
                }

            }
            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            string content = "";
            // TODO: check if filepath not exist, log exception using Logger class and return empty strin
            try
            {
                if (File.Exists(filePath))
                {
                    content = File.ReadAllText(filePath);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

            }
            // else read file and return its content
            return content;
        }

        private void LoadRedirectionRules(string filePath)
        {
            FileStream fs = null;

            try
            {
                // TODO: using the filepath paramter read the redirection rules from file
                fs = new FileStream(filePath, FileMode.Open);

                StreamReader sr = new StreamReader(fs);

                // then fill Configuration.RedirectionRules dictionary
                while (sr.Peek() != -1)
                {
                    string line = sr.ReadLine();
                    string[] data = line.Split(',');
                    if (data[0] == "")
                        break;
                    Configuration.RedirectionRules.Add(data[0], data[1]);

                }
                fs.Close();
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                Environment.Exit(1);
                fs.Close();
            }
        }
    }
}
