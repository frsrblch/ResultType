using OptionType;
using System;
using System.Collections.Generic;

namespace ResultType
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
    }
}