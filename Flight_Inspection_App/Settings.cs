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
    class Settings : INotifyPropertyChanged 
    {
        private string XMLFileName;
      //  public ArrayList values;
        //public List<Chunk> chunks;
        private ObservableDictionary<string, Chunk> chunks;
        private ObservableDictionary<string, int> namesCount;
        public ObservableDictionary<string, Chunk> Chunks
        {
            get { return this.chunks; }
            set { NotifyPropertyChanged("Chunks"); chunks = value; }         // needed?
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

        public void UploadSettings()          // need to throw exception  
        {
            using (XmlReader reader = XmlReader.Create(@XMLFileName))           // need @?
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
                                // maybe should add idx field in Chunk? (count)
                                break;
                            case "input":           // added
                                stop = true;
                                break;
                        }
                    }
                    Console.WriteLine("");
                }
            }
            //Console.ReadKey();        does error
        }

    }
}
