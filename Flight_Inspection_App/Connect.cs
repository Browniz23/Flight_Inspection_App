﻿using System;
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
using System.Runtime.InteropServices;

namespace Flight_Inspection_App
{
    class Connect : INotifyPropertyChanged
    {

        // ****model field*****//
        private string CSVFileName;
        private Settings settings;
        private int currline;
        private int linelength;
        private int timetosleep;
        private bool stop;
        private double airspeed;
        private string[] chunkName;
        private float minCorr = 0.9F;               // todo: add button to change corr
        //***//


        //****model property****///
        public int currLine { get { return currline; } 
            set 
            {
                currline = value;
                NotifyPropertyChanged("CurrLine");
            }
        }

        public double AirSpeed { get { return airspeed; } 
            set
            {
                airspeed = value;
                NotifyPropertyChanged("AirSpeed");

            }
        }

        public int timeToSleep
        {
            get { return timetosleep; }
            set
            {
                this.timetosleep = value;
                NotifyPropertyChanged("timeToSleep");

            }
        }

        public int lineLength { get { return linelength; }
            set
            {
                linelength = value;
                NotifyPropertyChanged("lineLength");
            }
        }

        public string[] ChunkName { get { return chunkName; }
            set
            {
                chunkName = value;
                NotifyPropertyChanged("ChunkName");
            }
        }

        public string CSV_Name { get { return CSVFileName; } set { CSVFileName = value; } }
        public Settings Settings { get { return settings; } set { settings = value; } }
        //***///

        //CTOR
        public Connect(String CSV_fileName, Settings settings)
        {
            this.CSVFileName = CSV_fileName;
            this.settings = settings;
            currLine = 0;
            timeToSleep = 100;
            stop = false;
        }

        //MVVM pattern
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        //***//


        // mvvm function
        public void playPause()
        {
            this.stop = this.stop ? false : true;
        }

        public double getValue(String s)
        {
            if (settings != null)
            {
                bool keyEx = settings.Chunks.ContainsKey(s);
                if (keyEx)
                    return settings.Chunks[s].CurrValue;
            }
        
            return 0;
        }
        //***//

        public bool isHighCorr(float corr)
        {
            return (corr > 0 && corr >= minCorr) || (corr < 0 && corr <= -minCorr);
        }

        [DllImport("Data_Process.dll")]
        public static extern float Pearson(float[] x, float[] y, int size);
        public void updateCorrelation()
        {
            int size = 0;
            if (settings != null)
                size = settings.Chunks.ElementAt(0).Value.Values.Count;
            for (int i = 0; i < this.settings.chunksName.Length; i++)
            {
                float maxCorr = 0;
                string bestMatch = "none";
                for (int j = i + 1; j < this.settings.chunksName.Length; j++)
                {
                    double[] firstC = settings.Chunks[chunkName[i]].Values.ToArray();
                    double[] secondC = settings.Chunks[chunkName[j]].Values.ToArray();
                    float[] floatArray1 = Array.ConvertAll(firstC, x => (float)x);
                    float[] floatArray2 = Array.ConvertAll(firstC, x => (float)x);
                    float pear = Pearson(floatArray1, floatArray2, size);
                    if (Math.Abs(pear) > maxCorr && isHighCorr(pear))
                    {
                        maxCorr = pear;
                        bestMatch = chunkName[j];
                    }
                }
                settings.Chunks[chunkName[i]].CorrChunk = bestMatch;
                settings.Chunks[chunkName[i]].Correlation = maxCorr;
            }
        }

        // main thread, send data to FG
        public void ExecuteClient(String CSVFileName)
        {
            string[] lines = File.ReadAllLines(CSVFileName);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] separetedLine = lines[i].Split(',');
                for (int j = 0; j < separetedLine.Length; j++)
                {
                    if (this.settings.Chunks.ElementAt(j).Value.IsFloat)
                        this.settings.Chunks.ElementAt(j).Value.Values.Add(float.Parse(separetedLine[j])); //maybe need to casr differently
                    else
                        this.settings.Chunks.ElementAt(j).Value.Values.Add(double.Parse(separetedLine[j])); //maybe need to casr differently
                }
                //Console.WriteLine(i);
            }
            ChunkName = this.Settings.chunksName;
//            updateCorrelation();

            new Thread(delegate ()
            {
                try
                {
                    // Create a TcpClient.
                    // Note, for this client to work you need to have a TcpServer
                    // connected to the same address as specified by the server, port
                    // combination.
                    Int32 port = 5400;
                    TcpClient client = new TcpClient("127.0.0.1", port);

                    // Get a client stream for reading and writing.
                    //  Stream stream = client.GetStream();
                    NetworkStream stream = client.GetStream();
                    lineLength = lines.Length;

                    while (true)  
                    {
                        // if stop = TRUE, or the csv end, stop send lines from soket.
                        if(stop || this.currline == this.linelength)
                        {
                            Thread.Sleep(500);
                            continue;
                        }

                        // Translate the passed message into ASCII and store it as a Byte array.
                        Byte[] data = System.Text.Encoding.ASCII.GetBytes(lines[currLine] + "\n");

                        // Send the message to the connected TcpServer.
                        stream.Write(data, 0, data.Length);

                        // JUST FOR NOW: prints all lines in console.
                        Console.WriteLine("Sent: {0}", lines[currLine]);
                        // added update

                        string[] separetedLine = lines[currLine].Split(',');
                        for (int j = 0; j < separetedLine.Length; j++)    // this.settings.Chunks.Count -1?
                        {
                           if (this.settings.Chunks.ElementAt(j).Value.IsFloat)
                           {
                             //   this.settings.Chunks.ElementAt(j).Value.Values.Add(float.Parse(separetedLine[j])); //maybe need to casr differently
                             this.settings.Chunks.ElementAt(j).Value.CurrValue = float.Parse(separetedLine[j]);
                           }
                           else
                           {
                             // this.settings.Chunks.ElementAt(j).Value.Values.Add(double.Parse(separetedLine[j])); //maybe need to casr differently
                             this.settings.Chunks.ElementAt(j).Value.CurrValue = double.Parse(separetedLine[j]);
                           }
                        }


                        AirSpeed = getValue("airspeed-kt");

                        stream.Flush();              // TODO: needed? also works without it.
                        Thread.Sleep(timeToSleep);
                        currLine++;
                            
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

