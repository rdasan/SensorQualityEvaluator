using System.Collections.Generic;
using SensorQuality.Extensions;

namespace SensorQuality.Evaluators
{
    internal sealed class HumidityEvaluator : IEvaluator
    {
        private readonly double _sensorReference;
        private const double FaultTolerance = 1;

        internal HumidityEvaluator(double sensorReference)
        {
            _sensorReference = sensorReference;
        }

        public string Evaluate(List<double> readings)
        {
            if (!readings.IsValid())
                return "No valid readings provided";

            return readings.AreFaultTolerant(_sensorReference, FaultTolerance) ? "keep" : "discard";
        }
    }
}