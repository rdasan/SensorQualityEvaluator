using System;
using System.Collections.Generic;
using SensorQuality.Evaluators;

namespace SensorQuality
{
    internal class SensorEvaluationStrategy
    {
        private readonly Dictionary<string, double> _sensorReferenceMap = new Dictionary<string, double>();

        public SensorEvaluationStrategy(string referenceLine)
        {
            Init(referenceLine);
        }

        public IEvaluator GetEvaluator(string sensorType)
        {
            sensorType = sensorType.Trim().ToLower();
            if(!_sensorReferenceMap.ContainsKey(sensorType))
                throw new InvalidOperationException($"Unknown Sensor Type: {sensorType}");

            return sensorType switch
            {
                "temp" => new TemperatureEvaluator(_sensorReferenceMap[sensorType]),
                "hum" => new HumidityEvaluator(_sensorReferenceMap[sensorType]),
                "mon" => new COMonoxideEvaluator(_sensorReferenceMap[sensorType]),
                _ => throw new InvalidOperationException($"Unknown Sensor Type: {sensorType}")
            };
        }


        private void Init(string referenceLine)
        {
            var parts = referenceLine?.Split(' ');

            string exceptionMessage = $"Reference line not formatted properly: {referenceLine}";
            if (parts == null || parts.Length <= 1)
                throw new ArgumentException(exceptionMessage);

            for (int i = 1; i < parts.Length; i++)
            {
                var keyVal = parts[i].Split(':');

                if(keyVal.Length != 2)
                    throw new ArgumentException(exceptionMessage);

                if (double.TryParse(keyVal[1], out double value))
                    _sensorReferenceMap.TryAdd(keyVal[0].ToLower(), value);
            }
        }
    }
}