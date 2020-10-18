namespace SensorQuality
{
    /// <summary>
    /// Interface that has methods for evaluating the quality of the sensors
    /// </summary>
    public interface IQualityChecker
    {
        /// <summary>
        /// Evaluates the quality status of sensors based on the logContent and returns the result in the format
        /// {
        ///     "temp-1": "precise",
        ///     "temp-2": "ultra precise",
        ///     "hum-1": "keep",
        ///     "hum-2": "discard",
        ///     "mon-1": "keep",
        ///     "mon-2": "discard"
        /// } 
        /// </summary>
        /// <param name="logContentsStr"></param>
        /// <returns></returns>
        string EvaluateLogFileContents(string logContentsStr);
    }
}