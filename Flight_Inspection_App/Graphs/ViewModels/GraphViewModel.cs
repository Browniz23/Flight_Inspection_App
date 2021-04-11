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
        private Connect connectModel;               // maybe dont need because GraphControl
        private string chosenChunk;
        private DateTime startDate;
        //private long startDate;
        private int currline = 0;
        // GraphControlViewModel graphicViewModel;

        public GraphViewModel()
        {
            PlotModel = new PlotModel();
            SetUpModel();
            this.startDate = DateTime.Now;//new DateTime();
            this.lastUpdate = DateTime.Now;//new DateTime();
        }

        internal Connect ConnectModel                   // added
        {
            get { return connectModel; }
        }
        public PlotModel PlotModel
        {
            get { return plotModel; }
            set { plotModel = value; NotifyPropertyChanged("PlotModel"); }
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
            {   if (this.chosenChunk != value) 
                {
                    this.chosenChunk = value;

                    if (!vm_Stop || vm_currLine > 0)            // todo:check currline addition!!1
                    {
                      //  PlotModel = new PlotModel();            // not sure if true! doesnt work!!!
                       // SetUpModel();
                        PlotModel.Series.Clear();

                        LoadData();
                    }                         
                }
            }
        }
        public bool vm_Stop
        {
            // returns stop. but in first time also load data when start
            get 
            {
                if (!connectModel.Stop && vm_currLine == 0) // maybe doesnt need only first time?// && connectModel.currLine == 0)       // not sure...
                {
                    // need to be only first time!
                    //              this.startDate = DateTime.Now;              // todo:gives exception when there is no breakpoint!
                    //this.startDate = 0;
                    //this.startDate = TimeSpan.Zero; // needed?
                    //         if (datetime == DateTime.MinValue)
                    //datetime = new DateTime(0, 0, 0, 0, 0, 0, 0);
                 
                    //this.startDate = new DateTime();
                    //this.lastUpdate = new DateTime();                 both moved to ctor
                    LoadData();
                }
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
                    PlotModel.Series.Clear();
                    LoadData();
                }
                currline = connectModel.currLine;
                return connectModel.currLine; 
            }
        }
        internal Settings Settings
        {
            get { return connectModel.Settings; }
            // no need for set
        }
        internal void setConnect(Connect c)
        {
            this.connectModel = c;
            // updates (first or second time?) first chunk to be chosen
            this.ChosenChunk = c.ChunkName[0];      // only this or loadData needed?    needed?????????????????????????
            //this.vm_ChunkName = c.ChunkName;
            this.connectModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("vm_" + e.PropertyName);
            };
           // LoadData(); // neededd??????????????????????????????????????????????????????????
        }
        internal bool isConnectSet()
        {
            return this.connectModel != null;
        }

        private DateTime lastUpdate;// = DateTime.MinValue;        //min value?
        //private long lastUpdate = 0;
        //private TimeSpan lastUpdate = TimeSpan.Zero;


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


        private void SetUpModel()
        {
            PlotModel.LegendTitle = "Chunks";
            PlotModel.LegendOrientation = LegendOrientation.Vertical;
            PlotModel.LegendPlacement = LegendPlacement.Outside;
            PlotModel.LegendPosition = LegendPosition.TopRight;
            PlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.AliceBlue);
            PlotModel.LegendBorder = OxyColors.Chocolate;
            // added
            PlotModel.LegendTitleFontSize = 9;
            var dateAxis = new TimeSpanAxis(AxisPosition.Bottom, "Time", "mm:ss") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 80 };
            PlotModel.Axes.Add(dateAxis);
            var valueAxis = new LinearAxis(AxisPosition.Left, 0) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Value" };
            PlotModel.Axes.Add(valueAxis);

        }

        private void LoadData()
        {
            List<Measurement> measurements = Data.GetData(this.connectModel, ChosenChunk, this.startDate);

            var dataPerDetector = measurements.GroupBy(m => m.DetectorId).OrderBy(m => m.Key).ToList();
            foreach (var data in dataPerDetector)
            {
                string name = ChosenChunk;
                if (data.Key != 0 && !vm_Stop)
                    name = "correlative:\n"+connectModel.Settings.Chunks[ChosenChunk].CorrChunk;
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
                data.ToList().ForEach(d => lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.dateTime), d.Value)));
                PlotModel.Series.Add(lineSerie);
            }

            //lastUpdate = DateTime.Now;
            //lastUpdate = startDate.AddMilliseconds(vm_currLine * 100);
            lastUpdate = startDate.AddMilliseconds(vm_currLine * 100);
        }

        public void UpdateModel()
        {
            List<Measurement> measurements = Data.GetUpdateData(lastUpdate, this.connectModel, ChosenChunk);
            var dataPerDetector = measurements.GroupBy(m => m.DetectorId).OrderBy(m => m.Key).ToList();

            foreach (var data in dataPerDetector)
            {
                var lineSerie = PlotModel.Series[data.Key] as LineSeries;          
                if (lineSerie != null)
                {
                    data.ToList()
                        .ForEach(d => lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.dateTime), d.Value)));
                    //.ForEach(d => lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.DateTime), d.Value)));
                }
            }
            //lastUpdate = DateTime.Now;
            //lastUpdate = startDate.AddMilliseconds(vm_currLine * 100);
            lastUpdate = startDate.AddMilliseconds(vm_currLine * 100);
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
