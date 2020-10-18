using System.Collections.Generic;

namespace SensorQuality.Evaluators
{
    internal class TemperatureEvaluator : IEvaluator
    {
        private readonly double _sensorReference;

        internal TemperatureEvaluator(double sensorReference)
        {
            _sensorReference = sensorReference;
        }

        public string Evaluate(List<double> readings)
        {
            //ToDo: Do the actual math evaluation
            return "precise";
        }
    }
}