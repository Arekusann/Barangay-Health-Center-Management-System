using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ConsoleRunner.ConsoleProjects.Utils;

namespace ConsoleRunner.ConsoleProjects.BHCM
{
    internal static class BHCMPatients
    {
        private static readonly string directoryPath = Path.Combine(@"C:\Users\rosal\Downloads\Final Project\Final Project\ConsoleRunner\ConsoleRunner\bin\Debug\net8.0", "BHCM");
        private static readonly string filePath = Path.Combine(directoryPath, "patients.json");

        private static List<Patient> _patients = LoadData();

        internal static List<Patient> Patients => _patients;

        internal static void ManagePatients()
        {
            while (true)
            {
                Console.Clear();
                var input = ConsoleUtils.ShowOptions("Patient Management", "Select an option", new List<string>
                {
                    $"1. Add {nameof(Patient)}",
                    $"2. Edit {nameof(Patient)}",
                    $"3. Delete {nameof(Patient)}",
                    $"4. View {nameof(Patient)}s",
                    $"5. Search {nameof(Patient)}s",
                    "6. Back",
                });
                switch (input)
                {
                    case "1": AddPatient(); break;
                    case "2": EditPatient(); break;
                    case "3": DeletePatient(); break;
                    case "4": ViewPatients(); break;
                    case "5": Search(); break;
                    case "6": return;
                    default: Console.WriteLine("Invalid selection."); break;
                }
            }
        }

        private static void AddPatient()
        {
            Console.WriteLine("Enter Patient Details:");
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Age: ");
            int age = int.Parse(Console.ReadLine());
            string gender = "";
            while (true)
            {
                Console.Write("Gender (Enter m for Male, f for Female): ");
                string genderInput = Console.ReadLine();
                if (genderInput == "m") { gender = "Male"; break; }
                else if (genderInput == "f") { gender = "Female"; break; }
                else { Console.WriteLine("Invalid input. Please enter m for Male or f for Female."); }
            }
            Console.Write("Contact: ");
            string contact = Console.ReadLine();
            Console.Write("Medical History: ");
            string medicalHistory = Console.ReadLine();

            _patients.Add(new Patient(GeneralUtils.Incrementer(_patients), name, age, gender, contact, medicalHistory));
            Console.WriteLine("Patient added successfully.");
            SaveData(_patients);
        }

        private static void EditPatient()
        {
            Console.Write("Enter Patient ID to Edit: ");
            int id = int.Parse(Console.ReadLine());
            var patient = _patients.FirstOrDefault(p => p.Id == id);
            if (patient != null)
            {
                Console.Write("New Name (leave blank to keep the same): ");
                string name = Console.ReadLine();
                if (!string.IsNullOrEmpty(name)) patient.Name = name;

                Console.Write("New Age (leave blank to keep the same): ");
                string ageStr = Console.ReadLine();
                if (!string.IsNullOrEmpty(ageStr)) patient.Age = int.Parse(ageStr);

                Console.Write("New Gender (Enter m for Male, f for Female, leave blank to keep the same): ");
                string genderInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(genderInput))
                {
                    if (genderInput == "m") { patient.Gender = "Male"; }
                    else if (genderInput == "f") { patient.Gender = "Female"; }
                    else { Console.WriteLine("Invalid input. Gender not updated."); }
                }

                Console.Write("New Contact (leave blank to keep the same): ");
                string contact = Console.ReadLine();
                if (!string.IsNullOrEmpty(contact)) patient.Contact = contact;

                Console.Write("New Medical History (leave blank to keep the same): ");
                string medicalHistory = Console.ReadLine();
                if (!string.IsNullOrEmpty(medicalHistory)) patient.MedicalHistory = medicalHistory;

                Console.WriteLine("Patient updated successfully.");
                SaveData(_patients);
            }
            else
            {
                Console.WriteLine("Patient not found.");
            }
        }

        private static void DeletePatient()
        {
            Console.Write("Enter Patient ID to Delete: ");
            int id = int.Parse(Console.ReadLine());
            var patient = _patients.FirstOrDefault(p => p.Id == id);
            if (patient != null)
            {
                _patients.Remove(patient);
                Console.WriteLine("Patient deleted successfully.");
                SaveData(_patients);
            }
            else
            {
                Console.WriteLine("Patient not found.");
            }
        }

        private static void ViewPatients()
            => View(_patients);

        private static void Search()
        {
            Console.Clear();
            Console.WriteLine("Enter search term (supports * and ? wildcards or ID): ");
            string input = Console.ReadLine();

            if (string.IsNullOrEmpty(input)) return;

            var pattern = "^" + System.Text.RegularExpressions.Regex.Escape(input)
                                  .Replace("\\*", ".*")
                                  .Replace("\\?", ".") + "$";

            var results = _patients.Where(p =>
                p.Id.ToString() == input || // Match exact ID
                System.Text.RegularExpressions.Regex.IsMatch(p.Name ?? "", pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase) // Match name with wildcards
            ).ToList();

            View(results);
        }

        private static void View(List<Patient> patients)
        {
            // Header for the table
            Console.WriteLine("ID    | Name                 | Age | Gender | Contact        | Medical History");
            Console.WriteLine("------|----------------------|-----|--------|----------------|-------------------------------");

            // Rows for each patient
            foreach (var patient in patients)
            {
                Console.WriteLine(
                    $"{patient.Id.ToString().PadRight(6)}| " +
                    $"{patient.Name.PadRight(22)}| " +
                    $"{patient.Age.ToString().PadRight(5)}| " +
                    $"{patient.Gender.PadRight(8)}| " +
                    $"{patient.Contact.PadRight(16)}| " +
                    $"{patient.MedicalHistory.PadRight(30)}"
                );
            }

            // Footer for navigation
            Console.WriteLine("\n[any key - exit]");
            Console.Write("> ");
            Console.ReadLine();
        }

        private static void SaveData(List<Patient> patients)
        {
            try
            {
                if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

                var jsonData = JsonSerializer.Serialize(patients, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, jsonData);
                Console.WriteLine("Data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        private static List<Patient> LoadData()
        {
            try
            {
                if (!File.Exists(filePath)) return new List<Patient>();

                var jsonData = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<Patient>>(jsonData) ?? new List<Patient>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                return new List<Patient>();
            }
        }
    }
}

