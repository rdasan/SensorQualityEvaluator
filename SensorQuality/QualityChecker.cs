﻿using System;
using System.Collections.Concurrent;
using System.Text.Json;
using SensorQuality.Evaluators;
using SensorQuality.Extensions;
using SensorQuality.Helpers;

namespace SensorQuality
{
    public class QualityChecker
    {
        private SensorEvaluationStrategy _sensorEvaluationStrategy;
        private static readonly object _lockObj = new object();

        public string EvaluateLogFile(string logContentsStr)
        {
            if (string.IsNullOrWhiteSpace(logContentsStr))
                throw new InvalidOperationException("File contents are invalid");

            var sensorReadingsMap = new SensorReadingsMap();

            foreach (ReadOnlySpan<char> line in logContentsStr.SplitLines()) //Todo: Use Parallel.ForEach for better performance
            {
                if (line.StartsWith("reference", StringComparison.OrdinalIgnoreCase))
                {
                    //Set up the SensorEvaluationStrategy using the line that has the "reference" info
                    //and immediately move to the next line
                    _sensorEvaluationStrategy = BuildSensorEvaluationStrategy(line);
                    continue;
                }

                Sensor sensor = GetSensorDevice(line);

                sensorReadingsMap.AddReading(sensor, sensor.Reading);
            }

            var evaluationResultMap = EvaluateSensors(sensorReadingsMap);

            return evaluationResultMap.IsEmpty
                ? "Sensor Evaluation did not yield results. Reason Unknown."
                : JsonSerializer.Serialize(evaluationResultMap, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
        }

        private ConcurrentDictionary<string, string> EvaluateSensors(SensorReadingsMap sensorReadingsMap)
        {
            if (_sensorEvaluationStrategy == null)
                throw new InvalidOperationException("Could not instantiate strategy for evaluating the sensors. Missing/Invalid reference header line");

            var sensorsResult = new ConcurrentDictionary<string, string>();

            foreach (var (sensor, readings) in sensorReadingsMap)
            {
                IEvaluator evaluator = _sensorEvaluationStrategy.GetEvaluator(sensor.Type);
                string evaluationResult = evaluator.Evaluate(readings);
                sensorsResult.TryAdd(sensor.Name, evaluationResult);
            }

            return sensorsResult;
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

        private SensorEvaluationStrategy BuildSensorEvaluationStrategy(ReadOnlySpan<char> line)
        {
            if (_sensorEvaluationStrategy != null)
                throw new InvalidOperationException(
                    $"Sensor evaluation references already initialized. Duplicate reference line found {line.ToString()}");

            if (_sensorEvaluationStrategy == null)
            {
                lock (_lockObj)
                {
                    _sensorEvaluationStrategy ??= new SensorEvaluationStrategy(line.ToString());
                }
            }

            return _sensorEvaluationStrategy;
        }
    }
}
