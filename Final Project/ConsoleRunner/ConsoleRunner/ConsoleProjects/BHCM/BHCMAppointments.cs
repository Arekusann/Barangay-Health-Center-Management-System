using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using ConsoleRunner.ConsoleProjects.Utils;

namespace ConsoleRunner.ConsoleProjects.BHCM
{
    internal static class BHCMAppointments
    {
        private static readonly List<Appointment> _appointments = LoadData();
        internal static List<Appointment> Appointments => _appointments;

        // Define the file path in the specified hardcoded location
        private static readonly string directoryPath = Path.Combine(@"C:\Users\rosal\Downloads\Final Project\Final Project\ConsoleRunner\ConsoleRunner\bin\Debug\net8.0", "BHCM");
        private static readonly string filePath = Path.Combine(directoryPath, "appointments.json");

        internal static void ManageAppointments()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Appointment Management");
                Console.WriteLine("1. Schedule Appointment");
                Console.WriteLine("2. View All Appointments");
                Console.WriteLine("3. View Appointments by Doctor");
                Console.WriteLine("4. View Appointments by Patient");
                Console.WriteLine("5. View List of Doctors");
                Console.WriteLine("6. View List of Patients");
                Console.WriteLine("7. Search Doctors");
                Console.WriteLine("8. Search Patients");
                Console.WriteLine("9. Back");
                Console.Write("Select an option: ");
                switch (Console.ReadLine())
                {
                    case "1": ScheduleAppointment(); break;
                    case "2": ViewAppointments(); break;
                    case "3": ViewAppointmentsByDoctor(); break;
                    case "4": ViewAppointmentsByPatient(); break;
                    case "5": ViewDoctorsList(); break;
                    case "6": ViewPatientsList(); break;
                    case "7": SearchDoctors(); break;
                    case "8": SearchPatients(); break;
                    case "9": return;
                    default: Console.WriteLine("Invalid selection."); break;
                }
            }
        }

        private static void ScheduleAppointment()
        {
            Console.WriteLine("Enter Appointment Details:");
            Console.Write("Patient ID: ");
            int patientId = int.Parse(Console.ReadLine());
            var patient = BHCMPatients.Patients.FirstOrDefault(p => p.Id == patientId);
            if (patient == null)
            {
                Console.WriteLine("Patient not found.");
                return;
            }

            Console.Write("Doctor ID: ");
            int doctorId = int.Parse(Console.ReadLine());
            var doctor = BHCMDoctors.Doctors.FirstOrDefault(d => d.Id == doctorId);
            if (doctor == null)
            {
                Console.WriteLine("Doctor not found.");
                return;
            }

            Console.Write("Appointment Date (yyyy-mm-dd): ");
            DateTime date = DateTime.Parse(Console.ReadLine());

            _appointments.Add(new Appointment(patientId, doctorId, date));
            Console.WriteLine("Appointment scheduled successfully.");
            SaveData(_appointments);
        }

        private static void ViewAppointments()
        {
            if (!_appointments.Any())
            {
                Console.WriteLine("No appointments scheduled.");
            }
            else
            {
                var table = _appointments.Select(app =>
                {
                    var patient = BHCMPatients.Patients.FirstOrDefault(p => p.Id == app.PatientID);
                    var doctor = BHCMDoctors.Doctors.FirstOrDefault(d => d.Id == app.DoctorID);
                    return $"{patient?.Name.PadRight(20)} | {doctor?.Name.PadRight(20)} | {app.Date:yyyy-MM-dd}";
                });
                ConsoleUtils.ViewList(table, "Appointments");
            }
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private static void ViewAppointmentsByDoctor()
        {
            Console.Write("Enter Doctor ID to view appointments: ");
            int doctorId = int.Parse(Console.ReadLine());
            var doctorAppointments = _appointments.Where(a => a.DoctorID == doctorId).ToList();

            if (!doctorAppointments.Any())
            {
                Console.WriteLine("No appointments found for this doctor.");
            }
            else
            {
                var doctor = BHCMDoctors.Doctors.FirstOrDefault(d => d.Id == doctorId);
                var table = doctorAppointments.Select(app =>
                {
                    var patient = BHCMPatients.Patients.FirstOrDefault(p => p.Id == app.PatientID);
                    return $"{patient?.Name.PadRight(20)} | {app.Date:yyyy-MM-dd}";
                });
                ConsoleUtils.ViewList(table, $"Appointments for Doctor: {doctor?.Name}");
            }
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private static void ViewAppointmentsByPatient()
        {
            Console.Write("Enter Patient ID to view appointments: ");
            int patientId = int.Parse(Console.ReadLine());
            var patientAppointments = _appointments.Where(a => a.PatientID == patientId).ToList();

            if (!patientAppointments.Any())
            {
                Console.WriteLine("No appointments found for this patient.");
            }
            else
            {
                var patient = BHCMPatients.Patients.FirstOrDefault(p => p.Id == patientId);
                var table = patientAppointments.Select(app =>
                {
                    var doctor = BHCMDoctors.Doctors.FirstOrDefault(d => d.Id == app.DoctorID);
                    return $"{doctor?.Name.PadRight(20)} | {app.Date:yyyy-MM-dd}";
                });
                ConsoleUtils.ViewList(table, $"Appointments for Patient: {patient?.Name}");
            }
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private static void ViewDoctorsList()
        {
            var table = BHCMDoctors.Doctors.Select(d => $"{d.Id.ToString().PadRight(5)} | {d.Name.PadRight(20)} | {d.Specialization.PadRight(20)}");
            ConsoleUtils.ViewList(table, "Doctors");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private static void ViewPatientsList()
        {
            var table = BHCMPatients.Patients.Select(p => $"{p.Id.ToString().PadRight(5)} | {p.Name.PadRight(20)}");
            ConsoleUtils.ViewList(table, "Patients");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private static void SearchDoctors()
        {
            Console.Write("Search Doctors (use * and ? as wildcards): ");
            string pattern = Console.ReadLine();

            string regexPattern = "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
            var results = BHCMDoctors.Doctors.Where(d => Regex.IsMatch(d.Name, regexPattern, RegexOptions.IgnoreCase))
                .Select(d => $"{d.Id.ToString().PadRight(5)} | {d.Name.PadRight(20)} | {d.Specialization.PadRight(20)}");

            if (!results.Any())
            {
                Console.WriteLine("No doctors found matching the search criteria.");
            }
            else
            {
                ConsoleUtils.ViewList(results, "Search Results - Doctors");
            }
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private static void SearchPatients()
        {
            Console.Write("Search Patients (use * and ? as wildcards): ");
            string pattern = Console.ReadLine();

            string regexPattern = "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
            var results = BHCMPatients.Patients.Where(p => Regex.IsMatch(p.Name, regexPattern, RegexOptions.IgnoreCase))
                .Select(p => $"{p.Id.ToString().PadRight(5)} | {p.Name.PadRight(20)}");

            if (!results.Any())
            {
                Console.WriteLine("No patients found matching the search criteria.");
            }
            else
            {
                ConsoleUtils.ViewList(results, "Search Results - Patients");
            }
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        // Save the current list of appointments to the JSON file
        private static void SaveData(List<Appointment> appointments)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                var jsonData = JsonSerializer.Serialize(appointments, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, jsonData);
                Console.WriteLine("Appointments data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        // Load the list of appointments from the JSON file
        private static List<Appointment> LoadData()
        {
            try
            {
                if (!File.Exists(filePath))
                    return new List<Appointment>();

                var jsonData = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<Appointment>>(jsonData) ?? new List<Appointment>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                return new List<Appointment>();
            }
        }
    }
}

