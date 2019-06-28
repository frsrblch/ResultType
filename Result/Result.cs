using System;

namespace Result
{
    public static class Result
    {

    }

    public readonly struct Result<TValue, TError>
    {
        public bool IsOkay { get; }
        public bool IsError => !IsOkay;
    }
}
