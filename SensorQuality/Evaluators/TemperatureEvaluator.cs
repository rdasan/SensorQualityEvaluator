namespace SensorQuality.Evaluators
{
    internal class TemperatureEvaluator : IEvaluator
    {
        private readonly double _sensorReference;

        public TemperatureEvaluator(double sensorReference)
        {
            _sensorReference = sensorReference;
        }

        public string GetQualityStatus(double readings)
        {
            //ToDo: Do the actual math evaluation
            return "precise";
        }
    }
}