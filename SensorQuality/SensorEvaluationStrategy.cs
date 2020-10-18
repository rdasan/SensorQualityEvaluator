using System;
using SensorQuality.Evaluators;
using SensorQuality.Helpers;

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
                    return new TemperatureEvaluator(_sensorReferenceMap[sensorType]);
                case "hum":
                    return new HumidityEvaluator(_sensorReferenceMap[sensorType]);
                case "mon":
                    return new COMonoxideEvaluator(_sensorReferenceMap[sensorType]);
                default:
                    throw new InvalidOperationException($"Unknown Sensor Type: {sensorType}");
                    
            }
        }
    }
}