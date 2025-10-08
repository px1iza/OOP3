namespace Entities
{
    public abstract class Human
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        protected Human(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}