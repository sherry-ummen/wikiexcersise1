using System;

namespace Wiki
{

    public static class ResultExtensions
    {

        public static Result<T> ToResult<T>(this T obj)
        {
            return obj == null ?
                Result.Fail<T>(string.Format("Object of type {0} found to be null!", typeof(T))) :
                Result<T>.Ok(obj);
        }

        public static Result OnSuccess(this Result result, Action action)
        {
            if (result.IsFailure)
                return result;
            action();
            return Result.Ok();
        }

        public static Result OnSuccess(this Result result, Action<Result> action)
        {
            if (result.IsFailure)
                return result;
            action(result);
            return Result.Ok();
        }

        public static Result OnSuccess(this Result result, Func<Result, Result> func)
        {
            return result.IsFailure ? result : func(result);
        }

        public static Result<ResultTypeOut> OnSuccess<ResultTypeIn, ResultTypeOut>(this Result<ResultTypeIn> result, Func<Result<ResultTypeIn>, Result<ResultTypeOut>> func)
        {
            return result.IsFailure ? Result<ResultTypeOut>.Fail<ResultTypeOut>(result.Error) : func(result);
        }


        public static Result OnFailure(this Result result, Action action)
        {
            if (result.IsFailure)
            {
                action();
            }
            return result;
        }

        public static Result OnFailure(this Result result, Action<Result> action)
        {
            if (result.IsFailure)
            {
                action(result);
            }
            return result;
        }

        public static Result OnFailure(this Result result, Func<Result, Result> func)
        {
            return result.IsFailure ? func(result) : result;
        }

        public static Result OnBoth(this Result result, Action<Result> action)
        {
            action(result);
            return result;
        }

        public static T OnBoth<T>(this Result result, Func<Result, T> func)
        {
            return func(result);
        }
    }
}
