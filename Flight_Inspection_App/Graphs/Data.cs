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
        public const int MS_PER_LINE = 100;
        public const int LAST_POINTS = 30;
        public const int LINE_PER_SEC = 10;
        internal static void CreateDateLine(PlotModel p, List<double> values, int size, DateTime start, string title)
        {
            var lineSerie = new LineSeries
            {
                StrokeThickness = 2,
                MarkerSize = 3,
                MarkerStroke = OxyColors.DarkGreen,                   // color always green!
                                                                      //MarkerType = markerTypes[5],
                CanTrackerInterpolatePoints = false,
                //Title = string.Format(name),
                Title = title,   // need to switch to name from measure?
                                 //    Title = measurements[0].Name, 
                Smooth = false,
            };
            for (int j = 0; j <= size; j++)        // -1?
            {                                                                             // todo: change from minutes to..??    
                                                                                          //measurements.Add(new Measurement() { DetectorId = 0, dateTime = start.AddMilliseconds(100 * j), Value = values[j] });
                lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(start.AddMilliseconds(MS_PER_LINE * j)), values[j]));
            }
            p.Series.Add(lineSerie);
        }

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

        internal static void CreateLinearLine(PlotModel p, List<double> xValues, List<double> yValues, string title)
        {
            var lineSerie = new LineSeries
            {
                StrokeThickness = 2,
                MarkerSize = 3,
                MarkerStroke = OxyColors.DarkGreen,                   // color always green!
                                                                      //MarkerType = markerTypes[5],
                CanTrackerInterpolatePoints = false,
                //Title = string.Format(name),
                Title = title,   // need to switch to name from measure?
                                 //    Title = measurements[0].Name, 
                Smooth = false,
            };
            for (int j = 0; j < xValues.Count; j++)        // -1?
            {                                                                             // todo: change from minutes to                        //measurements.Add(new Measurement() { DetectorId = 0, dateTime = start.AddMilliseconds(100 * j), Value = values[j] });
                lineSerie.Points.Add(new DataPoint(xValues[j], yValues[j]));
            }
            p.Series.Add(lineSerie);
        }

        internal static void UpdateDateLine(PlotModel p, List<double> values, int idx, DateTime updated)
        {
            var line = p.Series[0] as LineSeries;
            line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(updated.AddMilliseconds(100)), values[idx]));
        }

        internal static void UpdateScatterLine(PlotModel p, List<double> xValues, List<double> yValues, int idx)
        {
            var line = p.Series[2] as LineSeries;
            if (idx > LAST_POINTS * LINE_PER_SEC)
            {
                line.Points.RemoveAt(0);
            }
            line.Points.Add(new DataPoint(xValues[idx], yValues[idx]));
        }

    }
}
