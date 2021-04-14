using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Inspection_App.Graphs
{
    public class Data
    {
        // constants
        public const int MS_PER_LINE = 100;
        public const int LAST_POINTS = 30;
        public const int LINE_PER_SEC = 10;

        //// function gets a plot and values for to create a dateLine on plot.
        internal static void CreateDateLine(PlotModel p, List<double> values, int size, DateTime start)
        {
            var lineSerie = new LineSeries
            {
                StrokeThickness = 2,
                MarkerSize = 3,
                MarkerStroke = OxyColors.DarkGreen, 
                Color = OxyColors.Wheat,
                CanTrackerInterpolatePoints = false,
                Smooth = false,
            };
            for (int j = 0; j <= size; j++)        
            {                                      
                lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(start.AddMilliseconds(MS_PER_LINE * j)), values[j]));
            }
            p.Series.Add(lineSerie);
        }

        //// function create a ScatterLine. got plot and values.
        internal static void CreateScatterLine(PlotModel p, List<double> xValues, List<double> yValues, int start, int end, bool changeing)
        {
            var dots = new LineSeries();
            dots.MarkerType = MarkerType.Circle;
            dots.StrokeThickness = 0;
           
            if (!changeing)
            {
                dots.MarkerSize = 1.2;
                dots.MarkerStroke = OxyColors.ForestGreen;
            }
            else
            {
                dots.MarkerSize = 1.4;
                dots.MarkerStroke = OxyColors.Black;
            }
            for (int i = start; i <= end; i++)
            {
                dots.Points.Add(new DataPoint(xValues[i], yValues[i]));
            }
            p.Series.Add(dots);
        }

        // create linearLine on plotModel.
        internal static void CreateLinearLine(PlotModel p, List<double> xValues, List<double> yValues)
        {
            var lineSerie = new LineSeries
            {
                StrokeThickness = 2,
                MarkerSize = 3,
                MarkerStroke = OxyColors.Salmon,                   
                CanTrackerInterpolatePoints = false,
                Smooth = false,
            };
            for (int j = 0; j < xValues.Count; j++)        
            {                                                                           
                lineSerie.Points.Add(new DataPoint(xValues[j], yValues[j]));
            }
            p.Series.Add(lineSerie);
        }

        //get list of values to update DateLine on plot (update first line in plot).
        internal static void UpdateDateLine(PlotModel p, List<double> values, int idx, DateTime updated)
        {
            var line = p.Series[0] as LineSeries;
            line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(updated.AddMilliseconds(100)), values[idx]));
        }

        //get list of values to update Scatters on plot (update third line).
        // removes past 30 sec points and add new.
        internal static void UpdateScatterLine(PlotModel p, double xValues, double yValues, int idx)
        {
            var line = p.Series[2] as LineSeries;
            if (idx > LAST_POINTS * LINE_PER_SEC)
            {
                line.Points.RemoveAt(0);
            }
            line.Points.Add(new DataPoint(xValues, yValues));
        }

    }
}