using System;
using System.Collections.Concurrent;
using SensorQuality.Evaluators;
using SensorQuality.Extensions;

namespace SensorQuality
{
    public class QualityChecker
    {
        private SensorEvaluationStrategy _sensorEvaluationStrategy = null;

        public string EvaluateLogFile(string logContentsStr)
        {
            if (string.IsNullOrWhiteSpace(logContentsStr))
                throw new InvalidOperationException("File contents are invalid");

            var sensorReadings = new ConcurrentDictionary<Sensor, double>();
            foreach (ReadOnlySpan<char> line in logContentsStr.SplitLines())
            {
                if (line.StartsWith("reference", StringComparison.OrdinalIgnoreCase))
                {
                    //Set up the SensorEvaluationStrategy using the first reference line
                    _sensorEvaluationStrategy = new SensorEvaluationStrategy(line.ToString());
                    continue;
                }

                var sensor = GetSensorDevice(line);

                //if(!_sensorEvaluationStrategy.ContainsKey(sensor.Type))
                //    throw new InvalidOperationException($"No reference value provided for sensor type {sensor.Type}");

                sensorReadings.AddOrUpdate(sensor, sensor.Reading,
                    (key, existingReading) => existingReading + sensor.Reading);
            }

            string evaluationResult = EvaluateSensors(sensorReadings);

            return string.Empty;
        }

        private string EvaluateSensors(ConcurrentDictionary<Sensor, double> sensorReadings)
        {
            if (_sensorEvaluationStrategy == null)
                throw new InvalidOperationException("Could not instantiate strategy for evaluating the sensors. Missing/Invalid reference header line");

            var sensorsResult = new ConcurrentDictionary<string, string>();

            foreach (var (sensor, readings) in sensorReadings)
            {
                IEvaluator evaluator = _sensorEvaluationStrategy.GetEvaluator(sensor.Type);
                string evaluationResult = evaluator.GetQualityStatus(readings);
                sensorsResult.TryAdd(sensor.Name, evaluationResult);
            }

        }


        private static Sensor GetSensorDevice(in ReadOnlySpan<char> line)
        {
            //temp temp-1 2007-04-05T22:00 72.4
            var logLine = line.ToString();
            var parts = logLine.Split(' ');

            var sensorInfo = new Sensor
            {
                Type = parts[0],
                Name = parts[1]
            };
            if (double.TryParse(parts[3], out double reading))
                sensorInfo.Reading = reading;

            return sensorInfo;
        }

        //public string EvaluateLogFile(string logContentsStr)
        //{
        //    if (string.IsNullOrWhiteSpace(logContentsStr))
        //        throw new InvalidOperationException("File contents are invalid");

        //    string referenceLine;
        //    using (var reader = new StringReader(logContentsStr))
        //    {
        //        referenceLine = reader.ReadLine();
        //    }

        //    if (string.IsNullOrWhiteSpace(referenceLine))
        //        throw new InvalidOperationException("Could not find reference values for the sensors");

        //    _sensorReferenceMap.Load(referenceLine);




        //    return string.Empty;
        //}
    }
}
