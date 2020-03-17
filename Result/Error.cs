using OptionType;
using System;

namespace ResultType
{
    public struct Error : IEquatable<Error>
    {
        public Option<string> Message { get; }
        
        public Error(string message)
        {
            Message = message;
        }

        public bool Equals(Error other) => Message.Equals(other.Message);

        public override bool Equals(object obj) => obj is Error error && Equals(error);

        public override int GetHashCode() => 3221 * 5039;

        public override string ToString() => Message.Match(
            m => $"Error: {m}",
            () => "Error");
    }
}
