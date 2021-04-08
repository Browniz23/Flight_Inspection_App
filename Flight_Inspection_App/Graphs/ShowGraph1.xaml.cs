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
using System.Diagnostics;


namespace Flight_Inspection_App.Graphs
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ShowGraph1 : UserControl
    {
        private MainWindowModel viewModel;      // doesnt need ViewModels.?

        public ShowGraph1()
        {
            viewModel = new MainWindowModel();    // doesnt need ViewModels.?
            DataContext = viewModel;

            CompositionTarget.Rendering += CompositionTargetRendering;
            stopwatch.Start();

            InitializeComponent();
        }

        private long frameCounter;
        private System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
        private long lastUpdateMilliSeconds;

        private void CompositionTargetRendering(object sender, EventArgs e)
        {
            //if (stopwatch.ElapsedMilliseconds > lastUpdateMilliSeconds + 100)   //was 5000  need to be speed of or
            if (stopwatch.ElapsedMilliseconds > lastUpdateMilliSeconds + viewModel.timeToSleep)
            {
                viewModel.UpdateModel();
                Plot1.RefreshPlot(true);
                lastUpdateMilliSeconds = stopwatch.ElapsedMilliseconds;
            }
        }
    }
}
