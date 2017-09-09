// ReSharper disable StyleCop.SA1502

namespace Praxis.Extensions
{
    using System;

    public class Result<TSuccessSource, TFailure>
    {
        private Result(bool isSuccess)
        {
            this.IsSuccess = isSuccess;
        }

        public TFailure FailureValue { get; private set; }

        public bool IsSuccess { get; }

        // private readonly TSuccessSource successValue;
        // this is not actually immutable but we can let it pass in order to make it clearer in the constructor
        public TSuccessSource SuccessValue { get; private set; }

        public static Result<TSuccessSource, TFailure> FailWith(TFailure value) =>
            new Result<TSuccessSource, TFailure>(false) { FailureValue = value };

        // public Result<TSuccess, TFailure> Bind(Func<TSuccess, Result<TSuccess, TFailure>> fn)
        // {
        // return this.IsSuccess
        // ? fn(this.SuccessValue)
        // : this;
        // }
        public static Result<TSuccessSource, TFailure> SucceedWith(TSuccessSource value) =>
            new Result<TSuccessSource, TFailure>(true) { SuccessValue = value };

        public Result<TSuccessResult, TFailure> Bind<TSuccessResult>(
            Func<TSuccessSource, Result<TSuccessResult, TFailure>> fn) =>
            this.IsSuccess ? fn(this.SuccessValue) : Result<TSuccessResult, TFailure>.FailWith(this.FailureValue);

        /// <summary>
        /// Maps the specified function.
        /// Again, here we are assuming that the func delegate, fn, is actually a pure function.
        /// One assumption we always make in this functional extensions is that all delegates are always pure.
        /// </summary>
        /// <typeparam name="TSuccessResult">The type of the success result.</typeparam>
        /// <param name="fn">The function.</param>
        /// <returns>The result of the function wrapped in the Result monad</returns>
        public Result<TSuccessResult, TFailure> Map<TSuccessResult>(Func<TSuccessSource, TSuccessResult> fn) =>
            this.IsSuccess
                ? Result<TSuccessResult, TFailure>.SucceedWith(fn(this.SuccessValue))
                : Result<TSuccessResult, TFailure>.FailWith(this.FailureValue);

        // return this.Bind(x => fn(x).WrapResult<TSuccessResult, TFailure>());
    }
}