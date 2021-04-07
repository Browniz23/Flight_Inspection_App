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
       // private List<string> properties;
      //  public ArrayList values;
        //public List<Chunk> chunks;
        private ObservableDictionary<string, Chunk> chunks;
        private ObservableDictionary<string, int> namesCount;
        public ObservableDictionary<string, Chunk> Chunks
        {
            get { return this.chunks; }
            set { chunks = value; }         // needed?
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void notifyPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        /*  // <!-- Flight Controls -->
private float aileron;
private float elevator;
private float rudder;
private float flaps;
private float slats;
private float speedbrake;
// <!-- Engines -->
private float throttle1;    //1
private float throttle2;    //2
// <!-- Gear -->
// <!-- Hydraulics -->
private float engine_pump;  // _ instead of -
private float engine-pump;
private float electric-pump;
private float electric-pump;
// <!-- Electric -->
private float external-power;
private float APU-generator;
// <!-- Autoflight -->
// <!-- Position -->
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;
private float aileron;*/

        public Settings(string XML_fileName)
        {
            this.XMLFileName = XML_fileName;
            //    properties = new List<string>();
            //   values = new ArrayList();
            this.chunks = new ObservableDictionary<string, Chunk>();

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

         /*   string[] lines = File.ReadAllLines(this.XMLFileName);
            for (int i = 0; i < lines.Length; i++)
            {
                int idx1 = lines[i].IndexOf("<name>");
                if (idx1 >= 0)
                {
                    int idx2 = lines[i].IndexOf("</name>", idx1);
                    string name = lines[i].Substring(idx1 + 6, idx2-idx1-6);
                  //  properties.Add(name);
                  //  values.Add((float) 0);         // do casting??
            //        chunks.Add(new Chunk(name, ))
                }
            }*/
        }

    }
}
