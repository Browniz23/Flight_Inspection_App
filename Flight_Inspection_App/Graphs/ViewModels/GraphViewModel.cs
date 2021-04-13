using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Flight_Inspection_App.viewModel;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
//using OxyPlotDemo.Annotations;

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

        public GraphViewModel()
        {
            PlotModel = new PlotModel();
            SetUpModel(PlotModel);
            plotModel_corr = new PlotModel();
            SetUpModel(plotModel_corr);
            this.startDate = DateTime.MinValue;//new DateTime();
            this.lastUpdate = DateTime.MinValue;//new DateTime();
            this.lastUpdateCorr = DateTime.MinValue;
            plotModel_reg = new PlotModel();
            setUpModel_reg();
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
        // need mvvm fix
        public int TimeToSleep
        {
            get 
            { if (connectModel != null) 
                   return connectModel.timeToSleep;
                return 100;     // defalut 100
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
            {   if (this.chosenChunk != null) 
                {
                    return this.chosenChunk;
                }
                return vm_ChunkName[0];
            }
            // when choose another - if we started the run (means there is data) - starts from begining
            set 
            {   if (value != null && this.chosenChunk != value) 
                {
                    this.chosenChunk = value;
                    this.correlatedChunk = connectModel.Settings.Chunks[chosenChunk].CorrChunk;     // can starts with dump value

                    if (!vm_Stop || vm_currLine > 0)            // todo:check currline addition!!1
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
                            loadScatter();      // here also?
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
            // returns stop. but in first time also load data when start
            get 
            {
                //if (!connectModel.Stop && vm_currLine == 0)
                //{
                    // need to be only first time!
                   
                    //this.startDate = new DateTime();
                    //this.lastUpdate = new DateTime();                 both moved to 
                //}
                return connectModel.Stop; 
            }
        }
        public int vm_currLine
        {
            // update currline and load data again if jumped with bar backward or forward
            get 
            {
                //if (currline != connectModel.currLine - 1 && currline != connectModel.currLine)      
                if (currline > connectModel.currLine)
                {
                    currline = connectModel.currLine;
                    PlotModel.Axes.Clear();
                    PlotModel_reg.Axes.Clear();
                    PlotModel_corr.Axes.Clear();
                    PlotModel.Series.Clear();
                    PlotModel_reg.Series.Clear();
                    PlotModel_corr.Series.Clear();
                    LoadData(plotModel, ChosenChunk, ref lastUpdate);
                    if (correlatedChunk != "none")
                    {
                        LoadData(plotModel_corr, correlatedChunk, ref lastUpdateCorr);
                        loadScatter();          // here?
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
            //var dateAxis = new DateTimeAxis(AxisPosition.Bottom, "Time", "mm:ss") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 80 };

/*            var dateAxis = new LinearAxis(AxisPosition.Bottom, "Time") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 80 };
            plot.Axes.Add(dateAxis);
            var valueAxis = new LinearAxis(AxisPosition.Left, 0) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Value" };
            plot.Axes.Add(valueAxis);*/

        }

        private void setUpModel_reg()
        {
            plotModel_reg.LegendTitle = "Correlation";
            plotModel_reg.LegendOrientation = LegendOrientation.Vertical;
            plotModel_reg.LegendPlacement = LegendPlacement.Inside;
            plotModel_reg.LegendPosition = LegendPosition.TopRight;
            plotModel_reg.LegendBackground = OxyColor.FromAColor(200, OxyColors.AliceBlue);
            plotModel_reg.LegendBorder = OxyColors.Chocolate;
            // added
            plotModel_reg.LegendTitleFontSize = 9;
            /*var chunkAxisX = new LinearAxis(AxisPosition.Bottom, 0) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 80, Title = "Chunk"};
            plotModel_reg.Axes.Add(chunkAxisX);
            var chunkAxisY = new LinearAxis(AxisPosition.Left, 0) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Corroleated Chunk" };
            plotModel_reg.Axes.Add(chunkAxisY);*/
        }
        private void loadScatter()
        {
            double min1 = connectModel.Settings.Chunks[ChosenChunk].Values.Min(), max1 = connectModel.Settings.Chunks[ChosenChunk].Values.Max();
            double min2 = connectModel.Settings.Chunks[correlatedChunk].Values.Min(), max2 = connectModel.Settings.Chunks[correlatedChunk].Values.Max();
            //todo: maybe need to choose minimum for axes according to linreg also? ( minimum(f(min1), min2) . maximum(f(max2), max2).
            var chunkAxisX = new LinearAxis(AxisPosition.Bottom, 0) { Minimum = min1, Maximum = max1, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 80, Title = "Chunk" };
            var chunkAxisY = new LinearAxis(AxisPosition.Left, 0) { Minimum = min2, Maximum = max2, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Corroleated Chunk" };
            // adding axes at bottom after inserting min\max
            var s1 = new LineSeries
            {
                StrokeThickness = 0,
                MarkerSize = 1.2,
                MarkerStroke = OxyColors.ForestGreen,
                MarkerType = MarkerType.Circle,
            };
            for (int i = 0; i < connectModel.lineLength; i++)
            {
                s1.Points.Add(new DataPoint(connectModel.Settings.Chunks[ChosenChunk].Values[i], connectModel.Settings.Chunks[correlatedChunk].Values[i]));
            }
            plotModel_reg.Series.Add(s1);

            var lin_reg = new LineSeries
            {
                StrokeThickness = 1,
                MarkerSize = 0.3,
                MarkerStroke = OxyColors.GreenYellow,
                MarkerType = markerTypes[5],
                CanTrackerInterpolatePoints = false,
                Title = "reg_line",
                Smooth = false,
            };
            double a = connectModel.Settings.Chunks[ChosenChunk].lin_reg.a, b = connectModel.Settings.Chunks[ChosenChunk].lin_reg.b;
            //for (int i = ; i < 700; i++)
            //{
            //double x1 = min1, x2 = max1;
            //double y1 = a * x1 + b, y2 = a * x2 + b;
            double addition = (max1-min1) / 300, x = min1;
            double y = a * x + b; ;
            /*if (y > a * max1 + b)
            {
                chunkAxisY.Maximum = y;
            }*/
            while (x < max1)
            {
                x += addition;
                y = a * x + b;
                lin_reg.Points.Add(new DataPoint(x, y));
            }
            /*if (chunkAxisX.Minimum > lin_reg.MinX)
                chunkAxisX.Minimum = lin_reg.MinX;
            if (chunkAxisY.Minimum > lin_reg.MinY)
                chunkAxisY.Minimum = lin_reg.MinY;
            if (chunkAxisX.Maximum < lin_reg.MaxX)
                chunkAxisX.Maximum = lin_reg.MaxX;
            if (chunkAxisY.Maximum < lin_reg.MaxY)
                chunkAxisY.Maximum = lin_reg.MaxY;*/
            /*if (y < a*min1 + b)
            {
                chunkAxisY.Minimum = y;
            }*/
            plotModel_reg.Axes.Add(chunkAxisX);
            plotModel_reg.Axes.Add(chunkAxisY);
            //lin_reg.Points.Add(new DataPoint(x1, y1));
            //lin_reg.Points.Add(new DataPoint(x2, y2));
            //}
            plotModel_reg.Series.Add(lin_reg);

            var sCurrLasrPoints = new LineSeries
            {
                StrokeThickness = 0,
                MarkerSize = 1.4,
                MarkerStroke = OxyColors.Black,
                MarkerType = MarkerType.Circle,
            };
            int start = 0;
            if (connectModel.currLine > 30 * 10)            // 30 seconds when 10 lines per sec
            {
                start = connectModel.currLine - 30 * 10;
            }
            //todo: chage currline in connect so stop is true when got to end.
            for (int i = start; i <= vm_currLine; i++)
            {
                sCurrLasrPoints.Points.Add(new DataPoint(connectModel.Settings.Chunks[ChosenChunk].Values[i], connectModel.Settings.Chunks[correlatedChunk].Values[i]));
            }
            plotModel_reg.Series.Add(sCurrLasrPoints);
        }
        private void LoadData(PlotModel plot, string chunk, ref DateTime updated)
        {
            //int min1 = 0, max1 = this.connectModel.lineLength;
            DateTime min1 = new DateTime(), max1 = min1.AddMilliseconds(100 * this.connectModel.lineLength);
            updated = min1;
            double min2 = connectModel.Settings.Chunks[chunk].Values.Min();
            double max2 = connectModel.Settings.Chunks[chunk].Values.Max();                       // need to do each 10 seconds  
            var dateAxis = new DateTimeAxis(AxisPosition.Bottom, "Time") { StringFormat = "mm:ss", Minimum = DateTimeAxis.ToDouble(min1), Maximum = DateTimeAxis.ToDouble(max1), MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalType = DateTimeIntervalType.Seconds, IntervalLength = 1 }; // MajorTickSize = 10};

                                        
            //var dateAxis = new LinearAxis(AxisPosition.Bottom, "Time") { Minimum = min1, Maximum = max1, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 80 };
            plot.Axes.Add(dateAxis);
            var valueAxis = new LinearAxis(AxisPosition.Left, 0) { Minimum = min2, Maximum = max2, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Value" };
            plot.Axes.Add(valueAxis);

            List<Measurement> measurements = Data.GetData(this.connectModel, chunk, min1);
            var lineSerie = new LineSeries
            {
                StrokeThickness = 2,
                MarkerSize = 3,
                MarkerStroke = colors[0],                   // color always green!
                MarkerType = markerTypes[5],
                CanTrackerInterpolatePoints = false,
                //Title = string.Format(name),
                Title = chunk,   // need to switch to name from measure?
                                //    Title = measurements[0].Name, 
                Smooth = false,
            };
            for (int i = 0; i < measurements.Count; i++)
            {
                //lineSerie.Points.Add(new DataPoint(i, measurements[i].Value));
                lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(measurements[i].dateTime), measurements[i].Value));
            }
            plot.Series.Add(lineSerie);
            /*var dataPerDetector = measurements.GroupBy(m => m.DetectorId).OrderBy(m => m.Key).ToList(); 
            foreach (var data in dataPerDetector)
            {
                string name = chunk;
                var lineSerie = new LineSeries
                {
                    StrokeThickness = 2,
                    MarkerSize = 3,
                    MarkerStroke = colors[data.Key],
                    MarkerType = markerTypes[5],
                    CanTrackerInterpolatePoints = false,
                    //Title = string.Format(name),
                    Title = name,   // need to switch to name from measure?
                //    Title = measurements[0].Name, 
                    Smooth = false,
                };
                //data.ToList().ForEach(d => lineSerie.Points.Add(new DataPoint(count, d.Value)));
                data.ToList().ForEach(d => lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.dateTime), d.Value)));
                plot.Series.Add(lineSerie);
            }*/

            //lastUpdate = DateTime.Now;
            //lastUpdate = startDate.AddMilliseconds(vm_currLine * 100);
            updated = startDate.AddMilliseconds(vm_currLine * 100);
        }

        public void updateRegLine()
        {
            // if currline not at end. updtae at connect
            var series = plotModel_reg.Series[2] as LineSeries;
            if (vm_currLine > 30 * 10)
            {
                series.Points.RemoveAt(0);
            }
            series.Points.Add(new DataPoint(connectModel.Settings.Chunks[ChosenChunk].Values[vm_currLine], connectModel.Settings.Chunks[correlatedChunk].Values[vm_currLine]));
            /*if (connectModel.Settings.Chunks[ChosenChunk].CorrChunk != "none")
            {
                var scatterSerie = plotModel_reg.Series[0] as ScatterSeries;
            }*/
        }
        public void UpdateModel(PlotModel plot, string chunk, ref DateTime updated)
        {
            if (chunk != "none")
            {
                List<Measurement> measurements = Data.GetUpdateData(updated, this.connectModel, chunk);
                var dataPerDetector = measurements.GroupBy(m => m.DetectorId).OrderBy(m => m.Key).ToList();

                foreach (var data in dataPerDetector)
                {
                    var lineSerie = plot.Series[data.Key] as LineSeries;
                    if (lineSerie != null)
                    {
                        data.ToList().ForEach(d => lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.dateTime), d.Value)));
                        //data.ToList().ForEach(d => lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.dateTime), d.Value)));
                    }
                }
                //lastUpdate = DateTime.Now;
                //lastUpdate = startDate.AddMilliseconds(vm_currLine * 100);
                updated = startDate.AddMilliseconds(vm_currLine * 100);
                //updated = updated.AddMilliseconds(100);
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
