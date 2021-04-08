using NSwag.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Flight_Inspection_App
{
    class testViewModel : INotifyPropertyChanged
    {
        //private Settings settingsModel;
        private testModel tm;
        private Connect connectModel;

        public List<float> vm_speeds
        {
            get; set;
        }

        public float vm_selectedSpeed
        {
            set
            {
                connectModel.timeToSleep =(int)( 100 / value);
                Console.WriteLine(value);
            }
        }



        public testViewModel(testModel testM, Connect c)//Settings model)
        {
            //            this.settingsModel = model;
            this.connectModel = c;
            this.tm = testM;

            float[] speedArr = { 0.25f, 0.5f, 1.0f,2.0f,4.0f };
            vm_speeds = new List<float>(speedArr);
            vm_selectedSpeed = 1.0f;

            this.connectModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                notifyPropertyChanged("vm_" + e.PropertyName);
            };
        }
        /*public Dictionary<string, Chunk> vm_Chunks
        {
            get { return settingsModel.Chunks; }
            // no set for now
        }*/
        public int vm_currLine
        {
            get { return connectModel.currLine; }
            set { connectModel.currLine = value; }  // no needed!
        }

        public int vm_lineLength
        {
            get { return connectModel.lineLength; }
        }

        public ObservableDictionary<string, Chunk> vm_Chunks
        {
            get { return connectModel.Settings.Chunks; }
            set { connectModel.Settings.Chunks = value; }  // needed?
        }
        public int vm_X // can change model
        {
            get { return tm.X; }
            set { if (value != tm.X) 
                    tm.X = value;
            }
        }
        public int vm_Y // can change model
        {
            get { return tm.Y; }
            set
            {
                if (value != tm.Y)
                    tm.Y = value;
            }
        }

        public ObservableDictionary<int, int> vm_D
        {
            get { return tm.D; }
            set { tm.D = value; }
        }
        public List<int> vm_L {get { return tm.L; } set { tm.L = value; } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void notifyPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public void playPause()
        {
            connectModel.playPause();
        }
    }
}
