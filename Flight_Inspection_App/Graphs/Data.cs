﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Inspection_App.Graphs
{
    public class Data
    {
        internal static List<Measurement> GetData(Connect c, string chosenChunk)
        {
            var measurements = new List<Measurement>();

            var startDate = DateTime.Now;//.AddMinutes(-10);       

            int PointsNum = c.currLine;
            if (PointsNum == 0)
                PointsNum++;
            for (int j = 0; j < PointsNum; j++)        // -1?
            {                                                                           // todo: get string as parameter/bind
                measurements.Add(new Measurement() { DetectorId = 0, DateTime = startDate.AddMinutes(j), Value = c.Settings.Chunks[chosenChunk].Values[j] });    // c.Settings.Chunks["throttle"].Values[j]
            }
            string corrChosenChunk = c.Settings.Chunks[chosenChunk].CorrChunk;
            for (int j = 0; j < PointsNum; j++)        // cooraltive
            {
                measurements.Add(new Measurement() { DetectorId = 1, DateTime = startDate.AddMinutes(j), Value = c.Settings.Chunks[corrChosenChunk].Values[j] });   // c.Settings.Chunks["pitch-deg"].Values[j]
            }
            measurements.Sort((m1, m2) => m1.DateTime.CompareTo(m2.DateTime));
            return measurements;
        }

        internal static List<Measurement> GetUpdateData(DateTime dateTime, Connect c, string chosenChunk)
        {
            var measurements = new List<Measurement>();
            measurements.Add(new Measurement() { DetectorId = 0, DateTime = dateTime.AddSeconds(1), Value = c.getValue(chosenChunk) });  // Value = c.getValue("throttle") 
            string corrChosenChunk = c.Settings.Chunks[chosenChunk].CorrChunk;
            measurements.Add(new Measurement() { DetectorId = 1, DateTime = dateTime.AddSeconds(1), Value = c.getValue(corrChosenChunk) }); // Value = c.getValue("pitch-deg") 
            return measurements;
        }
    }

    public class Measurement
    {
        public int DetectorId { get; set; }
        public double Value { get; set; }
        public DateTime DateTime { get; set; }
    }
}
