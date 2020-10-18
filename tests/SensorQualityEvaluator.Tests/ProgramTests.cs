using System;
using FluentAssertions;
using Moq;
using SensorQuality;
using Xunit;

namespace SensorQualityEvaluator.Tests
{
    public class ProgramTests
    {
        private const int SuccessCode = 0;
        private const int FailureCode = 1;

        [Theory]
        [InlineData("")]
        [InlineData("c:/someFile")]
        public async void App_Returns_Failure_Invalid_CmdLine_Args(string filePath)
        {
            var result = await Program.Main(new [] { filePath });
            result.Should().Be(FailureCode);
        }

        [Fact]
        public async void App_Returns_Success()
        {
            var mockQualityChecker = new Mock<IQualityChecker>();
            mockQualityChecker.Setup(q => q.EvaluateLogFileContents(It.IsAny<string>())).Returns("valid result");

            var result = await Program.EvaluateLogFile("logFileContents", mockQualityChecker.Object);
            result.Should().Be(SuccessCode);
        }

        [Fact]
        public async void App_Returns_Failure_If_Library_Throws()
        {
            var mockQualityChecker = new Mock<IQualityChecker>();
            mockQualityChecker.Setup(q => q.EvaluateLogFileContents(It.IsAny<string>()))
                .Throws<InvalidOperationException>();

            var result = await Program.EvaluateLogFile("logFileContents", mockQualityChecker.Object);
            result.Should().Be(FailureCode);
        }
    }
}
