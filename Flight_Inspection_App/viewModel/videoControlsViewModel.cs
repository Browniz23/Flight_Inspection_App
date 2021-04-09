using NSwag.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Flight_Inspection_App.viewModel
{
    internal class videoControlsViewModel : INotifyPropertyChanged
    {

        //fields
        private Connect connectModel;

        //***property***///
        public List<float> vm_speeds
        {
            get; set;
        }

        public float vm_selectedSpeed
        { set { connectModel.timeToSleep = (int)(100 / value); } }
     
        public int vm_currLine
        {
            get { return connectModel.currLine; }
            set { connectModel.currLine = value; } 
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
        //*****////

        //CTOR
        public videoControlsViewModel()
        {
            this.connectModel = new Connect(" ", new Settings(" "));

            float[] speedArr = { 0.25f, 0.5f, 1.0f, 2.0f, 4.0f };
            vm_speeds = new List<float>(speedArr);
            vm_selectedSpeed = 1.0f;

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
        //**//


        public void setConnect(Connect c)
        {
            this.connectModel = c;
            this.connectModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("vm_" + e.PropertyName);
            };
        }
        public void playPause()
        {
            connectModel.playPause();
        }
    }
}
