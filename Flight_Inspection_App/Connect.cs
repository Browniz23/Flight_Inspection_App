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
        private int currTime;
        private double throttle;
        private double rudder;
        private double elevator;
        private double aileron;
        private bool stop;
        private double height;
        private double airSpeed;
        private double flightDirection;
        private double yaw;
        private double roll;
        private double pitch;
        private string[] chunkName;
        private float minCorr = 0F;               // todo: add button to change corr
        private string[] lines;
        //***//


        //****model property****///
        ///////////////////////////////////// added + changed stop to be true at begining and false when thread starts.
        public bool Stop
        {
            get { return stop; }
            set
            {
                stop = value;
                NotifyPropertyChanged("Stop");
            }////////////////////////////////
        }
        public int currLine { get { return currline; } 
            set 
            {
                currline = value;
                CurrTime = value==0? 0: value / 10;
                NotifyPropertyChanged("CurrLine");
            }
        }

        public int CurrTime
        {
            get { return currTime; }
            set
            {
                currTime = value;
                NotifyPropertyChanged("CurrTime");
            }

        }

        public double Height
        {
            get => height;
            set
            {
                height = value;
                NotifyPropertyChanged("Height");
            }
        }

        public double AirSpeed
        {
            get => airSpeed;
            set
            {
                airSpeed = value;
                NotifyPropertyChanged("AirSpeed");
            }
        }

        public double FlightDirection
        {
            get => flightDirection;
            set
            {
                flightDirection = value;
                NotifyPropertyChanged("FlightDirection");
            }
        }

        public double Yaw
        {
            get => yaw;
            set
            {
                yaw = value;
                NotifyPropertyChanged("Yaw");
            }
        }

        public double Roll
        {
            get => roll;
            set
            {
                roll = value;
                NotifyPropertyChanged("Roll");
            }
        }

        public double Pitch
        {
            get => pitch;
            set
            {
                pitch = value;
                NotifyPropertyChanged("Pitch");
            }
        }

        public int timeToSleep
        {
            get => timetosleep;
            set
            {
                this.timetosleep = value;
                NotifyPropertyChanged("timeToSleep");
            }
        }

        public int lineLength
        {
            get { return linelength; }
            set
            {
                linelength = value;
                NotifyPropertyChanged("lineLength");
            }
        }

        public double Rudder { get { return rudder; }
            set
            {
                rudder = value;
                NotifyPropertyChanged("Rudder");
            }
        }

        public double Throttle { get { return throttle; }
            set
            {
                throttle = value;
                NotifyPropertyChanged("Throttle");
            }
        }

        public double Aileron { get { return aileron; }
            set
            {
                aileron = value;
                NotifyPropertyChanged("Aileron");
            }
        }

        public double Elevator
        {
            get { return elevator; }
            set
            {
                elevator = value;
                NotifyPropertyChanged("Elevator");
            }
        }

        public string CSV_Name { get { return CSVFileName; } set { CSVFileName = value; } }
        public Settings Settings { get { return settings; } set { settings = value; } }
        public string[] ChunkName
        {
            get { return chunkName; }
            set
            {
                chunkName = value;
                NotifyPropertyChanged("ChunkName");
            }
        }

       
        //***///

        //CTOR
        public Connect(String CSV_fileName, Settings settings)
        {
            this.CSVFileName = CSV_fileName;
            this.settings = settings;
            ChunkName = this.Settings.chunksName;
            currLine = 0;
            timeToSleep = 100;
            stop = true;
            if (CSVFileName != " ")
            {
                lines = File.ReadAllLines(CSVFileName);
                lineLength = lines.Length;
                updateData();
            }
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

        void updateData()
        {
            for (int i = 0; i < lines.Length; i++)
            {
                string[] separetedLine = lines[i].Split(',');
                for (int j = 0; j < separetedLine.Length; j++)
                {
                    if (this.settings.Chunks.ElementAt(j).Value.IsFloat)
                        this.settings.Chunks.ElementAt(j).Value.Values.Add(float.Parse(separetedLine[j])); 
                    else
                        this.settings.Chunks.ElementAt(j).Value.Values.Add(double.Parse(separetedLine[j])); 
                }
                //Console.WriteLine(i);
            }
            updateCorrelation();
        }
        // mvvm function
        /// <summary>
        /// function changed stop field.
        /// make the thread that send data to FG to stop or proceed.
        /// </summary>
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

        public bool isHighCorr(double corr)
        {
            return (corr > 0 && corr >= minCorr) || (corr < 0 && corr <= -minCorr);
        }
        /// <summary>
        /// function update all data that need to be
        /// update in every line send to FG.
        /// the data will be displayed on the dashboard.
        /// </summary>
        private void updateDashboardProperty()
        {
            Height = getValue("altitude-ft");
            AirSpeed = getValue("airspeed-kt");
            FlightDirection = getValue("heading-deg");
            Yaw = getValue("side-slip-deg");
            Roll = getValue("roll-deg");
            Pitch = getValue("pitch-deg");
        }

        /*[DllImport("Data_Process.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern float Pearson(float[] x, float[] y, int size);*/
        public void updateCorrelation()
        {
            int size = 0;
            ChunkName = this.Settings.chunksName;
            if (settings != null)
                size = settings.Chunks.ElementAt(0).Value.Values.Count;
            for (int i = 0; i < this.settings.chunksName.Length; i++)
            {
                double maxCorr = 0;
                string bestMatch = "none";
                for (int j = 0; j < this.settings.chunksName.Length; j++)
                {
                    if (j != i)
                    //corr 1 //        if (true)
                    {
                        double[] firstC = settings.Chunks[chunkName[i]].Values.ToArray();
                        double[] secondC = settings.Chunks[chunkName[j]].Values.ToArray();
                        double pear = probabilityLib.pearson(firstC, secondC);
                        if (Math.Abs(pear) > Math.Abs(maxCorr) && isHighCorr(pear))
                        {
                            maxCorr = pear;
                            bestMatch = chunkName[j];
                        }
                    }
                }
                settings.Chunks[chunkName[i]].CorrChunk = bestMatch;
                settings.Chunks[chunkName[i]].Correlation = maxCorr;
                if (bestMatch != "none")
                {
     //               settings.Chunks[bestMatch].CorrChunk = chunkName[i];
     //               settings.Chunks[bestMatch].Correlation = maxCorr;
                    settings.Chunks[chunkName[i]].lin_reg = probabilityLib.linearReg(settings.Chunks[chunkName[i]].Values.ToArray(),
                        settings.Chunks[bestMatch].Values.ToArray());
                } else
                {
                    //settings.Chunks[chunkName[i]].lin_reg = new Line(0, 0);           not really need
                }
            }
        }

        // main thread, send data to FG
        public void ExecuteClient()
        {
            ChunkName = this.Settings.chunksName;

            Stop = false;                           // update as property - start running

            new Thread(delegate()
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
                        if (stop || this.currline == this.linelength)
                        {
                            if (this.currline == this.linelength) Stop = true;
                            Thread.Sleep(500);
                            continue;
                        }

                        // Translate the passed message into ASCII and store it as a Byte array.
                        Byte[] data = System.Text.Encoding.ASCII.GetBytes(lines[currLine] + "\n");

                        // Send the message to the connected TcpServer.
                        stream.Write(data, 0, data.Length);

                        // added update

                        string[] separetedLine = lines[currLine].Split(',');
                        for (int j = 0; j < separetedLine.Length; j++) // this.settings.Chunks.Count -1?
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

                        updateDashboardProperty();
                        Throttle = getValue("throttle");
                        Rudder = getValue("rudder");
                        Aileron = getValue("aileron");
                        Elevator = getValue("elevator");
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