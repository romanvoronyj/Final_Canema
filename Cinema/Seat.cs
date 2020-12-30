using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    abstract class Seat
    {
        public int Number { get; private set; }

        public bool IsBusy { get; set; }

        public Seat(int number)
        {
            Number = number;
        }

    }
}
