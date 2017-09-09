namespace Praxis.Extensions
{
    public static class ValueTypeToRefTypeExtensions
    {
        public static ValueTypeToRefType<T> ToRefType<T>(this T @this)
            where T : struct => ValueTypeToRefType<T>.CreateRefType(@this);
    }
}