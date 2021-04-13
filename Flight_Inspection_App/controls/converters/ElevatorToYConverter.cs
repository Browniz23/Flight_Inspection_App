using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Flight_Inspection_App.controls.converters
{
    public class ElevatorToYConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                double y = (double) value;
                
                return -y * 50;
            }
            catch
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (int)value - 60;
        }
    }
}
