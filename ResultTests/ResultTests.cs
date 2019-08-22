using System;
using OptionType;
using ResultType;
using Xunit;

namespace ResultTests
{
    public class ResultTests
    {
        private enum TestValue { One, Two, Three }

        private enum TestError { A, B }
        
        [Fact]
        public void Okay_GivenNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => Result<string, string>.Okay(null));
        }

        [Fact]
        public void Error_GivenNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => Result<string, string>.Error(null));
        }

        [Fact]
        public void Okay_OkayAndErrorSameType_ReturnsOkay()
        {
            var result = Result<int, int>.Okay(2);

            Assert.True(result.IsOkay);
        }

        [Fact]
        public void Error_OkayAndErrorSameType_ReturnsError()
        {
            var result = Result<int, int>.Error(2);

            Assert.True(result.IsError);
        }

        [Fact]
        public void IsOkay_GivenOkay_ReturnsTrue()
        {
            var result = Result<string, int>.Okay("is okay");

            Assert.True(result.IsOkay);
        }

        [Fact]
        public void IsOkay_GivenError_ReturnsFalse()
        {
            var result = Result<int, string>.Error("is error");

            Assert.False(result.IsOkay);
        }

        [Fact]
        public void IsError_GivenOkay_ReturnsFalse()
        {
            var result = Result<string, int>.Okay("is okay");

            Assert.False(result.IsError);
        }

        [Fact]
        public void IsError_GivenError_ReturnsTrue()
        {
            var result = Result<int, string>.Error("is error");

            Assert.True(result.IsError);
        }

        [Fact]
        public void Match_GivenOkay_ExecutesOkayAction()
        {
            var actionExecuted = false;
            var result = Result<string, int>.Okay("okay");

            result.Match(_ => actionExecuted = true, _ => { });

            Assert.True(actionExecuted);
        }

        [Fact]
        public void Match_GivenOkay_DoesNotExecuteErrorAction()
        {
            var result = Result<string, int>.Okay("okay");

            result.Match(_ => { }, _ => throw new Exception());
        }

        [Fact]
        public void Match_GivenError_DoesNotExecuteOkayAction()
        {
            var result = Result<string, int>.Error(1);

            result.Match(_ => throw new Exception(), _ => { });
        }

        [Fact]
        public void Match_GivenError_ExecutesErrorAction()
        {
            var actionExecuted = false;
            var result = Result<string, int>.Error(0);

            result.Match(_ => { }, _ => actionExecuted = true);

            Assert.True(actionExecuted);
        }

        [Fact]
        public void MatchOkay_GivenOkay_ExecutesAction()
        {
            var actionExecuted = false;
            var result = Result<string, int>.Okay("okay");

            result.MatchOkay(_ => actionExecuted = true);

            Assert.True(actionExecuted);
        }

        [Fact]
        public void MatchOkay_GivenError_DoesNotExecuteAction()
        {
            var result = Result<string, int>.Error(1);

            result.MatchOkay(_ => throw new Exception());
        }

        [Fact]
        public void MatchError_GivenOkay_DoesNotExecuteAction()
        {
            var result = Result<string, int>.Okay("okay");

            result.MatchError(_ => throw new Exception());
        }

        [Fact]
        public void MatchError_GivenError_ExecutesAction()
        {
            var actionExecuted = false;
            var result = Result<string, int>.Error(1);

            result.MatchError(_ => actionExecuted = true);

            Assert.True(actionExecuted);
        }

        [Fact]
        public void Contains_GivenSameValue_ReturnsTrue()
        {
            var result = Result<TestValue, TestError>.Okay(TestValue.Two);

            Assert.True(result.Contains(TestValue.Two));
        }

        [Fact]
        public void Contains_GivenDifferentValue_ReturnsFalse()
        {
            var result = Result<TestValue, TestError>.Okay(TestValue.Two);

            Assert.False(result.Contains(TestValue.One));
        }

        [Fact]
        public void Contains_GivenError_ReturnsFalse()
        {
            var result = Result<TestValue, TestError>.Error(TestError.A);

            Assert.False(result.Contains(TestValue.One));
        }

        [Fact]
        public void ContainsError_GivenOkay_ReturnsFalse()
        {
            var result = Result<TestValue, TestError>.Okay(TestValue.Two);

            Assert.False(result.ContainsError(TestError.A));
        }

        [Fact]
        public void ContainsError_GivenSameError_ReturnsTrue()
        {
            var result = Result<TestValue, TestError>.Error(TestError.A);

            Assert.True(result.ContainsError(TestError.A));
        }

        [Fact]
        public void ContainsError_GivenDifferentError_ReturnsFalse()
        {
            var result = Result<TestValue, TestError>.Error(TestError.A);

            Assert.False(result.ContainsError(TestError.B));
        }

        [Fact]
        public void Map_GivenOkay_ReturnsMappedResult()
        {
            var result = Result<int, TestError>.Okay(2);

            var stringResult = result.Map(i => i.ToString());

            Assert.True(stringResult.Contains("2"));
        }

        [Fact]
        public void Map_GivenError_ReturnsSameError()
        {
            var result = Result<int, TestError>.Error(TestError.B);

            var stringResult = result.Map(i => i.ToString());

            Assert.True(stringResult.ContainsError(TestError.B));
        }

        [Fact]
        public void Map_GivenValueAndFunctionThatReturnsResult_ReturnsMappedValue()
        {
            Result<string, TestError> getStringOrError(int value)
            {
                return value.ToString();
            }

            var result = Result<int, TestError>.Okay(5);

            Result<string, TestError> stringResult = result.AndThen(getStringOrError);

            Assert.True(stringResult.Contains("5"));
        }

        [Fact]
        public void Map_GivenErrorAndFunctionThatReturnsResult_ReturnsError()
        {
            Result<string, TestError> getStringOrError(int value)
            {
                return value.ToString();
            }

            var result = Result<int, TestError>.Error(TestError.B);

            Result<string, TestError> stringResult = result.AndThen(getStringOrError);

            Assert.True(stringResult.ContainsError(TestError.B));
        }

        [Fact]
        public void MapError_GivenOkay_ReturnsMappedOkay()
        {
            var result = Result<string, int>.Okay("a");

            Result<string, string> strResult = result.MapError(i => i.ToString());

            Assert.True(strResult.IsOkay);
        }

        [Fact]
        public void MapError_GivenError_ReturnsMappedError()
        {
            var result = Result<string, int>.Error(2);

            Result<string, string> strResult = result.MapError(i => i.ToString());

            Assert.True(strResult.ContainsError("2"));
        }
        
        [Fact]
        public void AsEnumerable_GivenOkay_ReturnsSingleValue()
        {
            var result = Result<TestValue, TestError>.Okay(TestValue.Three);
            
            Assert.Single(result.AsEnumerable(), TestValue.Three);
        }

        [Fact]
        public void AsEnumerable_GivenError_ReturnsEmpty()
        {
            var result = Result<TestValue, TestError>.Error(TestError.A);

            Assert.Empty(result.AsEnumerable());
        }

        [Fact]
        public void Equals_GivenSameOkay_AreEqual()
        {
            var result = Result<TestValue, TestError>.Okay(TestValue.One);
            var same = Result<TestValue, TestError>.Okay(TestValue.One);

            Assert.Equal(result, same);
        }

        [Fact]
        public void Equals_GivenDifferentOkayValues_AreNotEqual()
        {
            var result = Result<TestValue, TestError>.Okay(TestValue.One);
            var different = Result<TestValue, TestError>.Okay(TestValue.Two);

            Assert.NotEqual(result, different);
        }

        [Fact]
        public void Equals_GivenSameOkayAndError_AreNotEqual()
        {
            var result = Result<TestValue, TestValue>.Okay(TestValue.One);
            var different = Result<TestValue, TestValue>.Error(TestValue.One);

            Assert.NotEqual(result, different);
        }

        [Fact]
        public void Equals_GivenSameError_AreEqual()
        {
            var result = Result<TestValue, TestError>.Error(TestError.A);
            var same = Result<TestValue, TestError>.Error(TestError.A);

            Assert.Equal(result, same);
        }

        [Fact]
        public void Equals_GivenDifferentErrorValues_AreNotEqual()
        {
            var result = Result<TestValue, TestError>.Error(TestError.A);
            var different = Result<TestValue, TestError>.Error(TestError.B);

            Assert.NotEqual(result, different);
        }

        [Fact]
        public void Equals_GivenErrorAndOkay_AreNotEqual()
        {
            var okay = Result<TestValue, TestError>.Error(TestError.A);
            var error = Result<TestValue, TestError>.Okay(TestValue.One);

            Assert.NotEqual(okay, error);
        }

        [Fact]
        public void GetHashCode_GivenSameOkayValue_AreEqual()
        {
            var result = Result<TestValue, TestError>.Okay(TestValue.One);
            var same = Result<TestValue, TestError>.Okay(TestValue.One);

            Assert.Equal(result.GetHashCode(), same.GetHashCode());
        }

        [Fact]
        public void GetHashCode_GivenDifferentOkayValues_AreNotEqual()
        {
            var result = Result<TestValue, TestError>.Okay(TestValue.One);
            var different = Result<TestValue, TestError>.Okay(TestValue.Two);

            Assert.NotEqual(result.GetHashCode(), different.GetHashCode());
        }

        [Fact]
        public void GetHashCode_GivenSameOkayAndError_AreNotEqual()
        {
            var result = Result<TestValue, TestValue>.Okay(TestValue.One);
            var different = Result<TestValue, TestValue>.Error(TestValue.One);

            Assert.NotEqual(result.GetHashCode(), different.GetHashCode());
        }

        [Fact]
        public void GetHashCode_GivenSameError_AreEqual()
        {
            var result = Result<TestValue, TestError>.Error(TestError.A);
            var same = Result<TestValue, TestError>.Error(TestError.A);

            Assert.Equal(result.GetHashCode(), same.GetHashCode());
        }

        [Fact]
        public void GetHashCode_GivenDifferentErrorValues_AreNotEqual()
        {
            var result = Result<TestValue, TestError>.Error(TestError.A);
            var different = Result<TestValue, TestError>.Error(TestError.B);

            Assert.NotEqual(result.GetHashCode(), different.GetHashCode());
        }

        [Fact]
        public void ToString_GivenOkay()
        {
            var okay = Result<int, TestError>.Okay(1);

            Assert.Equal("Okay(1)", okay.ToString());
        }

        [Fact]
        public void ToString_GivenError()
        {
            var okay = Result<TestValue, int>.Error(1);

            Assert.Equal("Error(1)", okay.ToString());
        }

        [Fact]
        public void OkayAndError_WithSameTypeAndValue_NotEqual()
        {
            var okay = Result<int, int>.Okay(1);
            var error = Result<int, int>.Error(1);

            Assert.NotEqual(okay, error);
            Assert.True(okay.IsOkay);
            Assert.True(error.IsError);
            Assert.False(okay.IsError);
            Assert.False(error.IsOkay);
        }

        [Fact]
        public void ImplicitCastTest()
        {
            Result<int, string> okay = 3;
            Result<int, string> error = "three";

            Assert.True(okay.IsOkay);
            Assert.True(error.IsError);
        }

        [Fact]
        public void ValueOrThrow_GivenOkay_ReturnsValue()
        {
            Result<int, string> okay = 2;

            Assert.Equal(2, okay.ValueOrThrow());
        }

        [Fact]
        public void ValueOrThrow_GivenError_ThrowsException()
        {
            Result<int, string> error = "error";

            Assert.Throws<InvalidOperationException>(() => error.ValueOrThrow());
        }

        [Fact]
        public void Okay_GivenOkay_ReturnsSome()
        {
            var okay = Result<int, string>.Okay(5);

            Assert.Equal(5.Some(), okay.Okay());
        }

        [Fact]
        public void Okay_GivenError_ReturnsNone()
        {
            var error = Result<int, string>.Error("e");

            Assert.Equal(5.None(), error.Okay());
        }

        [Fact]
        public void Error_GivenOkay_ReturnsNone()
        {
            var okay = Result<int, string>.Okay(5);

            Assert.Equal(string.Empty.None(), okay.Error());
        }

        [Fact]
        public void Error_GivenError_ReturnsSome()
        {
            var error = Result<int, string>.Error("e");

            Assert.Equal("e".Some(), error.Error());
        }

        /// <summary>
        /// This odd behaviour is the best I can do given C#'s insistence that all objects have a default value
        /// </summary>
        [Fact]
        public void Default_BehavesOddlyForNullableErrorType()
        {
            Result<int, string> result = default;

            Assert.True(result.IsError);
            Assert.False(result.Error().IsSome);
        }

        [Fact]
        public void Default_BehavesNormallyForNonNullableErrorType()
        {
            Result<int, int> result = default;

            Assert.True(result.IsError);
            Assert.True(result.Error().IsSome);
        }

        [Fact]
        public void CombineWith_SingleResult_BothOkay()
        {
            Result<int, string> result_a = 1;
            Func<Result<bool, string>> get_result_b = () => true;

            var combined = result_a.CombineWith(get_result_b);

            Assert.True(combined.Contains((1, true)));
        }

        [Fact]
        public void CombineWith_SingleResult_FirstError()
        {
            Result<int, string> result_a = "foo";
            Func<Result<bool, string>> get_result_b = () => true;

            var combined = result_a.CombineWith(get_result_b);

            Assert.True(combined.ContainsError("foo"));
        }

        [Fact]
        public void CombineWith_SingleResult_SecondError()
        {
            Result<int, string> result_a = 1;
            Func<Result<bool, string>> get_result_b = () => "bar";

            var combined = result_a.CombineWith(get_result_b);

            Assert.True(combined.ContainsError("bar"));
        }

        [Fact]
        public void CombineWith_FiveResults_AllOkay()
        {
            Result<int, string> result_a = 1;
            Result<bool, string> result_b = true;
            Result<string, string> result_c = Result.Okay<string, string>("foo");
            Result<double, string> result_d = 3.141;
            Result<TestValue, string> result_e = TestValue.One;
            Result<Okay, string> result_f = new Okay();

            var combined = result_a
                .CombineWith(() => result_b)
                .CombineWith(() => result_c)
                .CombineWith(() => result_d)
                .CombineWith(() => result_e)
                .CombineWith(() => result_f);

            Assert.True(combined.Contains((1, true, "foo", 3.141, TestValue.One, new Okay())));
        }

        [Fact]
        public void CombineWith_FiveResults_ErrorInMiddle()
        {
            Result<int, string> result_a = 1;
            Result<bool, string> result_b = true;
            Result<string, string> result_c = Result.Error<string, string>("foo");
            Result<double, string> result_d = 3.141;
            Result<TestValue, string> result_e = TestValue.One;
            Result<Okay, string> result_f = new Okay();

            var combined = result_a
                .CombineWith(() => result_b)
                .CombineWith(() => result_c)
                .CombineWith(() => result_d)
                .CombineWith(() => result_e)
                .CombineWith(() => result_f);

            Assert.True(combined.ContainsError("foo"));
        }
    }
}
