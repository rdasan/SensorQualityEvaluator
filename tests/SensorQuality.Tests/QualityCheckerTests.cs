using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace SensorQuality.Tests
{
    [ExcludeFromCodeCoverage]
    public class QualityCheckerTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EvaluateLogFileContents_Throws_For_Invalid_Input(string logContentsStr)
        {
            var qualityChecker = new QualityChecker();
            Func<string> act = () => qualityChecker.EvaluateLogFileContents(logContentsStr);
            act.Should().Throw<ArgumentNullException>().WithMessage($"Value cannot be null. (Parameter '{nameof(logContentsStr)}')");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("thresholds temp:70.0 hum:45.0 mon:6")]
        public void EvaluateLogFileContents_Throws_For_Missing_Reference_Line(string referenceLine)
        {
            string inputLogContents =
                $@"{referenceLine}
temp temp-1 2007-04-05T22:00 72.4
temp temp-1 2007-04-05T22:01 76.0
hum hum-1 2007-04-05T22:05 45.3
hum hum-1 2007-04-05T22:06 45.1
temp temp-1 2007-04-05T22:04 71.2
temp temp-2 2007-04-05T22:02 70.1";

            var qualityChecker = new QualityChecker();
            Func<string> act = () => qualityChecker.EvaluateLogFileContents(inputLogContents);
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void EvaluateLogFileContents_Succeeds_Even_If_Empty_Lines_Found()
        {
            string inputLogContents =
                @"reference temp:70.0 hum:45.0 mon:6
temp temp-1 2007-04-05T22:00 72.4
temp temp-1 2007-04-05T22:01 76.0

hum hum-1 2007-04-05T22:05 45.3
hum hum-1 2007-04-05T22:06 45.1


temp temp-1 2007-04-05T22:04 71.2
temp temp-2 2007-04-05T22:02 70.1";

            var qualityChecker = new QualityChecker();
            var result = qualityChecker.EvaluateLogFileContents(inputLogContents);
            var sensorQuality = JsonSerializer.Deserialize<Dictionary<string, string>>(result);

            sensorQuality.Should().NotBeEmpty();
            sensorQuality.Should().ContainKeys(new[] {"temp-1", "hum-1", "temp-2"});
        }

        [Fact]
        public void EvaluateLogFile_Returns_Valid_Message_If_LogFile_Has_No_Readings()
        {
            string inputLogContents = 
                @"reference temp:70.0 hum:45.0 mon:6
";
            QualityChecker qualityChecker = new QualityChecker();
            var result = qualityChecker.EvaluateLogFileContents(inputLogContents);

            result.Should().Be("Sensor Evaluation did not yield results.");
        }

        [Fact]
        public async Task EvaluateLogFileContents_SmokeTest()
        {
            string inputLogContents = await File.ReadAllTextAsync("LogSample.txt");
            QualityChecker qualityChecker = new QualityChecker();

            var result = qualityChecker.EvaluateLogFileContents(inputLogContents);
            var sensorQuality = JsonSerializer.Deserialize<Dictionary<string, string>>(result);

            sensorQuality.Should().NotBeEmpty();
            sensorQuality["temp-1"].Should().Be("precise");
            sensorQuality["temp-2"].Should().Be("ultra precise");
            sensorQuality["hum-1"].Should().Be("keep");
            sensorQuality["hum-2"].Should().Be("discard");
            sensorQuality["mon-1"].Should().Be("keep");
            sensorQuality["mon-2"].Should().Be("discard");
        }
    }
}
