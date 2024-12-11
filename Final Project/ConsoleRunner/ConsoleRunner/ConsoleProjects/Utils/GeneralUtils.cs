namespace ConsoleRunner.ConsoleProjects.Utils
{
    public class GeneralUtils
    {
        public static int Incrementer<T>(List<T> list)
            => list.Count + 1;

        public static int Incrementer<T>(HashSet<T> list)
            => list.Count + 1;
    }
}
