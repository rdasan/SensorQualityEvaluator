namespace SensorQuality.Evaluators
{
    internal class HumidityEvaluator : IEvaluator
    {
        private readonly double _sensorReference;

        public HumidityEvaluator(double sensorReference)
        {
            _sensorReference = sensorReference;
        }

        public string GetQualityStatus(double readings)
        {
            //ToDo: Do the actual math evaluation
            return "discard";
        }
    }
}