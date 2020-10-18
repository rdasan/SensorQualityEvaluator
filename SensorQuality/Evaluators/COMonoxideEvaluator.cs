using System.Collections.Generic;

namespace SensorQuality.Evaluators
{
    internal class COMonoxideEvaluator : IEvaluator
    {
        private readonly double _sensorReference;

        internal COMonoxideEvaluator(double sensorReference)
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