using System;
using System.Collections.Generic;

namespace SensorQuality.Extensions
{
    internal static class ListExtensions
    {
        internal static bool AreFaultTolerant(this List<double> readings, double referenceValue, double faultTolerance)
        {
            foreach (var reading in readings)
            {
                if (Math.Abs(referenceValue - reading) > faultTolerance)
                    return false;
            }

            return true;
        }

        internal static bool IsValid(this List<double> readings)
        {
            return readings != null && readings.Count != 0;
        }
    }
}
