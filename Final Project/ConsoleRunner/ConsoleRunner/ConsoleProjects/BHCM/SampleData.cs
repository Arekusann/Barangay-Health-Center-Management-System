namespace ConsoleRunner.ConsoleProjects.BHCM
{
    internal static class SampleData
    {
        internal static List<Doctor> LoadSampleDoctors()
        {
            return [
                new Doctor(1, "Olivia Silence", "Optometrist"),
                new Doctor(2, "Adele Naumann", "Otolarynologist"),
                new Doctor(3, "Dorothy Franks", "General Physician"),
                new Doctor(4, "Joyce Moore", "Medical Technician"),
                new Doctor(5, "Elliot Glover", "Podiatrist"),
                new Doctor(6, "Mayer Stony", "Pediatrician"),
                new Doctor(7, "Kirsten Wright", "Cardiologist"),
            ];
        }
    }
}
