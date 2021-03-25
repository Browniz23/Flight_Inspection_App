using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

using System.IO;
using System.Threading;


namespace Flight_Inspection_App
{
    static class Connect
    {
        public static void ExecuteClient(String CSVFileName)
        {
            string[] lines = File.ReadAllLines(CSVFileName);


            try
            {
                //test

                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 5400;
                TcpClient client = new TcpClient("127.0.0.1", port);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();
                NetworkStream stream = client.GetStream();

                for (int i = 0; i < lines.Length; i++)
                {
                    // Translate the passed message into ASCII and store it as a Byte array.
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(lines[i] + "\n");

                    // Send the message to the connected TcpServer.
                    stream.Write(data, 0, data.Length);

                    // JUST FOR NOW: prints all lines in console.
                    Console.WriteLine("Sent: {0}", lines[i]);

                    stream.Flush();              // TODO: needed? also works without it.
                    Thread.Sleep(100);
                }

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
    }
}
