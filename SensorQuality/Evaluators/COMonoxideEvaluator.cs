using System.Collections.Generic;
using SensorQuality.Extensions;

namespace SensorQuality.Evaluators
{
    internal sealed class COMonoxideEvaluator : IEvaluator
    {
        private readonly double _sensorReference;
        private const double ErrorTolerance = 3;

        internal COMonoxideEvaluator(double sensorReference)
        {
            _sensorReference = sensorReference;
        }

        public string Evaluate(List<double> readings)
        {
            return readings.AreFaultTolerant(_sensorReference, ErrorTolerance) ? "keep" : "discard";
        }
    }
}