using ConsoleRunner.ConsoleProjects.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ConsoleRunner.ConsoleProjects.BHCM
{
    public static partial class BarangayHealthCenterManagement
    {
        const string patientsFile = "patients.txt";
        const string medicinesFile = "medicines.txt";
        const string appointmentsFile = "appointments.txt";
        const string doctorsFile = "doctors.txt";  

        public static void Run()
        {
            LoadData();
            while (true)
            {
                Console.Clear();
                var input = ConsoleUtils.ShowOptions("Barangay Health Center Management System", "Select an option", new List<string>
                {
                    "1. Management Patient Records",
                    "2. Track Medicine Inventory",
                    "3. Manage Appointments",
                    "4. Manage Doctors",
                    "5. Exit"
                });
                switch (input)
                {
                    case "1": BHCMPatients.ManagePatients(); break;
                    case "2": BHCMMedicines.ManageMedicines(); break;
                    case "3": BHCMAppointments.ManageAppointments(); break;
                    case "4": BHCMDoctors.ManageDoctors(); break;
                    case "5": SaveData(); Environment.Exit(0); break;
                    default: Console.WriteLine("Invalid selection."); break;
                }
            }
        }

        // Save and Load Data
        public static void SaveData()
        {
            // Save patient, doctor, appointment, and medicine data here using file handling.
        }

        
        static void LoadData()
        {
            // Load patient, doctor, appointment, and medicine data here using file handling.
            LoadSampleData();

            // For Debugging Purposes
            static void LoadSampleData()
            {
                BHCMDoctors.LoadSampleData();
            }
        }
    }
}
