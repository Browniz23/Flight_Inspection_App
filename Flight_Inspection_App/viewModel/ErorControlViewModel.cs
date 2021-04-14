using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Inspection_App.viewModel
{
    class ErorControlViewModel : INotifyPropertyChanged
    {
        //fields
        private Connect connectModel;
        private List<string> erorList;

        //prop
        public List<string> vm_ErorList
        {
            get { return this.erorList; }
            set { this.erorList = value; }   
        }

        public string vm_currLine
        {
            set {
                if (value != null)
                {
                    string[] valueSplit = value.Split(',');
                    connectModel.currLine = int.Parse(valueSplit[0]);
                }
            }
        }


        //CTOR
        public ErorControlViewModel()
        {
            this.connectModel = new Connect(" ", " ", new Settings(" "));
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

        public List<string> detect(string dllPath)
        {
            return connectModel.detect(dllPath);
        }
    }
}
