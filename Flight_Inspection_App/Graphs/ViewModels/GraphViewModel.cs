using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Flight_Inspection_App.viewModel;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;


namespace Flight_Inspection_App.Graphs
{
    public class GraphViewModel : INotifyPropertyChanged
    {
        //// fields ///// 
        // connectModel is the model.
        private PlotModel plotModel;
        private PlotModel plotModel_reg;
        private PlotModel plotModel_corr;
        private Connect connectModel;
        private string chosenChunk;
        private string correlatedChunk;
        public DateTime startDate;
        public DateTime lastUpdate;
        public DateTime lastUpdateCorr;
        private int currline = 0;
        // constants
        public const int LINE_PER_SEC = 10;
        public const int MS_PER_LINE = 100;
        public const int LAST_POINTS = 30;

        public GraphViewModel()
        {
            PlotModel = new PlotModel();
            plotModel_corr = new PlotModel();
            plotModel_reg = new PlotModel();
            this.startDate = DateTime.MinValue;
            this.lastUpdate = DateTime.MinValue;
            this.lastUpdateCorr = DateTime.MinValue;
        }

        //// properties
        public PlotModel PlotModel_reg
        {
            get { return plotModel_reg; }
            set { PlotModel_reg = value; NotifyPropertyChanged("PlotModel_reg"); }
        }
        public PlotModel PlotModel
        {
            get { return plotModel; }
            set { plotModel = value; NotifyPropertyChanged("PlotModel"); }
        }
        public PlotModel PlotModel_corr
        {
            get { return plotModel_corr; }
            set { PlotModel_corr = value; NotifyPropertyChanged("PlotModel_corr"); }
        }
        internal Connect ConnectModel                  
        {
            get { return connectModel; }
        }
        public int TimeToSleep
        {
            get
            {
                if (connectModel != null)
                    return connectModel.timeToSleep;
                return MS_PER_LINE;     // defalut 100
            }
            // no need for set
        }
        public string[] vm_ChunkName
        {
            get
            {
                if (isConnectSet())
                    return connectModel.ChunkName;
                return new string[] { "no chunks" };
            }
        }

        public string ChosenChunk
        {
            // first chosen as defalut
            get                                               
            {
                if (this.chosenChunk != null)
                {
                    return this.chosenChunk;
                }
                return vm_ChunkName[0];
            }
            // sets asked chosen chunk. realod appropriate graphs.
            set
            {
                if (value != null && this.chosenChunk != value)
                {
                    this.chosenChunk = value;
                    this.correlatedChunk = connectModel.Settings.Chunks[chosenChunk].CorrChunk;     

                    if (!vm_Stop || vm_currLine > 1)            
                    {
                        PlotModel.Axes.Clear();
                        PlotModel_reg.Axes.Clear();
                        PlotModel_corr.Axes.Clear();
                        PlotModel.Series.Clear();
                        PlotModel_reg.Series.Clear();
                        PlotModel_corr.Series.Clear();
                        LoadDate(plotModel, ChosenChunk, ref this.lastUpdate);
                        // if no correlative only one graph is shown.
                        if (correlatedChunk != "none")
                        {
                            LoadDate(plotModel_corr, correlatedChunk, ref lastUpdateCorr);
                            loadScatter();
                        }
                    }
                }
            }
        }
        public string CorrelatedChunk
        {
            get
            {
                return correlatedChunk;
            }
            set
            {
                correlatedChunk = value;
            }
        }

        public bool vm_Stop
        {
            get
            {
                return connectModel.Stop;
            }
        }

        public int vm_currLine
        {
            // update currline and load data again if jumped with bar backward or forward
            get
            {
                // gets current line. if changed dramaticlly (and even a bit backwards) reaload graph.
                if (currline > connectModel.currLine || currline + 50 < connectModel.currLine)            
                {
                    currline = connectModel.currLine;
                    PlotModel.Axes.Clear();
                    PlotModel_reg.Axes.Clear();
                    PlotModel_corr.Axes.Clear();
                    PlotModel.Series.Clear();
                    PlotModel_reg.Series.Clear();
                    PlotModel_corr.Series.Clear();
                    lastUpdate = startDate.AddMilliseconds(MS_PER_LINE * currline);
                    lastUpdateCorr = startDate.AddMilliseconds(MS_PER_LINE * currline);
                    LoadDate(plotModel, ChosenChunk, ref lastUpdate);
                    // if no correlative only one graph is shown.
                    if (correlatedChunk != "none")
                    {
                        LoadDate(plotModel_corr, correlatedChunk, ref lastUpdateCorr);
                        loadScatter();
                    }
                }
                currline = connectModel.currLine;
                return connectModel.currLine;
            }
        }
        internal Settings Settings
        {
            get { return connectModel.Settings; }
        }

        //// set model! get Connect and load graphs.
        internal void setConnect(Connect c)
        {
            this.connectModel = c;
            this.connectModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("vm_" + e.PropertyName);
            };
            LoadDate(plotModel, ChosenChunk, ref lastUpdate);
            correlatedChunk = c.Settings.Chunks[ChosenChunk].CorrChunk;
            // if no correaltive only one graph.
            if (correlatedChunk != "none")
            {
                LoadDate(plotModel_corr, correlatedChunk, ref lastUpdateCorr);
                loadScatter();
            }
        }
        internal bool isConnectSet()
        {
            return this.connectModel != null;
        }

