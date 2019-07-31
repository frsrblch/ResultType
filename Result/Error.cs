using System;

namespace Result
{
    public struct Error : IEquatable<Error>
    {
        public bool Equals(Error _) => true;

        public override bool Equals(object obj) => obj is Error;

        public override int GetHashCode() => 3221 * 5039;

        public override string ToString() => "Error";
    }
}
