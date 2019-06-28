using System;
using System.Collections.Generic;

namespace Result
{
    /// <summary>
    /// Empty tuple.
    /// 
    /// Usage: Result<int, Unit>
    /// </summary>
    public struct Unit : IEquatable<Unit>
    {
        public bool Equals(Unit _) => true;

        public override bool Equals(object obj) => obj is Unit;

        public override int GetHashCode() => 3221 * 5039;

        public override string ToString() => "()";
    }

    public static class Result
    {
        public static Result<TValue, TError> Okay<TValue, TError>(TValue value)
        {
            return new Result<TValue, TError>(value);
        }

        public static Result<TValue, TError> Error<TValue, TError>(TError error)
        {
            return new Result<TValue, TError>(error);
        }
    }

    public struct Result<TValue, TError> : IEquatable<Result<TValue, TError>>
    {
        public bool IsOkay { get; }
        public bool IsError => !IsOkay;

        private readonly object _value;

        public static Result<TValue, TError> Okay(TValue value)
        {
            return new Result<TValue, TError>(value);
        }

        public static Result<TValue, TError> Error(TError error)
        {
            return new Result<TValue, TError>(error);
        }

        internal Result(TValue value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Result.Okay: the okay value cannot be null.");
            }

            IsOkay = true;
            _value = value;
        }

        internal Result(TError error)
        {
            if (error == null)
            {
                throw new ArgumentNullException("Result.Error: the error value cannot be null.");
            }

            IsOkay = false;
            _value = error;
        }

        public static implicit operator Result<TValue, TError>(TValue value)
        {
            return Okay(value);
        }

        public static implicit operator Result<TValue, TError>(TError error)
        {
            return Error(error);
        }

        public UValue Match<UValue>(Func<TValue, UValue> okayFunc, Func<TError, UValue> errorFunc)
        {
            if (IsOkay)
            {
                return okayFunc((TValue)_value);
            }
            else
            {
                return errorFunc((TError)_value);
            }
        }

        public void Match(Action<TValue> okayAction, Action<TError> errorAction)
        {
            if (IsOkay)
            {
                okayAction((TValue)_value);
            }
            else
            {
                errorAction((TError)_value);
            }
        }

        public void MatchOkay(Action<TValue> action)
        {
            Match(action, _ => { });
        }

        public void MatchOkay(Action action)
        {
            Match(_ => action(), _ => { });
        }

        public void MatchError(Action<TError> action)
        {
            Match(_ => { }, action);
        }

        public void MatchError(Action action)
        {
            Match(_ => { }, _ => action());
        }

        public Result<UValue, TError> Map<UValue>(Func<TValue, UValue> mapFunction)
        {
            return Match(
                okay => mapFunction(okay),
                error => Result<UValue, TError>.Error(error));
        }

        public Result<TValue, UError> MapError<UError>(Func<TError, UError> mapError)
        {
            return Match(
                okay => Result.Okay<TValue, UError>(okay),
                error => mapError(error));
        }

        public bool Contains(TValue value)
        {
            return IsOkay && ((TValue)_value).Equals(value);
        }

        public bool ContainsError(TError error)
        {
            return IsError && ((TError)_value).Equals(error);
        }

        public bool Equals(Result<TValue, TError> other)
        {
            return Match(
                okay => other.Contains(okay),
                error => other.ContainsError(error));
        }

        public override bool Equals(object obj)
        {
            return obj is Result<TValue, TError> result
                && Equals(result);
        }

        public override int GetHashCode()
        {
            return Match(
                okay => 2633 ^ okay.GetHashCode(),
                error => 3137 ^ error.GetHashCode());
        }

        public override string ToString()
        {
            return Match(
                okay => $"Okay({okay})",
                error => $"Error({error})");
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            if (IsOkay) yield return (TValue)_value;
        }

        public IEnumerable<TValue> AsEnumerable()
        {
            if (IsOkay) yield return (TValue)_value;
        }
    }
}
