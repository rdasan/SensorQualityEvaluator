using System;
using System.Collections.Generic;

namespace SensorQuality.Extensions
{
    internal static class ListExtensions
    {
        internal static bool AreFaultTolerant(this List<double> readings, double referenceValue, double errorTolerance)
        {
            foreach (var reading in readings)
            {
                if (Math.Abs(referenceValue - reading) > errorTolerance)
                    return false;
            }

            return true;
        }
    }
}
