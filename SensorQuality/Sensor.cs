using System;

namespace SensorQuality
{
    internal struct Sensor : IEquatable<Sensor>
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public double Reading { get; set; }

        public bool Equals(Sensor other)
        {
            return Type == other.Type && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return obj is Sensor other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Name);
        }

        public static bool operator ==(Sensor left, Sensor right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Sensor left, Sensor right)
        {
            return !left.Equals(right);
        }
    }
}