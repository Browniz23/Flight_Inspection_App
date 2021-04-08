using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Inspection_App.Graphs
{
    public class Data
    {
        public static List<Measurement> GetData()
        {
            var measurements = new List<Measurement>();

            var startDate = DateTime.Now;//.AddMinutes(-10);       
            var r = new Random();

            // for (int i = 0; i < 5; i++)
            // {
            for (int j = 0; j < 1; j++)
            {
                measurements.Add(new Measurement() { DetectorId = 0, DateTime = startDate.AddMinutes(j), Value = r.Next(1, 4) });
            }
            for (int j = 0; j < 1; j++)
            {
                measurements.Add(new Measurement() { DetectorId = 1, DateTime = startDate.AddMinutes(j), Value = r.Next(10, 30) });
            }
            //  }
            measurements.Sort((m1, m2) => m1.DateTime.CompareTo(m2.DateTime));
            return measurements;
        }

        public static List<Measurement> GetUpdateData(DateTime dateTime)
        {
            var measurements = new List<Measurement>();
            var r = new Random();

            //           for (int i = 0; i < 2; i++)
            //{
            measurements.Add(new Measurement() { DetectorId = 0, DateTime = dateTime.AddSeconds(1), Value = r.Next(1, 4) });
            measurements.Add(new Measurement() { DetectorId = 1, DateTime = dateTime.AddSeconds(1), Value = r.Next(10, 30) });
            //}
            return measurements;
        }
    }

    public class Measurement
    {
        public int DetectorId { get; set; }
        public int Value { get; set; }
        public DateTime DateTime { get; set; }
    }
}
