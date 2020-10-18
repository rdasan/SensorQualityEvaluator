using System.Collections.Generic;

namespace SensorQuality.Evaluators
{
    internal interface IEvaluator
    {
        string Evaluate(List<double> readings);
    }
}
