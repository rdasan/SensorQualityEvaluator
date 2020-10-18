using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SensorQuality
{
    internal class SensorReadingsMap : ConcurrentDictionary<Sensor, IEnumerable<double>>
    {
        internal void AddReading(Sensor sensor, double reading)
        {
            if (ContainsKey(sensor))
            {
                var list = this[sensor];
                if (!list.Contains(reading))
                {
                    var updatedList = list.Append(reading);
                    TryUpdate(sensor, updatedList, list);
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