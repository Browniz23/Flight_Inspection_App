using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Windows;

namespace Flight_Inspection_App
{
    class Connect : INotifyPropertyChanged
    {
        private string CSVFileName;
        private Settings settings;
        private int currLine;
        public int CurrLine { get { return currLine; } set { currLine = value; } }
        public string CSV_Name { get { return CSVFileName; } set { CSVFileName = value; } }
        public Settings Settings { get { return settings; } set { settings = value; } }
        public Connect(String CSV_fileName, Settings settings)
        {
            this.CSVFileName = CSV_fileName;
            this.settings = settings;

            NotifyPropertyChanged("CSVFileName");
            NotifyPropertyChanged("settings");

            currLine = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public void ExecuteClient(String CSVFileName)
        {
            new Thread(delegate ()
            {
            try
            {
                string[] lines = File.ReadAllLines(CSVFileName);

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
                    // added update

                    string[] separetedLine = lines[i].Split(',');
                    for (int j = 0; j < this.settings.Chunks.Count - 1; j++)
                    {
                            if (this.settings.Chunks.ElementAt(j).Value.IsFloat)
                                this.settings.Chunks.ElementAt(j).Value.Values.Add(float.Parse(separetedLine[j])); //maybe need to casr differently
                                //this.settings.Chunks.ElementAt(j).Value.Values.AddOnUI(float.Parse(separetedLine[j]));
                            //Application.Current.Dispatcher.BeginInvoke(new Action(() => this.settings.Chunks.ElementAt(j).Value.Values.Add(float.Parse(separetedLine[j]))));
                            else
                                this.settings.Chunks.ElementAt(j).Value.Values.Add(double.Parse(separetedLine[j])); //maybe need to casr differently
                        }
                        CurrLine = i;

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

            }).Start();
        }
    }
}
