using System;
using System.IO;
using System.Threading.Tasks;
using SensorQuality;

namespace SensorQualityEvaluator
{
    class Program
    {
        //For Cross Platform compatibility. If this app is run in a docker container,
        //the specific exit codes can help determine action to be taken within the container
        private const int SuccessCode = 0;
        private const int FailureCode = 1;

        internal static async Task<int> Main(string[] args)
        {
            //The first command line argument should be the file path
            if (args.Length < 1)
            {
                await LogErrorAsync("Missing arguments");
                return FailureCode;
            }

            string samplingFilePath = args[0];
            if (!File.Exists(samplingFilePath))
            {
                await LogErrorAsync("File not found");
                return FailureCode;
            }

            var samplingContent = await File.ReadAllTextAsync(samplingFilePath);

            return await EvaluateLogFile(samplingContent, new QualityChecker());
        }

        internal static async Task<int> EvaluateLogFile(string samplingContent, IQualityChecker qualityChecker)
        {
            try
            {
                string sensorQualityReport = qualityChecker.EvaluateLogFileContents(samplingContent);

                Console.WriteLine(sensorQualityReport);
                return SuccessCode;
            }
            catch (Exception ex)
            {
                await LogExceptionAsync(ex);
                return FailureCode;
            }
        }

        private static async Task LogExceptionAsync(Exception exception)
        {
            await LogErrorAsync($"{exception}", "EXCEPTION");
        }

        private static async Task LogErrorAsync(string message, string errorType = "ERROR")
        {
            //If we have other log providers like NLog, Serilog, we can log to them here
            await Console.Error.WriteLineAsync($"{errorType} {DateTime.UtcNow} Application: SensorQualityEvaluator, message: {message}");
        }
    }
}