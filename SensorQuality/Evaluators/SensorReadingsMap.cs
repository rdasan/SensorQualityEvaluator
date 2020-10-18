using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SensorQuality.Evaluators
{
    internal class SensorReadingsMap : ConcurrentDictionary<Sensor, List<double>>
    {
        internal void AddReading(Sensor sensor, double reading)
        {
            if (ContainsKey(sensor))
            {
                List<double> list = this[sensor];
                if (!list.Contains(reading))
                {
                    list.Add(reading);
                }
            }
            else
            {
                List<double> list = new List<double> {reading};
                TryAdd(sensor, list);
            }
        }
    }
}