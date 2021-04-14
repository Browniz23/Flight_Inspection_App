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
    /// Interaction logic for ErorControl.xaml
    /// </summary>
    public partial class ErorControl : UserControl
    {
        private ErorControlViewModel ecvm;
        public ErorControl()
        {
            InitializeComponent();
            ecvm = new ErorControlViewModel();
            this.DataContext = ecvm;
        }

        internal void setConnect(Connect c)
        {
            ecvm.setConnect(c);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            bool? response = openFileDialog.ShowDialog();
            if (response == true)
            {
                String dllPath= openFileDialog.FileName;
               listbox.ItemsSource = ecvm.detect(dllPath);
                
            }
        }

      

        private void selected(object sender, SelectionChangedEventArgs e)
        {
            ecvm.vm_currLine = (string) listbox.SelectedItem;
        }
    }
}
