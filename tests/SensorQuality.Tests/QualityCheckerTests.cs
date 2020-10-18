using System;
using FluentAssertions;
using Xunit;

namespace SensorQuality.Tests
{
    public class QualityCheckerTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EvaluateLogFile_Throws_For_Invalid_Input(string logContentsStr)
        {
            var qualityChecker = new QualityChecker();
            Func<string> act = () => qualityChecker.EvaluateLogFile(logContentsStr);
            act.Should().Throw<ArgumentNullException>().WithMessage($"Value cannot be null. (Parameter '{nameof(logContentsStr)}')");
        }
    }
}
