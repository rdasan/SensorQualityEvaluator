using System;
using System.Diagnostics;
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

        static async Task<int> Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
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

            try
            {
                var samplingContent = await File.ReadAllTextAsync(samplingFilePath);

                QualityChecker qualityChecker = new QualityChecker();
                string sensorQualityReport = qualityChecker.EvaluateLogFile(samplingContent);

                Console.WriteLine(sensorQualityReport);

                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds);

                return SuccessCode;
            }
            catch (Exception ex)
            {
                await LogExceptionAsync(ex);
                return FailureCode;
            }
        }

        public static async Task LogExceptionAsync(Exception exception)
        {
            await LogErrorAsync($"{exception}", "EXCEPTION");
        }

        public static async Task LogErrorAsync(string message, string errorType = "ERROR")
        {
            //If we have other log providers like NLog, Serilog, we can log to them here
            await Console.Error.WriteLineAsync($"{errorType} {DateTime.UtcNow} Application: SensorQualityEvaluator, message: {message}");
        }
    }
}