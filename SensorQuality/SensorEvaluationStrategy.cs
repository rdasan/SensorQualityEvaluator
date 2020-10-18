using System;
using SensorQuality.Evaluators;

namespace SensorQuality
{
    internal class SensorEvaluationStrategy
    {
        private readonly SensorReferenceMap _sensorReferenceMap = new SensorReferenceMap();

        public SensorEvaluationStrategy(string referenceLine)
        {
            _sensorReferenceMap.Load(referenceLine);
        }

        public IEvaluator GetEvaluator(string sensorType)
        {
            switch (sensorType)
            {
                case "temp":
                    return new TemperatureEvaluator();
                case "hum":
                    return new HumidityEvaluator();
                case "mon":
                    return new COMonoxideEvaluator();
                default:
                    throw new InvalidOperationException($"Unknown Sensor Type: {sensorType}");
                    
            }
        }
    }
}