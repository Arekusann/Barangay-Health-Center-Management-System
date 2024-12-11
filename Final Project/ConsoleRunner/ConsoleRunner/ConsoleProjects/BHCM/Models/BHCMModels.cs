 using System.Numerics;
using System.Xml.Linq;

namespace ConsoleRunner.ConsoleProjects.BHCM
{
    public abstract class BaseModel(int id, string name)
    {
        protected int _id { get; set; } = id;
        public string Name { get; set; } = name;

        public int Id { get => _id; }
    }

    internal class Patient(int id, string name, int age, string gender, string contact, string medicalHistory) : BaseModel(id, name)
    {
        public string Code { get; set; } = "PTN-" + id.ToString().PadLeft(3, '0');
        public int Age { get; set; } = age;
        public string Gender { get; set; } = gender;
        public string Contact { get; set; } = contact;
        public string MedicalHistory { get; set; } = medicalHistory;
        public bool IsDeleted { get; set; } = false;

        public int Id { get => _id;  }
        public string Data { get => $"ID: {_id}, Code: {Code}, Name: {Name}, Age: {Age}, Gender: {Gender}, Contact: {Contact}, Medical History: {MedicalHistory}"; }
        public void PrintData()
            => Console.WriteLine(Data);
    }

    internal class Medicine(string name, int stock, DateTime expirationDate)
    {
        public string Name { get; set; } = name;
        public int Stock { get; set; } = stock;
        public DateTime ExpirationDate { get; set; } = expirationDate;

        public string Data { get => $"Name: {Name}, Stock: {Stock}, Expiration Date: {ExpirationDate:yyyy-MM-dd}"; }
        public void PrintData()
            => Console.WriteLine(Data);
    }

    internal class Appointment(int patientId, int doctorId, DateTime date)
    {
        public int PatientID { get; set; } = patientId;
        public int DoctorID { get; set; } = doctorId;
        public DateTime Date { get; set; } = date;
    }

    class Doctor(int id, string name, string specialization) : BaseModel(id, name)
    {
        public string Code { get; set; } = "DOC-" + id.ToString().PadLeft(3, '0');
        public string Specialization { get; set; } = specialization;
        public bool IsDeleted { get; set; } = false;


        public string Data { get => $"ID: {_id}, Code: {Code}, Name: {Name}, Specialization: {Specialization}"; }
        public void PrintData()
            => Console.WriteLine(Data);
    }

    // Extension Class
    public static class BaseModelExtension
    {
        public static List<BaseModel> Search(this List<BaseModel> source, string textInput)
            => source.Where(w => w.Id.ToString() == textInput || w.Name.Equals(textInput, StringComparison.CurrentCultureIgnoreCase)).ToList();
    }
}
