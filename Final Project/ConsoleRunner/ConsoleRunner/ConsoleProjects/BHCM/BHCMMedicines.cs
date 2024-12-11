using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ConsoleRunner.ConsoleProjects.Utils;

namespace ConsoleRunner.ConsoleProjects.BHCM
{
    internal static class BHCMMedicines
    {
        private static readonly List<Medicine> _medicines = LoadData();
        internal static List<Medicine> Medicines => _medicines;

        // Define the file path in the specified hardcoded location
        private static readonly string directoryPath = Path.Combine(@"C:\Users\rosal\Downloads\Final Project\Final Project\ConsoleRunner\ConsoleRunner\bin\Debug\net8.0", "BHCM");
        private static readonly string filePath = Path.Combine(directoryPath, "medicines.json");

        internal static void ManageMedicines()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Medicine Inventory Management");
                Console.WriteLine("1. Add Medicine");
                Console.WriteLine("2. View Medicines");
                Console.WriteLine("3. Edit Medicine");
                Console.WriteLine("4. Delete Medicine");
                Console.WriteLine("5. Search Medicine");
                Console.WriteLine("6. Back");
                Console.Write("Select an option: ");
                switch (Console.ReadLine())
                {
                    case "1": AddMedicine(); break;
                    case "2": ViewMedicines(); break;
                    case "3": EditMedicine(); break;
                    case "4": DeleteMedicine(); break;
                    case "5": SearchMedicines(); break;
                    case "6": return;
                    default: Console.WriteLine("Invalid selection."); break;
                }
            }
        }

        private static void AddMedicine()
        {
            Console.Clear();
            Console.WriteLine("Enter Medicine Details:");
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Stock: ");
            int stock = int.Parse(Console.ReadLine());
            Console.Write("Expiration Date (yyyy-mm-dd): ");
            DateTime expirationDate = DateTime.Parse(Console.ReadLine());

            _medicines.Add(new Medicine(name, stock, expirationDate));
            Console.WriteLine("Medicine added successfully.");
            SaveData(_medicines);
        }

        private static void ViewMedicines()
        {
            Console.Clear();
            ConsoleUtils.ViewList(
                _medicines.Select(m => $"{m.Name.PadRight(20)} | {m.Stock.ToString().PadRight(10)} | {m.ExpirationDate:yyyy-MM-dd}"),
                "Medicine Inventory");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private static void EditMedicine()
        {
            Console.Clear();
            ViewMedicines();
            Console.Write("Enter the name of the medicine to edit: ");
            string name = Console.ReadLine();

            var medicine = _medicines.FirstOrDefault(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (medicine == null)
            {
                Console.WriteLine("Medicine not found.");
                return;
            }

            Console.WriteLine("Edit Medicine Details:");
            Console.Write($"Name ({medicine.Name}): ");
            string newName = Console.ReadLine();
            Console.Write($"Stock ({medicine.Stock}): ");
            string newStockInput = Console.ReadLine();
            Console.Write($"Expiration Date ({medicine.ExpirationDate:yyyy-MM-dd}): ");
            string newDateInput = Console.ReadLine();

            if (!string.IsNullOrEmpty(newName)) medicine.Name = newName;
            if (int.TryParse(newStockInput, out int newStock)) medicine.Stock = newStock;
            if (DateTime.TryParse(newDateInput, out DateTime newDate)) medicine.ExpirationDate = newDate;

            Console.WriteLine("Medicine updated successfully.");
            SaveData(_medicines);
        }

        private static void DeleteMedicine()
        {
            Console.Clear();
            ViewMedicines();
            Console.Write("Enter the name of the medicine to delete: ");
            string name = Console.ReadLine();

            var medicine = _medicines.FirstOrDefault(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (medicine == null)
            {
                Console.WriteLine("Medicine not found.");
                return;
            }

            _medicines.Remove(medicine);
            Console.WriteLine("Medicine deleted successfully.");
            SaveData(_medicines);
        }

        private static void SearchMedicines()
        {
            Console.Clear();
            Console.WriteLine("Search Medicines (use * and ? as wildcards):");
            Console.Write("Enter search pattern: ");
            string pattern = Console.ReadLine();

            // Convert wildcard pattern to regex
            string regexPattern = "^" + System.Text.RegularExpressions.Regex.Escape(pattern)
                .Replace("\\*", ".*")
                .Replace("\\?", ".") + "$";

            var results = _medicines.Where(m =>
                System.Text.RegularExpressions.Regex.IsMatch(m.Name, regexPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                .Select(m => $"{m.Name.PadRight(20)} | {m.Stock.ToString().PadRight(10)} | {m.ExpirationDate:yyyy-MM-dd}");

            if (!results.Any())
            {
                Console.WriteLine("No medicines found matching the search criteria.");
            }
            else
            {
                ConsoleUtils.ViewList(results, "Search Results");
            }

            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        // Save the current list of medicines to the JSON file
        private static void SaveData(List<Medicine> medicines)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                var jsonData = JsonSerializer.Serialize(medicines, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        // Load the list of medicines from the JSON file
        private static List<Medicine> LoadData()
        {
            try
            {
                if (!File.Exists(filePath))
                    return new List<Medicine>();

                var jsonData = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<Medicine>>(jsonData) ?? new List<Medicine>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                return new List<Medicine>();
            }
        }
    }
}

