using System.Collections.Generic;

namespace SensorQuality.Evaluators
{
    internal class HumidityEvaluator : IEvaluator
    {
        private readonly double _sensorReference;

        internal HumidityEvaluator(double sensorReference)
        {
            _sensorReference = sensorReference;
        }

        public string Evaluate(List<double> readings)
        {
            //ToDo: Do the actual math evaluation
            return "discard";
        }
    }
}