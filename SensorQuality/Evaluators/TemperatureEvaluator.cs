namespace SensorQuality.Evaluators
{
    internal class TemperatureEvaluator : IEvaluator
    {
        public string GetQualityStatus(double readings)
        {
            return "precise";
        }
    }
}