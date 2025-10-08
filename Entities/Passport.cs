using System.Text.RegularExpressions;

namespace Entities
{
    public class Passport
    {
        public string Series { get; set; }
        public int Number { get; set; }
        public string FullPassport => $"{Series}{Number:D6}";

        public Passport(string series, int number)
        {
            Series = series;
            Number = number;
        }

        public bool IsValidPassport()
        {
            return Regex.IsMatch(FullPassport, @"^[A-Z]{2}\d{6}$");
        }
    }
}