using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Inspection_App.viewModel
{
    internal class joystickViewModel : INotifyPropertyChanged
    {

        //fields
        private Connect connectModel;

        //***property***///

        public double vm_elevator
        {
            get
            {
                if (connectModel == null) return 0;
                else return connectModel.Elevator;
            }
        }

        public double vm_aileron
        {
            get
            {
                if (connectModel == null) return 0;
                else return connectModel.Aileron;
            }
        }
        //*****////

        //CTOR
        public joystickViewModel()
        {
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
