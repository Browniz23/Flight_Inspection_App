using NSwag.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Flight_Inspection_App
{
    // class saves dictionary of all chunks got from xml settings file.
    class Settings : INotifyPropertyChanged 
    {
        //// fields
        private string XMLFileName;
        private ObservableDictionary<string, Chunk> chunks;         // dictionary which observable, saves all chunks.
        private ObservableDictionary<string, int> namesCount;

        ///// properties
        public ObservableDictionary<string, Chunk> Chunks
        {
            get { return this.chunks; }
            set { NotifyPropertyChanged("Chunks"); chunks = value; }        
        }

        public string[] chunksName { get { return Chunks.Keys.ToArray(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public Settings(string XML_fileName)
        {
            this.XMLFileName = XML_fileName;
            this.Chunks = new ObservableDictionary<string, Chunk>();

            this.namesCount = new ObservableDictionary<string, int>();
        }

        // read xml file and update chunks names and types in Chunks 
        public void UploadSettings()           
        {
            using (XmlReader reader = XmlReader.Create(@XMLFileName))         
            {
                bool stop = false;
                string name = "", type = "";
                while (reader.Read() && !stop)
                {
                    if (reader.IsStartElement())
                    {
                        //return only when you have START tag
                        string x = reader.Name.ToString();
                        switch (x)
                        {
                            case "name":
                                name = reader.ReadString();
                                if (chunks.ContainsKey(name))
                                {
                                    namesCount[name]++;
                                    name += namesCount[name];
                                }
                                namesCount.Add(name, 0);
                                break;
                            case "type":
                                type = reader.ReadString();
                                Chunk c = new Chunk(name, type);
                                chunks.Add(name, c);
                                break;
                            case "input":           
                                stop = true;
                                break;
                        }
                    }
                    Console.WriteLine("");
                }
            }
        }

    }
}