        private void loadScatter()
        {
            // calculates min and max values for axes.
            double min1 = connectModel.Settings.Chunks[ChosenChunk].Values.Min(), max1 = connectModel.Settings.Chunks[ChosenChunk].Values.Max();
            double min2 = connectModel.Settings.Chunks[correlatedChunk].Values.Min(), max2 = connectModel.Settings.Chunks[correlatedChunk].Values.Max();
            var chunkAxisX = new LinearAxis(AxisPosition.Bottom, 0) { Minimum = min1, Maximum = max1, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 80, Title = "Chunk" };
            var chunkAxisY = new LinearAxis(AxisPosition.Left, 0) { Minimum = min2, Maximum = max2, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Corroleated" };
            plotModel_reg.Title = "regression line";
            plotModel_reg.TitleFontSize = 10;
            //****** all dots line *******
            Data.CreateScatterLine(plotModel_reg, connectModel.Settings.Chunks[ChosenChunk].Values.ToList(), connectModel.Settings.Chunks[CorrelatedChunk].Values.ToList(), 1, connectModel.lineLength - 2, false);
            // data for regression line
            double a = connectModel.Settings.Chunks[ChosenChunk].lin_reg.a, b = connectModel.Settings.Chunks[ChosenChunk].lin_reg.b;
            List<double> valuesX = new List<double>();
            List<double> valuesY = new List<double>();
            valuesX.Add(min1);
            valuesX.Add(max1);
            valuesY.Add(min1 * a + b);
            valuesY.Add(max1 * a + b);
            if (min2 > min1 * a + b)
                min2 = min1 * a + b;
            if (max2 < max1 * a + b)
                max2 = max1 * a + b;
            double spaceY = (max2 - min2) / 80;
            min2 -= spaceY; max2 += spaceY;
            chunkAxisX.Minimum = min1; chunkAxisX.Maximum = max1;
            chunkAxisY.Minimum = min2; chunkAxisY.Maximum = max2;
            //****** linear regression line *******
            Data.CreateLinearLine(plotModel_reg, valuesX, valuesY);
            // add updated axes.
            plotModel_reg.Axes.Add(chunkAxisX);
            plotModel_reg.Axes.Add(chunkAxisY);

            //****** updated 30 second points *******
            int start = 1, end = vm_currLine -1;
            if (connectModel.currLine > LAST_POINTS * LINE_PER_SEC)            // 30 seconds when 10 lines per sec
            {
                start = connectModel.currLine - LAST_POINTS * LINE_PER_SEC;
            }
            if (end == ConnectModel.lineLength)
                end--;
            if (end == ConnectModel.lineLength - 1)
                end--;
            Data.CreateScatterLine(plotModel_reg, connectModel.Settings.Chunks[ChosenChunk].Values.ToList(), connectModel.Settings.Chunks[CorrelatedChunk].Values.ToList(), start, end, true);
        }
        // load DateLine data. for chosen chunk graph, and for its correlative if exists (for each).
        private void LoadDate(PlotModel plot, string chunk, ref DateTime updated)
        {
            // min and max values for axes
            DateTime min1 = new DateTime(), max1 = min1.AddMilliseconds(MS_PER_LINE * this.connectModel.lineLength);
            updated = min1;
            double min2 = connectModel.Settings.Chunks[chunk].Values.Min();
            double max2 = connectModel.Settings.Chunks[chunk].Values.Max();
            double spaceY = (max2 - min2) / 80;
            min2 -= spaceY; max2 += spaceY;
            var dateAxis = new DateTimeAxis(AxisPosition.Bottom, "Time") { StringFormat = "mm:ss", Minimum = DateTimeAxis.ToDouble(min1), Maximum = DateTimeAxis.ToDouble(max1), MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalType = DateTimeIntervalType.Seconds, IntervalLength = 1 }; // MajorTickSize = 10};
            plot.Title = chunk;
            plot.TitleFontSize = 10;
            plot.Axes.Add(dateAxis);
            var valueAxis = new LinearAxis(AxisPosition.Left, 0) { Minimum = min2, Maximum = max2, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Value" };
            plot.Axes.Add(valueAxis);

            int size = vm_currLine-1;                           
            if (size == connectModel.lineLength)
                size--;
            if (size == connectModel.lineLength-1)
                size--;
            //********* creates DateLine ************
            Data.CreateDateLine(plot, ConnectModel.Settings.Chunks[chunk].Values.ToList(), size, updated);

            updated = startDate.AddMilliseconds((vm_currLine-1) * MS_PER_LINE);
        }

        //// updates regression line.
        public void updateRegLine()
        {
            if (CorrelatedChunk != "none" && vm_currLine < connectModel.lineLength - 1) 
            {
                double valx = connectModel.getValue(ChosenChunk), valy = connectModel.getValue(correlatedChunk);
                Data.UpdateScatterLine(PlotModel_reg, valx, valy, vm_currLine);
            }
        }
        //// updates Dateline.
        public void UpdateDateLine(PlotModel plot, string chunk, ref DateTime updated)
        {
            if (chunk != "none" && vm_currLine < connectModel.lineLength - 1)          
            {
                Data.UpdateDateLine(plot, this.connectModel.Settings.Chunks[chunk].Values.ToList(), vm_currLine, updated);
                updated = startDate.AddMilliseconds((vm_currLine-1) * MS_PER_LINE);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}