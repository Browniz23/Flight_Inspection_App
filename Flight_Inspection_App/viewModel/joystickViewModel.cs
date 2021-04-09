using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Inspection_App.viewModel
{
    class joystickViewModel : INotifyPropertyChanged
    {
        private Connect connectModel;

        public joystickViewModel()//Settings model)
        {
           
            //added!!!!!!!!!!!!!!!!!!!
            this.connectModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("vm_" + e.PropertyName);
            };
        }

        public void setConnect(Connect c)
        {
            this.connectModel = c;
        }

        public void NotifyPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public double vm_throttle
        {
            get { return this.connectModel.getValue("throttle"); }
           
        }
    }
}
