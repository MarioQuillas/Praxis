namespace Praxis.Extensions
{
    using System;

    public static class OptionExtensions
    {
        public static Option<TResult> Select<TSource, TResult>(this Option<TSource> @this, Func<TSource, TResult> fn)
            where TSource : class where TResult : class => @this.Map(fn);

        // public static Option<T> MaybeJoin<T>(this Option<Option<T>> @this) where T : class
        // {

        // }
        public static Option<TResult> SelectMany<TSource, TSelector, TResult>(
            this Option<TSource> @this,
            Func<TSource, Option<TSelector>> selector,
            Func<TSource, TSelector, TResult> resultSelector)
            where TSource : class where TSelector : class where TResult : class =>
            @this.Bind(x => selector(x).Bind(y => resultSelector(x, y).WrapOption()));

        /// <summary>
        /// We should think of the category dotnet as the one not having nulls
        /// the wrapping of the null to a maybe is to make the result type of a function honest about its signature
        /// </summary>
        /// <typeparam name="T">Generic parameter</typeparam>
        /// <param name="this">The this.</param>
        /// <returns>Wrapped type as an Option</returns>
        public static Option<T> WrapOption<T>(this T @this)
            where T : class => @this == null ? Option<T>.CreateEmpty() : Option<T>.Create(@this);
    }
}