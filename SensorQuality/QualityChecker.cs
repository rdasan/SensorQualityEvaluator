﻿using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading.Tasks;
using SensorQuality.Evaluators;
using SensorQuality.Extensions;

namespace SensorQuality
{
    public class QualityChecker : IQualityChecker
    {
        private SensorEvaluationStrategy _sensorEvaluationStrategy;

        public string EvaluateLogFileContents(string logContentsStr)
        {
            if (string.IsNullOrWhiteSpace(logContentsStr))
                throw new ArgumentNullException(nameof(logContentsStr));

            var sensorReadingsMap = new SensorReadingsMap();

            int index = logContentsStr.IndexOf(Environment.NewLine, StringComparison.InvariantCulture);
            string referenceLine = GetReferenceLine(logContentsStr, index);

            //Set up the SensorEvaluationStrategy using the line that has the "reference" info
            _sensorEvaluationStrategy = new SensorEvaluationStrategy(referenceLine);

            //We no longer need the first reference line. So get rid of it from the logContents
            string logContents = logContentsStr.Substring(index + 1);

            //Using a Custom string.SplitLines() extension method to save unnecessary memory allocation
            //Please see more details in Extensions.StringExtensions.cs or ReadMe.md
            foreach (ReadOnlySpan<char> line in logContents.SplitLines())
            {
                if (!line.IsEmpty)//Skipping empty lines if any
                {
                    Sensor sensor = GetSensor(line);
                    sensorReadingsMap.AddReading(sensor, sensor.Reading);
                }
            }

            var evaluationResultMap = EvaluateSensors(sensorReadingsMap);

            return evaluationResultMap.IsEmpty
                ? "Sensor Evaluation did not yield results."
                : JsonSerializer.Serialize(evaluationResultMap, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
        }

        private static string GetReferenceLine(string logContentsStr, int index)
        {
            string referenceLine = index != -1
                ? logContentsStr.Substring(0, index)
                : throw new InvalidOperationException("Input log contents not formatted correctly with spaces");

            if (!referenceLine.StartsWith("reference"))
                throw new InvalidOperationException(
                    $"Reference line not provided. First line in log contents is: '{referenceLine}'");
            return referenceLine;
        }

        private  ConcurrentDictionary<string, string> EvaluateSensors(SensorReadingsMap sensorReadingsMap)
        {
            if (_sensorEvaluationStrategy == null)
                throw new InvalidOperationException("Could not instantiate strategy for evaluating the sensors. Missing/Invalid reference header line");

            var sensorsResult = new ConcurrentDictionary<string, string>();

            //For the provided example sample and level of complexity of calculations,
            //Parallel.ForEach is not required. But concurrency might yield results faster for real life scenarios
            //with huge data sets and complex mathematical evaluations. (Need to consider it case by case)
            Parallel.ForEach(sensorReadingsMap, (sensorReadingsMapItem) =>
            {
                IEvaluator evaluator = _sensorEvaluationStrategy.GetEvaluator(sensorReadingsMapItem.Key.Type);
                string evaluationResult = evaluator.Evaluate(sensorReadingsMapItem.Value);
                sensorsResult.TryAdd(sensorReadingsMapItem.Key.Name, evaluationResult);
            });

            return sensorsResult;
        }
        
        private static Sensor GetSensor(in ReadOnlySpan<char> line)
        {
            //temp temp-1 2007-04-05T22:00 72.4
            var logLine = line.ToString();
            var parts = logLine.Split(' ');

            var sensorInfo = new Sensor
            {
                Type = parts[0],
                Name = parts[1]
            };

            //We don't care about the date part . So discarding that

            if (double.TryParse(parts[3], out double reading))
                sensorInfo.Reading = reading;

            return sensorInfo;
        }
    }
}
