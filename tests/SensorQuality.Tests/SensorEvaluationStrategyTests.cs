using System;
using FluentAssertions;
using SensorQuality.Evaluators;
using Xunit;

namespace SensorQuality.Tests
{
    public class SensorEvaluationStrategyTests
    {
        private const string GoodReferenceLine = "reference temp:70.0 hum:45.0 mon:6";

        [Theory]
        [InlineData("temp")]
        [InlineData("TEMP")]
        [InlineData("tEmP")]
        public void GetEvaluator_Returns_TemperatureEvaluator_Case_Insensitive(string sensorType)
        {
            var strategy = new SensorEvaluationStrategy(GoodReferenceLine);

            IEvaluator evaluator = strategy.GetEvaluator(sensorType);
            evaluator.Should().BeOfType<TemperatureEvaluator>();
        }

        [Theory]
        [InlineData("hum")]
        [InlineData("HUM")]
        [InlineData("hUm")]
        public void GetEvaluator_Returns_HumidityEvaluator_Case_Insensitive(string sensorType)
        {
            var strategy = new SensorEvaluationStrategy(GoodReferenceLine);

            IEvaluator evaluator = strategy.GetEvaluator(sensorType);
            evaluator.Should().BeOfType<HumidityEvaluator>();
        }

        [Theory]
        [InlineData("mon")]
        [InlineData("MON")]
        [InlineData("mOn")]
        public void GetEvaluator_Returns_COMonoxideEvaluator_Case_Insensitive(string sensorType)
        {
            var strategy = new SensorEvaluationStrategy(GoodReferenceLine);

            IEvaluator evaluator = strategy.GetEvaluator(sensorType);
            evaluator.Should().BeOfType<COMonoxideEvaluator>();
        }

        [Fact]
        public void GetEvaluator_Throws_For_Unrecognized_SensorType()
        {
            var sensorType = "smoke";
            var strategy = new SensorEvaluationStrategy(GoodReferenceLine);

            Func<IEvaluator> action = () => strategy.GetEvaluator(sensorType);
            action.Should().Throw<InvalidOperationException>().WithMessage($"Unknown Sensor Type: {sensorType}");
        }

        [Theory]
        [InlineData("referencetemp:70.0hum:45.0mon:6")]
        [InlineData("reference temp:70.0;hum:45.0;mon:6")]
        [InlineData("reference temp:70.0 hum:45.0 mon-6")]
        [InlineData("reference temp:70.0 hum")]
        [InlineData("reference")]
        [InlineData("")]
        [InlineData(null)]
        public void Constructor_Throws_For_Invalid_ReferenceLine(string referenceLine)
        {
            Func<SensorEvaluationStrategy> action = () => new SensorEvaluationStrategy(referenceLine);
            action.Should().Throw<ArgumentException>().WithMessage($"Reference line not formatted properly: {referenceLine}");
        }
    }
}
