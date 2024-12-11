using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ConsoleRunner.ConsoleProjects.Utils;

namespace ConsoleRunner.ConsoleProjects.BHCM
{
    internal static class BHCMDoctors
    {
        private static readonly List<Doctor> _doctors = LoadData();
        internal static List<Doctor> Doctors => _doctors;

        // Define the file path in the specified hardcoded location
        private static readonly string directoryPath = Path.Combine(@"C:\Users\rosal\Downloads\Final Project\Final Project\ConsoleRunner\ConsoleRunner\bin\Debug\net8.0", "BHCM");
        private static readonly string filePath = Path.Combine(directoryPath, "doctors.json");

        internal static void LoadSampleData()
        {
            // Load Sample Doctors
            _doctors.Clear();
            _doctors.AddRange(SampleData.LoadSampleDoctors());
        }

        internal static void ManageDoctors()
        {
            while (true)
            {
                Console.Clear();
                var input = ConsoleUtils.ShowOptions("Doctor Management", "Select an option", new List<string>
                {
                    $"1. Add {nameof(Doctor)}",
                    $"2. Edit {nameof(Doctor)}",
                    $"3. Delete {nameof(Doctor)}",
                    $"4. View {nameof(Doctor)}s",
                    $"5. Search {nameof(Doctor)}s",
                    "6. Back",
                });
                switch (input)
                {
                    case "1": AddDoctor(); break;
                    case "2": EditDoctor(); break;
                    case "3": DeleteDoctor(); break;
                    case "4": ViewDoctors(); break;
                    case "5": Search(); break;
                    case "6": return;
                    default: Console.WriteLine("Invalid selection."); break;
                }
            }
        }

        private static void AddDoctor()
        {
            Console.WriteLine("Enter Doctor Details:");
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Specialization: ");
            string specialization = Console.ReadLine();

            _doctors.Add(new Doctor(GeneralUtils.Incrementer(_doctors), name, specialization));
            Console.WriteLine("Doctor added successfully.");
            SaveData(_doctors);
        }

        private static void EditDoctor()
        {
            Console.Write("Enter Doctor ID to Edit: ");
            int id = int.Parse(Console.ReadLine());
            var doctor = _doctors.FirstOrDefault(d => d.Id == id);
            if (doctor != null)
            {
                Console.Write("New Name (leave blank to keep the same): ");
                string name = Console.ReadLine();
                if (!string.IsNullOrEmpty(name)) doctor.Name = name;

                Console.Write("New Specialization (leave blank to keep the same): ");
                string specialization = Console.ReadLine();
                if (!string.IsNullOrEmpty(specialization)) doctor.Specialization = specialization;

                Console.WriteLine("Doctor updated successfully.");
                SaveData(_doctors);
            }
            else
            {
                Console.WriteLine("Doctor not found.");
            }
        }

        private static void DeleteDoctor()
        {
            Console.Write("Enter Doctor ID to Delete: ");
            int id = int.Parse(Console.ReadLine());
            var doctor = _doctors.FirstOrDefault(d => d.Id == id);
            if (doctor != null)
            {
                _doctors.Remove(doctor);
                doctor.IsDeleted = true;
                _doctors.Add(doctor);
                Console.WriteLine("Doctor deleted successfully.");
                SaveData(_doctors);
            }
            else
            {
                Console.WriteLine("Doctor not found.");
            }
        }

        private static void ViewDoctors()
            => View(_doctors);

        private static void Search()
        {
            Console.Clear();
            Console.WriteLine("Enter search term (supports * and ? wildcards or ID): ");
            string input = Console.ReadLine();

            if (string.IsNullOrEmpty(input)) return;

            var pattern = "^" + System.Text.RegularExpressions.Regex.Escape(input)
                                  .Replace("\\*", ".*")
                                  .Replace("\\?", ".") + "$";

            var results = _doctors.Where(d =>
                d.Id.ToString() == input || // Match exact ID
                System.Text.RegularExpressions.Regex.IsMatch(d.Name ?? "", pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase) || // Match name
                System.Text.RegularExpressions.Regex.IsMatch(d.Specialization ?? "", pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase) // Match specialization
            ).ToList();

            View(results);
        }

        private static void View(List<Doctor> doctors)
        {
            // Header for the table
            Console.WriteLine("ID    | Name                 | Specialization        ");
            Console.WriteLine("------|----------------------|-----------------------");

            // Rows for each doctor
            foreach (var doctor in doctors.Where(d => !d.IsDeleted))
            {
                Console.WriteLine(
                    $"{doctor.Id.ToString().PadRight(6)}| " +
                    $"{doctor.Name.PadRight(22)}| " +
                    $"{doctor.Specialization.PadRight(23)}"
                );
            }

            // Footer for navigation
            Console.WriteLine("\n[any key - exit]");
            Console.Write("> ");
            Console.ReadLine();
        }

        // Save the current list of doctors to the JSON file
        private static void SaveData(List<Doctor> doctors)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                var jsonData = JsonSerializer.Serialize(doctors, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, jsonData);
                Console.WriteLine("Data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        // Load the list of doctors from the JSON file
        private static List<Doctor> LoadData()
        {
            try
            {
                if (!File.Exists(filePath))
                    return new List<Doctor>();

                var jsonData = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<Doctor>>(jsonData) ?? new List<Doctor>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                return new List<Doctor>();
            }
        }
    }
}
