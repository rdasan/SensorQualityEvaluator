using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Statistics;
using SensorQuality.Extensions;

namespace SensorQuality.Evaluators
{
    internal sealed class TemperatureEvaluator : IEvaluator
    {
        private readonly double _sensorReference;
        private const double MeanFaultTolerance = 0.5;
        private const double UltraPreciseStdDevTolerance = 3;
        private const double VeryPreciseStdDevTolerance = 5;

        internal TemperatureEvaluator(double sensorReference)
        {
            _sensorReference = sensorReference;
        }

        public string Evaluate(IEnumerable<double> readings)
        {
            if (!readings.IsValid())
                return "No valid readings provided";

            var mean = readings.Average();

            if (Math.Abs(_sensorReference - mean) < MeanFaultTolerance &&
                readings.PopulationStandardDeviation() < UltraPreciseStdDevTolerance)
            {
                return "ultra precise";
            }

            if (Math.Abs(_sensorReference - mean) < MeanFaultTolerance &&
                readings.PopulationStandardDeviation() < VeryPreciseStdDevTolerance)
            {
                return "very precise";
            }

            return "precise";
        }
    }
}