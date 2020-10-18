using System.Collections.Generic;
using SensorQuality.Extensions;

namespace SensorQuality.Evaluators
{
    internal sealed class COMonoxideEvaluator : IEvaluator
    {
        private readonly double _sensorReference;
        private const double FaultTolerance = 3;

        internal COMonoxideEvaluator(double sensorReference)
        {
            _sensorReference = sensorReference;
        }

        public string Evaluate(IEnumerable<double> readings)
        {
            if (!readings.IsValid())
                return "No valid readings provided";

            return readings.AreFaultTolerant(_sensorReference, FaultTolerance) ? "keep" : "discard";
        }
    }
}