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
        private PlotModel plotModel;
        private Connect connectModel;
        private string chosenChunk;
        private string correlatedChunk;
        public DateTime startDate;
        public DateTime lastUpdate;
        public DateTime lastUpdateCorr;
        private int currline = 0;

        private PlotModel plotModel_reg;
        private PlotModel plotModel_corr;

        public const int LINE_PER_SEC = 10;
        public const int MS_PER_LINE = 100;
        public const int LAST_POINTS = 30;

        public GraphViewModel()
        {
            PlotModel = new PlotModel();
            SetUpModel(PlotModel);
            plotModel_corr = new PlotModel();
            SetUpModel(plotModel_corr);
            plotModel_reg = new PlotModel();
            setUpModel_reg();
            this.startDate = DateTime.MinValue;
            this.lastUpdate = DateTime.MinValue;
            this.lastUpdateCorr = DateTime.MinValue;
        }

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
        internal Connect ConnectModel                   // added
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
            get                                                 // maybe doesnt need
            {
                if (this.chosenChunk != null)
                {
                    return this.chosenChunk;
                }
                return vm_ChunkName[0];
            }
            // when choose another - if we started the run (means there is data) - starts from begining
            set
            {
                if (value != null && this.chosenChunk != value)
                {
                    this.chosenChunk = value;
                    this.correlatedChunk = connectModel.Settings.Chunks[chosenChunk].CorrChunk;     // can starts with dump value

                    if (!vm_Stop || vm_currLine > 1)            // todo:check currline addition!!1
                    {
                        PlotModel.Axes.Clear();
                        PlotModel_reg.Axes.Clear();
                        PlotModel_corr.Axes.Clear();
                        PlotModel.Series.Clear();
                        PlotModel_reg.Series.Clear();
                        PlotModel_corr.Series.Clear();
                        LoadData(plotModel, ChosenChunk, ref this.lastUpdate);
                        if (correlatedChunk != "none")
                        {
                            LoadData(plotModel_corr, correlatedChunk, ref lastUpdateCorr);
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
                //if (currline != connectModel.currLine - 1 && currline != connectModel.currLine)      
                if (currline > connectModel.currLine || currline + 50 < connectModel.currLine)              // changed to 50 from 1
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
                    LoadData(plotModel, ChosenChunk, ref lastUpdate);
                    if (correlatedChunk != "none")
                    {
                        LoadData(plotModel_corr, correlatedChunk, ref lastUpdateCorr);
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
            // no need for set
        }       // needed?
        internal void setConnect(Connect c)
        {
            this.connectModel = c;
            // updates (first or second time?) first chunk to be chosen
            //changed now!           //this.ChosenChunk = c.ChunkName[0];      // only this or loadData needed?    needed?????????????????????????
            //this.vm_ChunkName = c.ChunkName;
            this.connectModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("vm_" + e.PropertyName);
            };
            // LoadData(); // neededd??????????????????????????????????????????????????????????
            LoadData(plotModel, ChosenChunk, ref lastUpdate);
            correlatedChunk = c.Settings.Chunks[ChosenChunk].CorrChunk;
            if (correlatedChunk != "none")
            {
                LoadData(plotModel_corr, correlatedChunk, ref lastUpdateCorr);
                loadScatter();
            }
        }
        internal bool isConnectSet()
        {
            return this.connectModel != null;
        }



        private readonly List<OxyColor> colors = new List<OxyColor>
                                        {
                                            OxyColors.Green,
                                            OxyColors.IndianRed,
                                            OxyColors.Coral,
                                            OxyColors.Chartreuse,
                                            OxyColors.Azure
                                        };

        private readonly List<MarkerType> markerTypes = new List<MarkerType>
                                                {
                                                    MarkerType.Plus,
                                                    MarkerType.Star,
                                                    MarkerType.Diamond,
                                                    MarkerType.Triangle,
                                                    MarkerType.Cross,
                                                    MarkerType.None
                                                };


        private void SetUpModel(PlotModel plot)
        {
            plot.LegendTitle = "Chunk:";
            plot.LegendOrientation = LegendOrientation.Vertical;
            plot.LegendPlacement = LegendPlacement.Inside;
            plot.LegendPosition = LegendPosition.TopRight;
            plot.LegendBackground = OxyColor.FromAColor(200, OxyColors.AliceBlue);
            plot.LegendBorder = OxyColors.Chocolate;
            plot.IsLegendVisible = true;
            plot.LegendTitleFontSize = 9;                                                                                                               // maybe change this?


        }

        private void setUpModel_reg()
        {
            plotModel_reg.LegendTitle = "Correlation";
            plotModel_reg.LegendOrientation = LegendOrientation.Vertical;
            plotModel_reg.LegendPlacement = LegendPlacement.Inside;
            plotModel_reg.LegendPosition = LegendPosition.TopRight;
            plotModel_reg.LegendBackground = OxyColor.FromAColor(200, OxyColors.AliceBlue);
            plotModel_reg.LegendBorder = OxyColors.Chocolate;
            plotModel_reg.LegendTitleFontSize = 9;
        }
        private void loadScatter()
        {
            double min1 = connectModel.Settings.Chunks[ChosenChunk].Values.Min(), max1 = connectModel.Settings.Chunks[ChosenChunk].Values.Max();
            double min2 = connectModel.Settings.Chunks[correlatedChunk].Values.Min(), max2 = connectModel.Settings.Chunks[correlatedChunk].Values.Max();
            var chunkAxisX = new LinearAxis(AxisPosition.Bottom, 0) { Minimum = min1, Maximum = max1, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 80, Title = "Chunk" };
            var chunkAxisY = new LinearAxis(AxisPosition.Left, 0) { Minimum = min2, Maximum = max2, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Corroleated Chunk" };

            Data.CreateScatterLine(plotModel_reg, connectModel.Settings.Chunks[ChosenChunk].Values.ToList(), connectModel.Settings.Chunks[CorrelatedChunk].Values.ToList(), 1, connectModel.lineLength - 2, false);

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

            Data.CreateLinearLine(plotModel_reg, valuesX, valuesY, "correlation");

            plotModel_reg.Axes.Add(chunkAxisX);
            plotModel_reg.Axes.Add(chunkAxisY);

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
        private void LoadData(PlotModel plot, string chunk, ref DateTime updated)
        {
            DateTime min1 = new DateTime(), max1 = min1.AddMilliseconds(MS_PER_LINE * this.connectModel.lineLength);
            updated = min1;
            double min2 = connectModel.Settings.Chunks[chunk].Values.Min();
            double max2 = connectModel.Settings.Chunks[chunk].Values.Max();                       // need to do each 10 seconds  
            var dateAxis = new DateTimeAxis(AxisPosition.Bottom, "Time") { StringFormat = "mm:ss", Minimum = DateTimeAxis.ToDouble(min1), Maximum = DateTimeAxis.ToDouble(max1), MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalType = DateTimeIntervalType.Seconds, IntervalLength = 1 }; // MajorTickSize = 10};

            plot.Axes.Add(dateAxis);
            var valueAxis = new LinearAxis(AxisPosition.Left, 0) { Minimum = min2, Maximum = max2, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Value" };
            plot.Axes.Add(valueAxis);

            //int size = vm_currLine;
            int size = vm_currLine-1;                            //HEREEEEEE
            if (size == connectModel.lineLength)
                size--;
            if (size == connectModel.lineLength-1)
                size--;
            Data.CreateDateLine(plot, ConnectModel.Settings.Chunks[chunk].Values.ToList(), size, updated, chunk);

            updated = startDate.AddMilliseconds((vm_currLine-1) * MS_PER_LINE);
        }

        public void updateRegLine()
        {
            if (CorrelatedChunk != "none" && vm_currLine < connectModel.lineLength - 1) // TODO: CHAGNED TO BE < -1
            {
                Data.UpdateScatterLine(PlotModel_reg, connectModel.Settings.Chunks[ChosenChunk].Values.ToList(), connectModel.Settings.Chunks[correlatedChunk].Values.ToList(), vm_currLine);
            }
        }
        public void UpdateModel(PlotModel plot, string chunk, ref DateTime updated)
        {
            if (chunk != "none" && vm_currLine < connectModel.lineLength - 1)          // TODO: CHAGNED TO BE < -1
            {
                Data.UpdateDateLine(plot, this.connectModel.Settings.Chunks[chunk].Values.ToList(), vm_currLine, updated);
                updated = startDate.AddMilliseconds((vm_currLine-1) * MS_PER_LINE);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        //           [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}