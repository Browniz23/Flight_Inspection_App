using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using OxyPlot;


namespace Flight_Inspection_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Settings s;
        Connect c;
        ControlScreen cs;
        string regualr_CSV_path;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_XML(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            bool? response = openFileDialog.ShowDialog();
            if (response == true)
            {
                String filePath = openFileDialog.FileName;
                if (filePath.EndsWith("xml"))
                {
                    s = new Settings(filePath);
                    s.UploadSettings();
                    X.Visibility = Visibility.Hidden;
                    V.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Please choose a XML file");
                }
            }
        }


        private void regularFlightButtonClick(object sender, RoutedEventArgs e) 
        {
            if (X.Visibility == Visibility.Hidden) // needs to be only if settings are on
            {
                // open file dialog so user can pick a CSV file.
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                bool? response = openFileDialog.ShowDialog();
                if (response == true)
                {
                    String filePath = openFileDialog.FileName;
                    if (filePath.EndsWith("csv"))
                    {
                        X_Copy.Visibility = Visibility.Hidden;
                        V_Copy.Visibility = Visibility.Visible;
                        regualr_CSV_path = filePath;
                    }
                    else
                    {
                        MessageBox.Show("Please choose a CSV file");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please insert XMl settings before uploading CSV file");
            }
        }

        private void testFlightButtonClick(object sender, RoutedEventArgs e) 
        {
            if (X.Visibility == Visibility.Hidden)
            {
                if (X_Copy.Visibility == Visibility.Hidden) // needs to be only if settings are on
                {
                    // open file dialog so user can pick a CSV file.
                    Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                    bool? response = openFileDialog.ShowDialog();
                    if (response == true)
                    {
                        String filePath = openFileDialog.FileName;
                        if (filePath.EndsWith("csv"))
                        {
                            X_Copy1.Visibility = Visibility.Hidden;
                            V_Copy1.Visibility = Visibility.Visible;
                            c = new Connect(regualr_CSV_path, filePath, s);
                        }
                        else
                        {
                            MessageBox.Show("Please choose a CSV file");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please insert CSV reg_flight before uploading CSV test flight");
                }
            }
            else
            {
                MessageBox.Show("Please insert XMl settings before uploading CSV file");
            }
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            c.ExecuteClient();
            cs = new ControlScreen();
            cs.setConnect(c);
            cs.Show();
            this.Close();
        }
    }
}