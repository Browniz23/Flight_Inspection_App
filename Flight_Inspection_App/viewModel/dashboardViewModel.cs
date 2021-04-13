using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Inspection_App.viewModel
{
    class dashboardViewModel : INotifyPropertyChanged
    {

        //fields
        private Connect connectModel;

        //***property***//

        public double vm_Height{ get { return connectModel.Height; } }
        public double vm_AirSpeed{ get { return connectModel.AirSpeed; } }
        public double vm_FlightDirection{ get { return connectModel.FlightDirection; } }
        public double vm_Yaw{ get { return connectModel.Yaw; } }
        public double vm_Pitch{ get { return connectModel.Pitch; } }
        public double vm_Roll{ get { return connectModel.Roll; } }




        //***//

        //CTOR
        public dashboardViewModel()
        {
            this.connectModel = new Connect(" ", new Settings(" "));
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
    }
}
