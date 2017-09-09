namespace Praxis.Extensions
{
    using System;

    public static class ResultExtensions
    {
        public static Result<TSuccessSource, TFailure> JoinResult<TSuccessSource, TFailure>(
            this Result<Result<TSuccessSource, TFailure>, TFailure> @this) => @this.Bind(x => x);

        public static Result<TSuccessResult, TFailure> Select<TSuccessSource, TSuccessResult, TFailure>(
            this Result<TSuccessSource, TFailure> @this,
            Func<TSuccessSource, TSuccessResult> fn) => @this.Map(fn);

        public static Result<TSuccessResult, TFailure> SelectMany<TSuccessSource, TSuccessSelector, TSuccessResult, TFailure>(
                this Result<TSuccessSource, TFailure> @this,
                Func<TSuccessSource, Result<TSuccessSelector, TFailure>> selector,
                Func<TSuccessSource, TSuccessSelector, TSuccessResult> resultSelector) => @this.Bind(
            x => selector(x).Bind(y => resultSelector(x, y).WrapResult<TSuccessResult, TFailure>()));

        public static Result<TSuccessSource, TFailure> WrapResult<TSuccessSource, TFailure>(this TSuccessSource @this) =>
            Result<TSuccessSource, TFailure>.SucceedWith(@this);
    }
}