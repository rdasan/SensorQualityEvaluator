using System.Collections.Generic;
using FluentAssertions;
using SensorQuality.Evaluators;
using Xunit;

namespace SensorQuality.Tests.Evaluators
{
    public class COMonoxideEvaluatorTests
    {
        private const double ReferenceValue = 6;

        [Fact]
        public void Evaluate_Results_In_Keep()
        {
            var coMonoxideEvaluator = new COMonoxideEvaluator(ReferenceValue);
            var readings = new[] { 5.0, 7, 9};
            var sensorQuality = coMonoxideEvaluator.Evaluate(readings);

            sensorQuality.Should().Be("keep");
        }

        [Fact]
        public void Evaluate_Results_In_Discard()
        {
            var coMonoxideEvaluator = new COMonoxideEvaluator(ReferenceValue);
            var readings = new[] { 2, 4, 10, 8.0, 6 };
            var sensorQuality = coMonoxideEvaluator.Evaluate(readings);

            sensorQuality.Should().Be("discard");
        }

        [Fact]
        public void Evaluate_Results_In_Empty()
        {
            var coMonoxideEvaluator = new COMonoxideEvaluator(ReferenceValue);
            var readings = new List<double>();

            var sensorQuality = coMonoxideEvaluator.Evaluate(readings);

            sensorQuality.Should().Be("No valid readings provided");
        }
    }
}