
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
using System.Runtime.InteropServices;
using Flight_Inspection_App.viewModel;




namespace Flight_Inspection_App.Graphs
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ShowGraph1 : UserControl
    {
        private GraphViewModel viewModel;

        public ShowGraph1()
        {
            viewModel = new GraphViewModel();
            DataContext = viewModel;

            CompositionTarget.Rendering += CompositionTargetRendering;
            stopwatch.Start();

            InitializeComponent();
        }

        private System.Diagnostics.Stopwatch stopwatch = new Stopwatch();

        private void CompositionTargetRendering(object sender, EventArgs e)
        {
            if (!viewModel.isConnectSet() || viewModel.vm_Stop)
            {
                stopwatch.Stop();
            }
            else
            {
                if (!stopwatch.IsRunning)
                    stopwatch.Start();
                // update graphs info
                viewModel.UpdateDateLine(viewModel.PlotModel, viewModel.ChosenChunk, ref viewModel.lastUpdate);
                viewModel.UpdateDateLine(viewModel.PlotModel_corr, viewModel.CorrelatedChunk, ref viewModel.lastUpdateCorr);
                viewModel.updateRegLine();
                // if 800 milliseconds passed - refresh graphs graphic.
                if (stopwatch.ElapsedMilliseconds > 800)
                { 
                    Plot1.RefreshPlot(true);
                    Plot_corr.RefreshPlot(true);
                    Plot_reg.RefreshPlot(true);
                    stopwatch.Restart();
                }
            }
        }
        // set connect model
        internal void setConnect(Connect c)
        {
            viewModel.setConnect(c);
            listbox.ItemsSource = c.ChunkName;
        }

        private void ListBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}