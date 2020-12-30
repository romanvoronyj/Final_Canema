using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    class VipSeat: Seat
    {
        public decimal Price { get; private set; }
        public VipSeat(int number, decimal price)
            :base(number)
        {
            Price = price;
        }
    }
}
