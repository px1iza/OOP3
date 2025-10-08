using System.Text.RegularExpressions;

namespace Entities
{
    public class Student : Human, ISkill
    {
        public int Height { get; set; }
        public int Weight { get; set; }
        public string StudentID { get; set; }
        public Passport Passport { get; set; }

        private int _rideCount = 0;
        public int RideCount => _rideCount;

        public Student(string firstName, string lastName, int height, int weight, string studentID, Passport passport)
            : base(firstName, lastName)
        {
            Height = height;
            Weight = weight;
            StudentID = studentID;
            Passport = passport;
        }

        public bool HasIdealWeight()
        {
            return Weight == Height - 110;
        }

        public void RideBike()
        {
            _rideCount++;
        }

        public bool IsValidStudentID()
        {
            return Regex.IsMatch(StudentID, @"^[A-Z]{2}\d{6}$");
        }
    }
}
