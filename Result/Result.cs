using Option;
using System;
using System.Collections.Generic;

namespace Result
{

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

        private readonly TValue _value;
        private readonly TError _error;

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
            _error = default;
        }

        internal Result(TError error)
        {
            if (error == null)
            {
                throw new ArgumentNullException("Result.Error: the error value cannot be null.");
            }

            IsOkay = false;
            _value = default;
            _error = error;
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
                return okayFunc(_value);
            }
            else
            {
                return errorFunc(_error);
            }
        }

        public void Match(Action<TValue> okayAction, Action<TError> errorAction)
        {
            if (IsOkay)
            {
                okayAction(_value);
            }
            else
            {
                errorAction(_error);
            }
        }

        public void MatchOkay(Action<TValue> action)
        {
            Match(
                okay => action(okay),
                error => { });
        }

        public void MatchOkay(Action action)
        {
            Match(
                okay => action(),
                error => { });
        }

        public void MatchError(Action<TError> action)
        {
            Match(
                okay => { },
                error => action(error));
        }

        public void MatchError(Action action)
        {
            Match(
                okay => { },
                error => action());
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

        public Result<UValue, TError> AndThen<UValue>(Func<TValue, Result<UValue, TError>> mapFunction)
        {
            return Match(
                okay => mapFunction(okay),
                error => error);
        }

        public Result<TValue, UError> Or<UError>(Result<TValue, UError> alternative)
        {
            return Match(
                okay => okay,
                _err => alternative);
        }

        public Result<TValue, UError> OrElse<UError>(Func<TError, Result<TValue, UError>> mapFunction)
        {
            return Match(
                okay => okay,
                error => mapFunction(error));
        }

        public Result<TValue, UError> OrElse<UError>(Func<Result<TValue, UError>> operation)
        {
            return Match(
                okay => okay,
                _err => operation());
        }

        public Option<TValue> Okay()
        {
            return Match(
                okay => okay.Some(),
                error => default);
        }

        public Option<TError> Error()
        {
            return Match(
                okay => default,
                error => error.Some());
        }

        public TValue ValueOrThrow(string message = null)
        {
            return Match(
                okay => okay,
                error => throw new InvalidOperationException(message));
        }

        public TValue ValueOr(TValue other)
        {
            return Match(
                okay => okay,
                error => other);
        }

        public TError ErrorOrThrow(string message = null)
        {
            return Match(
                okay => throw new InvalidOperationException(message),
                error => error);
        }

        public bool Contains(TValue value)
        {
            return Match(
                okay => okay.Equals(value),
                error => false);
        }

        public bool ContainsError(TError value)
        {
            return Match(
                okay => false,
                error => error.Equals(value));
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
