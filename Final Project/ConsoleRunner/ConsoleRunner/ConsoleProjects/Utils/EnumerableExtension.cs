namespace ConsoleRunner.ConsoleProjects.Utils
{
    public static class EnumerableExtension
    {
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int start, int length)
        {
            if (length > 0)
            {
                if (start != 0)
                    start *= length;

                return source.Skip(start).Take(length);
            }

            return source;
        }
    }
}
