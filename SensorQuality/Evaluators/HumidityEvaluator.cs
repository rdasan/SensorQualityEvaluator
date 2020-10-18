namespace SensorQuality.Evaluators
{
    internal class HumidityEvaluator : IEvaluator
    {
        public string GetQualityStatus(double readings)
        {
            return "discard";
        }
    }
}