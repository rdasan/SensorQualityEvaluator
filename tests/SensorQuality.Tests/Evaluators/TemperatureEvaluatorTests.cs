using System.Collections.Generic;
using FluentAssertions;
using SensorQuality.Evaluators;
using Xunit;

namespace SensorQuality.Tests.Evaluators
{
    public class TemperatureEvaluatorTests
    {
        private const double ReferenceValue = 70.0;

        [Fact]
        public void Evaluate_Results_In_Precise()
        {
            var tempEvaluator = new TemperatureEvaluator(ReferenceValue);
            var readings = new [] {72.4, 76.0, 79.1, 75.6, 71.2, 71.4, 69.2, 65.2, 62.8};

            var sensorQuality = tempEvaluator.Evaluate(readings);

            sensorQuality.Should().Be("precise");
        }

        [Fact]
        public void Evaluate_Results_In_UltraPrecise()
        {
            var tempEvaluator = new TemperatureEvaluator(ReferenceValue);
            var readings = new [] { 69.5, 70.1, 71.3, 71.5, 69.8 };

            var sensorQuality = tempEvaluator.Evaluate(readings);

            sensorQuality.Should().Be("ultra precise");
        }

        [Fact]
        public void Evaluate_Results_In_VeryPrecise()
        {
            var tempEvaluator = new TemperatureEvaluator(ReferenceValue);
            var readings = new [] {60.0,70.4,72,69.8,72,69.7,70.2,75};

            var sensorQuality = tempEvaluator.Evaluate(readings);

            sensorQuality.Should().Be("very precise");
        }

        [Fact]
        public void Evaluate_Results_In_Empty()
        {
            var tempEvaluator = new TemperatureEvaluator(ReferenceValue);
            var readings = new List<double>();

            var sensorQuality = tempEvaluator.Evaluate(readings);

            sensorQuality.Should().Be("No valid readings provided");
        }
    }
}
