using System;
using System.Collections.Generic;
using NSwag.Collections;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Flight_Inspection_App.viewModel
{
    internal class joystickPanelViewModel : INotifyPropertyChanged
    {

        //fields
        private Connect connectModel;

        //***property***///
  
        public double vm_throttle
        {
            get {
                if (connectModel == null) return 0;
                else return connectModel.Throttle;
            }
        }

        public double vm_rudder
        {
            get { if (connectModel == null) return 0;
                else return connectModel.Rudder;
            }
        }
        //*****////

        //CTOR
        public joystickPanelViewModel() {
        } // maybe add empty model


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
    }
}
