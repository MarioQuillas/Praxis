namespace Praxis.Extensions
{
    using System;

    public static class FuncExtensions
    {
        public static Func<TResult> BindFunc<TSource, TResult>(
            this Func<TSource> @this,
            Func<TSource, Func<TResult>> fn) => fn(@this());

        public static Func<T> JoinFunc<T>(this Func<Func<T>> @this) => @this.BindFunc(x => x);

        public static Func<TResult> Select<TSource, TResult>(this Func<TSource> @this, Func<TSource, TResult> fn) =>
            () => fn(@this());

        public static Func<TResult> SelectMany<TSource, TSelector, TResult>(
            this Func<TSource> @this,
            Func<TSource, Func<TSelector>> selector,
            Func<TSource, TSelector, TResult> resultSelector) =>
            @this.BindFunc(x => selector(x).BindFunc(y => resultSelector(x, y).WrapFunc()));

        public static Func<TResult> WrapFunc<TResult>(this TResult @this) => () => @this;
    }
}