using System.Collections.Generic;
using SensorQuality.Extensions;

namespace SensorQuality.Evaluators
{
    internal sealed class HumidityEvaluator : IEvaluator
    {
        private readonly double _sensorReference;
        private const double ErrorTolerance = 1;

        internal HumidityEvaluator(double sensorReference)
        {
            _sensorReference = sensorReference;
        }

        public string Evaluate(List<double> readings)
        {
            return readings.AreFaultTolerant(_sensorReference, ErrorTolerance) ? "keep" : "discard";
        }
    }
}