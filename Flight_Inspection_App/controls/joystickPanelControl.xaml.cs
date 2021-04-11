using Flight_Inspection_App.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Flight_Inspection_App.controls
{
    /// <summary>
    /// Interaction logic for joystickPanelControl.xaml
    /// </summary>
    public partial class joystickPanelControl : UserControl
    {
        private joystickPanelViewModel jpvm;
        public joystickPanelControl()
        {
            InitializeComponent();
            jpvm = new joystickPanelViewModel();
            this.DataContext = jpvm;
        }

        internal void setConnect(Connect c)
        {
            jpvm.setConnect(c);
            joystick.setConnect(c);
        }

        private void joystickControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
