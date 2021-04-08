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
                NotifyPropertyChanged("vm_" + e.PropertyName);
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
            set { NotifyPropertyChanged("vm_Chunks"); connectModel.Settings.Chunks = value; }  // needed?
        }
       

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string name)
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
