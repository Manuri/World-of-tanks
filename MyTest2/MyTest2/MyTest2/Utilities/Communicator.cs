using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Threading;

namespace MyTest2.Utilities
{
    class Communicator
    {
        private static Communicator instance;
        private string serverIP = "127.0.0.1";
       // private string serverIP = "192.168.137.253";
        private string clientIP = "127.0.0.1";
      //  private string clientIP = "192.168.137.1";

        private Communicator() { }

        public static Communicator getCommunicator
        {
            get
            {
                if (instance == null)
                {
                    instance = new Communicator();
                }
                return instance;
            }
        }

        public string ServerIP
        {
            set { serverIP = value; }
            get { return serverIP; }

        }

        public string ClientIP
        {
            set { clientIP = value; }
            get { return clientIP; }

        }


        public void send(String request)
        {

            #region sending

            try
            {
                TcpClient tcpClient = new TcpClient();


                tcpClient.Connect(serverIP, 6000);


                NetworkStream outStream = tcpClient.GetStream();

                BinaryWriter writer = new BinaryWriter(outStream);
                ASCIIEncoding asci = new ASCIIEncoding();
                Byte[] ba = asci.GetBytes(request);
                writer.Write(ba);


                tcpClient.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error :" + e);

            }
        }

            #endregion sending

        #region recieving
        public void receive()
        {

            IPAddress ip = IPAddress.Parse(clientIP);
            
            
            while (true)
            {
                TcpListener listener = new TcpListener(ip, 7000);

                listener.Start();

                Socket s = listener.AcceptSocket();
                if (s.Connected)
                {
                    NetworkStream inputStream = new NetworkStream(s);
                    StreamReader reader = new StreamReader(inputStream);

                    try
                    {

                        String line = reader.ReadLine();

                        line = line.Remove(line.Length - 1, 1);

                        ThreadPool.QueueUserWorkItem(new WaitCallback(GameManager.getGameManager.decodeMessage), (Object)line);

                        reader.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Server feed read error!!!");
                    }
                }

                listener.Stop();


            }

        }
        #endregion recieving

    }
}
