using System;
using System.Collections.Generic;
using System.Linq;

namespace SensorQuality.Extensions
{
    internal static class ReadingsExtensions
    {
        internal static bool AreFaultTolerant(this IEnumerable<double> readings, double referenceValue, double faultTolerance)
        {
            foreach (var reading in readings)
            {
                if (Math.Abs(referenceValue - reading) > faultTolerance)
                    return false;
            }

            return true;
        }

        internal static bool IsValid(this IEnumerable<double> readings)
        {
            return readings != null && readings.Count() != 0;
        }
    }
}
