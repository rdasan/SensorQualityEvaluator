using System.Collections.Generic;

namespace SensorQuality.Evaluators
{
    internal interface IEvaluator
    {
        string Evaluate(IEnumerable<double> readings);
    }
}
