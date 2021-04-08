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
        public testViewModel()//Settings model)
        {
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

        public void setConnect(Connect c)
        {
            this.connectModel = c;
        }
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
