using System;
namespace Entities
{
    public class Librarian : Human, ISkill
    {
        private int _rideCount = 0;
        public int RideCount => _rideCount;

        public Librarian(string firstName, string lastName)
            : base(firstName, lastName) { }

        public void RideBike()
        {
            _rideCount++;
        }
    }
}