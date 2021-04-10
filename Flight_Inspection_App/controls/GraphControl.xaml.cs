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
using Flight_Inspection_App.viewModel;


namespace Flight_Inspection_App.controls
{
    /// <summary>
    /// Interaction logic for GraphControl.xaml
    /// </summary>
    public partial class GraphControl : UserControl
    {
        private GraphControlViewModel gcvm;

        public GraphControl()
        {
            InitializeComponent();
            gcvm = new GraphControlViewModel();
            this.DataContext = gcvm;

        }

        internal void setConnect(Connect c)
        {
            gcvm.setConnect(c);
        }

        private void GraphControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
