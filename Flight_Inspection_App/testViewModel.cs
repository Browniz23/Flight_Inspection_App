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
        private testModel tm;
        //        private Settings settingsModel;
        private Connect connectModel;
        public testViewModel(testModel testM, Connect c)//Settings model)
        {
            //            this.settingsModel = model;
            this.connectModel = c;
            this.tm = testM;
            this.tm.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("vm_" + e.PropertyName);
            };
            //added!!!!!!!!!!!!!!!!!!!
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
        public int vm_CurrLine
        {
            get { return connectModel.CurrLine; }
            set { NotifyPropertyChanged("vm_CurrLine"); connectModel.CurrLine = value; }  // no needed!
        }
        public ObservableDictionary<string, Chunk> vm_Chunks
        {
            get { return connectModel.Settings.Chunks; }
            set { NotifyPropertyChanged("vm_Chunks"); connectModel.Settings.Chunks = value; }  // needed?
        }
        public int vm_X // can change model
        {
            get { return tm.X; }
            set { if (value != tm.X)
                {
                    NotifyPropertyChanged("vm_X");
                    tm.X = value;
                } 
            }
        }
        public int vm_Y // can change model
        {
            get { return tm.Y; }
            set
            {
                if (value != tm.Y)
                {
                    NotifyPropertyChanged("vm_Y");
                    tm.Y = value;
                }
            }
        }

        public ObservableDictionary<int, int> vm_D
        {
            get { return tm.D; }
            set { NotifyPropertyChanged("vm_D"); tm.D = value; }
        }
        public List<int> vm_L {get { return tm.L; } set { tm.L = value; } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
