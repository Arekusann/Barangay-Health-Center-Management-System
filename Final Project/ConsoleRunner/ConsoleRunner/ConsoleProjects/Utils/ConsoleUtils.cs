using ConsoleRunner.ConsoleProjects.BHCM;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConsoleRunner.ConsoleProjects.Utils
{
    internal class ConsoleUtils
    {
        private static readonly int padding = 80;

        internal static string ShowOptions(string headerText, string footerText, params string[] options)
            => ShowOptions(headerText, footerText, options.ToList());

        internal static string ShowOptions(string headerText, string footerText, IEnumerable<string> options)
        {
            options ??= Array.Empty<string>();

            Console.WriteLine($"===== {headerText} ".PadRight(padding, '='));
            PrintList(options);
            Console.WriteLine("".PadRight(padding, '='));
            Console.Write($"\n{footerText}> ");
            var input = Console.ReadLine();
            return input ?? "";
        }

        internal static void ViewList(IEnumerable<string> list, string headerName = "", bool includeEmpty = false)
            => ViewList(list, 0, 0, headerName, includeEmpty);

        internal static void ViewList(IEnumerable<string> list, int start, int length, string headerName = "", bool includeEmpty = false)
        {
            Console.Clear();

            // Print header
            Console.WriteLine($"====={(string.IsNullOrEmpty(headerName) ? null : $" {headerName} ")}".PadRight(padding, '='));

            // Check if list is empty
            if (!list.Any())
            {
                Console.WriteLine("   E M P T Y ~");
            }
            else
            {
                // Display as a table
                var table = list
                    .Paginate(start, length)
                    .Select((item, index) => new { Index = start * length + index + 1, Item = item }) // Add row numbers
                    .ToList();

                Console.WriteLine($"{"Index".PadRight(10)} | {"Value".PadRight(padding - 15)}");
                Console.WriteLine(new string('-', padding));

                foreach (var row in table)
                {
                    Console.WriteLine($"{row.Index.ToString().PadRight(10)} | {row.Item}");
                }
            }

            // Print footer
            Console.WriteLine($"=== {CurrentViewCount(list, start, length)} ".PadRight(padding, '='));


            static string CurrentViewCount(IEnumerable<string> list, int start, int length)
            {
                var total = list.Count();
                var currentStart = list.Any() ? 1 : 0;
                var currentEnd = total;

                if (length > 0)
                {
                    var curStart = start * length + 1;
                    currentStart = curStart >= total ? total : curStart;
                    var curEnd = currentStart + (length - 1);
                    currentEnd = curEnd >= total ? total : curEnd;
                }

                return $"{currentStart}-{currentEnd} out of {total}";
            }
        }

        internal static IEnumerable<T>? SearchListBaseModel<T>(IEnumerable<T> records) where T : BaseModel
        {
            Console.Clear();
            var typeName = typeof(T).Name;

            Console.WriteLine($"===== Search ".PadRight(padding, '='));
            Console.Write($"Search {typeName.ToLower()} (ID/Name, supports * and ? wildcards)> ");
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input)) return Enumerable.Empty<T>();

            // Convert wildcard patterns (* -> .*, ? -> .)
            var pattern = "^" + Regex.Escape(input)
                                  .Replace("\\*", ".*")
                                  .Replace("\\?", ".") + "$";

            return records.Where(w =>
                w.Id.ToString() == input || // Match exact ID
                Regex.IsMatch(w.Name ?? "", pattern, RegexOptions.IgnoreCase)); // Match Name with wildcards
        }

        // ----- PRIVATES -----
        private static void PrintList(IEnumerable<string> list, bool includeEmpty = false)
        {
            if (!list.Any())
            {
                Console.WriteLine("   E M P T Y ~");
                return;
            }

            foreach (var l in list)
            {
                if (includeEmpty)
                {
                    Console.WriteLine(l);
                    continue;
                }

                if (!string.IsNullOrEmpty(l))
                    Console.WriteLine(l);
            }
        }
    }
}
