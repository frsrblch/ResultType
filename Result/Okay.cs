using System;

namespace ResultType
{
    public struct Okay : IEquatable<Okay>
    {
        public bool Equals(Okay _) => true;

        public override bool Equals(object obj) => obj is Okay;

        public override int GetHashCode() => 4463 * 6397;

        public override string ToString() => "()";
    }
}
