using System.Collections.Generic;

namespace SensorQuality
{
    internal sealed class SensorReferenceMap : Dictionary<string, double>
    {
        public void Load(string input)
        {
            var parts = input.Split(' ');

            for (int i = 1; i < parts.Length; i++)
            {
                var keyVal = parts[i].Split(':');
                if (double.TryParse(keyVal[1], out double value))
                    TryAdd(keyVal[0], value);
            }
        }
    }
}