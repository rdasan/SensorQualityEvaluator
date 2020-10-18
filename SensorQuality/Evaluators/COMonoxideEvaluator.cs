namespace SensorQuality.Evaluators
{
    internal class COMonoxideEvaluator : IEvaluator
    {
        public string GetQualityStatus(double readings)
        {
            return "discard";
        }
    }
}