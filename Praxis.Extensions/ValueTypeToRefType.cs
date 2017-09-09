namespace Praxis.Extensions
{
    public class ValueTypeToRefType<T>
        where T : struct
    {
        private ValueTypeToRefType(T value)
        {
            this.Value = value;
        }

        public T Value { get; }

        public static ValueTypeToRefType<T> CreateRefType(T value) => new ValueTypeToRefType<T>(value);
    }
}