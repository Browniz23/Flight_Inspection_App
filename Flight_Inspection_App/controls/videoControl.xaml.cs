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
    /// Interaction logic for UserControl2.xaml
    /// </summary>
    public partial class videoControl : UserControl
    {
        private videoControlsViewModel vcvm;

        public videoControl()
        {
            InitializeComponent();
            vcvm = new videoControlsViewModel();
            this.DataContext = vcvm;
        }

        internal void setConnect(Connect c)
        {
            vcvm.setConnect(c);
        }

        private void playPauseButton_Click(object sender, RoutedEventArgs e)
        {
            vcvm.playPause();  //TODO mvvm
        }

        private void startAgain_Click(object sender, RoutedEventArgs e)
        {
            vcvm.startAgain();
        }

    }
}
