namespace Praxis.Extensions
{
    using System;

    /// <summary>
    /// We are assuming that all the types in c# cannot be null
    /// I don't still know how to inforce this assumption in a clearer or stronger manner
    /// Not making TSource a value type because we already have for that nullable types and I don't want to 
    /// keep reference of a property in order to know if a value has or not a value
    /// Making an struct in order that it cannot be null
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    public struct Option<TSource>
        where TSource : class
    {
        // TODO : maybe to use default(T) instead of null
        private readonly TSource value;

        private Option(TSource value)
        {
            this.value = value;
        }

        public bool HasValue => this.value != null;

        public bool HasNoValue => !this.HasValue;

        public static Option<TSource> CreateEmpty()
        {
            return new Option<TSource>(null); // default(TSource));
        }

        public static Option<TSource> Create(TSource value) => new Option<TSource>(value);

        /// <summary>
        /// Maps the specified function.
        /// Here for example, event though we can do fn(null) in c#, in our minds we should think that this is not allow since it would most problably generate a null ref exception.
        /// Still to find a way to make more safe
        /// A better way to see it is : the "Option" category accepts as morphisms all the morphims form the donet category except that all empty maybes goes to empty maybes
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="fn">The function.</param>
        /// <returns>The return of fn wrapped as an option</returns>
        public Option<TResult> Map<TResult>(Func<TSource, TResult> fn)
            where TResult : class
        {
            // this doesn't make sense since the input value of a func can be null but it is most probably not null since we will operate on its values
            // normally this shouldn't even compile or the code on the selecttests shouldn't compile but we don't have non nullability in c# ... yet
            // return new Option<TResult>(fn(this.value));
            // return fn(this.value).WrapOption();

            // return this.Bind(x => fn(x).WrapOption());
            return this.value == null ? Option<TResult>.CreateEmpty() : Option<TResult>.Create(fn(this.value));
        }

        public Option<TResult> Bind<TResult>(Func<TSource, Option<TResult>> fn)
            where TResult : class
        {
            return this.value == null ? Option<TResult>.CreateEmpty() : fn(this.value);
        }

        public TSource UnWrap()
        {
            return this.value;
        }
    }
}