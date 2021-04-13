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
    /// Interaction logic for joystickControl.xaml
    /// </summary>
    public partial class joystickControl : UserControl
    {
        private joystickViewModel jvm;
        public joystickControl()
        {
            InitializeComponent();
            jvm = new joystickViewModel();
            this.DataContext = jvm;
        }

        internal void setConnect(Connect c)
        {
            jvm.setConnect(c);
        }
    }

    public class ElevatorToYStickConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (int)value * 60;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (int)value / 60;
        }
    }
}
