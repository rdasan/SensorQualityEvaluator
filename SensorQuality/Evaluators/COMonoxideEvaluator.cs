namespace SensorQuality.Evaluators
{
    internal class COMonoxideEvaluator : IEvaluator
    {
        private readonly double _sensorReference;

        public COMonoxideEvaluator(double sensorReference)
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