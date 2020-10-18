namespace SensorQuality.Evaluators
{
    internal interface IEvaluator
    {
        string GetQualityStatus(double readings);
    }
}
