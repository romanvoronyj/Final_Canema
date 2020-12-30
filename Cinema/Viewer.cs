using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    class Viewer
    {
        public sbyte Age { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public Viewer(sbyte age, string firstName, string lastName)
        {
            this.Age = age;
            this.FirstName = firstName;
            this.LastName = lastName;
        }
        public override string ToString()
        {
            return string.Format($"Прізвище та ім'я глядача: {LastName} {FirstName}, Вік: {Age}");
        }
    }
}
