using System;

namespace Tests
{
    internal struct Crate : IEquatable<Crate>
    {
        public Crate(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public bool Equals(Crate other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Crate other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public static bool operator ==(Crate left, Crate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Crate left, Crate right)
        {
            return !left.Equals(right);
        }
    }
}