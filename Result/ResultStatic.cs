using OptionType;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResultType
{
    public static class Result
    {
        public static IEnumerable<TError> GetErrors<TValue, TError>(this IEnumerable<Result<TValue, TError>> results)
        {
            return results
                .Where(r => r.IsError)
                .Select(r => r.ErrorOrThrow());
        }
        public static IEnumerable<TValue> GetValues<TValue, TError>(this IEnumerable<Result<TValue, TError>> results)
        {
            return results
                .Where(r => r.IsOkay)
                .Select(r => r.ValueOrThrow());
        }

        public static IEnumerable<TValue> ValuesOrThrow<TValue, TError>(this IEnumerable<Result<TValue, TError>> results)
        {
            return results
                .Select(r => r.ValueOrThrow())
                .ToList();
        }

        public static Result<TValue, TError> Okay<TValue, TError>(TValue value)
        {
            return new Result<TValue, TError>(value);
        }

        public static Result<TValue, TError> Error<TValue, TError>(TError error)
        {
            return new Result<TValue, TError>(error);
        }

        public static Result<Okay, Error> Try(Action action)
        {
            try
            {
                action();
                return new Okay();
            }
            catch (Exception e)
            {
                return new Error(e.Message);
            }
        }

        public static Result<TValue, Error> Try<TValue>(Func<TValue> func)
        {
            try
            {
                return func();
            }
            catch (Exception e)
            {
                return new Error(e.Message);
            }
        }

        public static Result<UValue, Error> TryCatch<TValue, UValue>(this Result<TValue, Error> result, Func<TValue, UValue> tryFunc)
        {
            return result.Match(
                value => Try(() => tryFunc(value)),
                error => error);
        }

        public static Result<Okay, Error> TryCatch<TValue>(this Result<TValue, Error> result, Action<TValue> tryAction)
        {
            return result.Match(
                value => Try(() => tryAction(value)),
                error => error);
        }

        public static Result<(AValue, BValue), TError> CombineWith<AValue, BValue, TError>(
            this Result<AValue, TError> firstResult,
            Func<Result<BValue, TError>> function)
        {
            return firstResult.Match(
                okay =>
                {
                    var result = function();
                    return result.Match(
                        other_okay => Okay<(AValue, BValue), TError>((okay, other_okay)),
                        error => error);
                },
                error => error);
        }

        public static Result<(AValue, BValue, CValue), TError> CombineWith<AValue, BValue, CValue, TError>(
            this Result<(AValue, BValue), TError> firstResult,
            Func<Result<CValue, TError>> function)
        {
            return firstResult.Match(
                okay =>
                {
                    var result = function();
                    return result.Match(
                        other_okay => Okay<(AValue, BValue, CValue), TError>((okay.Item1, okay.Item2, other_okay)),
                        error => error);
                },
                error => error);
        }

        public static Result<(AValue, BValue, CValue, DValue), TError> CombineWith<AValue, BValue, CValue, DValue, TError>(
            this Result<(AValue, BValue, CValue), TError> firstResult,
            Func<Result<DValue, TError>> function)
        {
            return firstResult.Match(
                okay =>
                {
                    var result = function();
                    return result.Match(
                        other_okay =>
                        {
                            var new_okay = (okay.Item1, okay.Item2, okay.Item3, other_okay);
                            return Okay<(AValue, BValue, CValue, DValue), TError>(new_okay);
                        },
                        error => error);
                },
                error => error);
        }

        public static Result<(AValue, BValue, CValue, DValue, EValue), TError> CombineWith<AValue, BValue, CValue, DValue, EValue, TError>(
            this Result<(AValue, BValue, CValue, DValue), TError> firstResult,
            Func<Result<EValue, TError>> function)
        {
            return firstResult.Match(
                okay =>
                {
                    var result = function();
                    return result.Match(
                        other_okay =>
                        {
                            var new_okay = (okay.Item1, okay.Item2, okay.Item3, okay.Item4, other_okay);
                            return Okay<(AValue, BValue, CValue, DValue, EValue), TError>(new_okay);
                        },
                        error => error);
                },
                error => error);
        }

        public static Result<(AValue, BValue, CValue, DValue, EValue, FValue), TError> 
            CombineWith<AValue, BValue, CValue, DValue, EValue, FValue, TError>(
            this Result<(AValue, BValue, CValue, DValue, EValue), TError> firstResult,
            Func<Result<FValue, TError>> function)
        {
            return firstResult.Match(
                okay =>
                {
                    var result = function();
                    return result.Match(
                        other_okay =>
                        {
                            var new_okay = (okay.Item1, okay.Item2, okay.Item3, okay.Item4, okay.Item5, other_okay);
                            return Okay<(AValue, BValue, CValue, DValue, EValue, FValue), TError>(new_okay);
                        },
                        error => error);
                },
                error => error);
        }

        public static Result<(AValue, BValue), TError> CombineWith<AValue, BValue, TError>(
            this Result<AValue, TError> firstResult,
            Result<BValue, TError> second)
        {
            return firstResult.Match(
                okay =>
                {
                    return second.Match(
                        other_okay => Okay<(AValue, BValue), TError>((okay, other_okay)),
                        error => error);
                },
                error => error);
        }

        public static Result<(AValue, BValue, CValue), TError> CombineWith<AValue, BValue, CValue, TError>(
            this Result<(AValue, BValue), TError> firstResult,
            Result<CValue, TError> third)
        {
            return firstResult.Match(
                okay =>
                {
                    return third.Match(
                        other_okay => Okay<(AValue, BValue, CValue), TError>((okay.Item1, okay.Item2, other_okay)),
                        error => error);
                },
                error => error);
        }

        public static Result<(AValue, BValue, CValue, DValue), TError> CombineWith<AValue, BValue, CValue, DValue, TError>(
            this Result<(AValue, BValue, CValue), TError> firstResult,
            Result<DValue, TError> fourth)
        {
            return firstResult.Match(
                okay =>
                {
                    return fourth.Match(
                        other_okay =>
                        {
                            var new_okay = (okay.Item1, okay.Item2, okay.Item3, other_okay);
                            return Okay<(AValue, BValue, CValue, DValue), TError>(new_okay);
                        },
                        error => error);
                },
                error => error);
        }

        public static Result<(AValue, BValue, CValue, DValue, EValue), TError> CombineWith<AValue, BValue, CValue, DValue, EValue, TError>(
            this Result<(AValue, BValue, CValue, DValue), TError> firstResult,
            Result<EValue, TError> fifth)
        {
            return firstResult.Match(
                okay =>
                {
                    return fifth.Match(
                        other_okay =>
                        {
                            var new_okay = (okay.Item1, okay.Item2, okay.Item3, okay.Item4, other_okay);
                            return Okay<(AValue, BValue, CValue, DValue, EValue), TError>(new_okay);
                        },
                        error => error);
                },
                error => error);
        }

        public static Result<(AValue, BValue, CValue, DValue, EValue, FValue), TError>
            CombineWith<AValue, BValue, CValue, DValue, EValue, FValue, TError>(
            this Result<(AValue, BValue, CValue, DValue, EValue), TError> firstResult,
            Result<FValue, TError> sixth)
        {
            return firstResult.Match(
                okay =>
                {
                    return sixth.Match(
                        other_okay =>
                        {
                            var new_okay = (okay.Item1, okay.Item2, okay.Item3, okay.Item4, okay.Item5, other_okay);
                            return Okay<(AValue, BValue, CValue, DValue, EValue, FValue), TError>(new_okay);
                        },
                        error => error);
                },
                error => error);
        }
    }
}