using System.Collections.Generic;
using FluentAssertions;
using SensorQuality.Evaluators;
using Xunit;

namespace SensorQuality.Tests.Evaluators
{
    public class HumidityEvaluatorTests
    {
        private const double ReferenceValue = 45.0;

        [Fact]
        public void Evaluate_Results_In_Keep()
        {
            var humEvaluator = new HumidityEvaluator(ReferenceValue);
            var readings = new[] {45.2, 45.3, 45.1};
            var sensorQuality = humEvaluator.Evaluate(readings);

            sensorQuality.Should().Be("keep");
        }

        [Fact]
        public void Evaluate_Results_In_Discard()
        {
            var humEvaluator = new HumidityEvaluator(ReferenceValue);
            var readings = new[] { 45.0, 43.0, 45.1 };
            var sensorQuality = humEvaluator.Evaluate(readings);

            sensorQuality.Should().Be("discard");
        }

        [Fact]
        public void Evaluate_Results_In_Empty()
        {
            var tempEvaluator = new HumidityEvaluator(ReferenceValue);
            var readings = new List<double>();

            var sensorQuality = tempEvaluator.Evaluate(readings);

            sensorQuality.Should().Be("No valid readings provided");
        }
    }
}