using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Inspection_App.Graphs
{
    public class Data
    {
        internal static List<Measurement> GetData(Connect c, string chunk, DateTime d)
        {
            var measurements = new List<Measurement>();

            //var startDate = DateTime.Now;//.AddMinutes(-10);       
         //   var startDate = new DateTime(2000,1,1,0,0,0,0);

            int PointsNum = c.currLine;
            if (PointsNum == 0)
                PointsNum++;
            for (int j = 0; j < PointsNum; j++)        // -1?
            {                                                                             // todo: change from minutes to..??    
                measurements.Add(new Measurement() { DetectorId = 0, dateTime = d.AddMilliseconds(100 * j), Value = c.Settings.Chunks[chunk].Values[j] });   
                //measurements.Add(new Measurement() { DetectorId = 0, dateTime = d.AddMilliseconds(100 * j), Value = c.Settings.Chunks[chunk].Values[j] });    // c.Settings.Chunks["throttle"].Values[j]
            }
            /*string corrChosenChunk = c.Settings.Chunks[chosenChunk].CorrChunk;
            if (corrChosenChunk != "none")
            {
                for (int j = 0; j < PointsNum; j++)        // cooraltive
                {                                                                                       // c.timeToSleep
                    measurements.Add(new Measurement() { DetectorId = 1, dateTime = d.AddMilliseconds(100 * j), Value = c.Settings.Chunks[corrChosenChunk].Values[j] });   // c.Settings.Chunks["pitch-deg"].Values[j]
                }
                measurements.Sort((m1, m2) => m1.dateTime.CompareTo(m2.dateTime));
            } else
            {
                // need to show in textBox?
            }*/
            return measurements;
        }

        internal static List<Measurement> GetUpdateData(DateTime d, Connect c, string chunk)
        {
            var measurements = new List<Measurement>();                                 // addMilies chaged from c.timeToSleep
            measurements.Add(new Measurement() { DetectorId = 0, dateTime = d.AddMilliseconds(100), Value = c.getValue(chunk), Name = chunk });  // Value = c.getValue("throttle") 
           /* string corrChosenChunkForName = "correlative:\n";
            string corrChosenChunk = c.Settings.Chunks[chosenChunk].CorrChunk;
            corrChosenChunkForName += corrChosenChunk;
            if (corrChosenChunk != "none")
                measurements.Add(new Measurement() { DetectorId = 1, dateTime = d.AddMilliseconds(100), Value = c.getValue(corrChosenChunk), Name = corrChosenChunkForName }); // Value = c.getValue("pitch-deg") */
            return measurements;
        }
    }

    public class Measurement
    {
        public int DetectorId { get; set; }
        public double Value { get; set; }
        //private DateTime datetime;// = DateTime.MinValue;
       // public DateTime dateTime { get; set; }
        public DateTime dateTime
        {
            get;
            /*get
            {
                if (dateTime == DateTime.MinValue)
                    dateTime = new DateTime(2000,1,1,0,0,0,0);
                return dateTime;
            }*/
            set;
        }
        //DateTime Axis?

        public string Name { get; set; }            // maybe doesnt need?!
    }
}
