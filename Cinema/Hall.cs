using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    class Hall: IShortInfo
    {
        public Hall(int number, decimal price)
        {
            Number = number;
            Price = price;
        }

        public int Number { get; private set; }

        public int SeatsNumber
        {
            get
            {

                if (Seats == null) return 0;

                return Seats.Count;
            }
        }

        public List<Seat> Seats{ get; set; }

        public decimal Price { get; private set; }

        public override string ToString()
        {
            return string.Format($"Кінозал 3D на {Seats} осіб");
        }

        public string ToShortString()
        {
            return string.Format($"Кінозал: 3D ");
        }

        public void AddSeat(Seat seat)
        {
            if (seat == null)
            {
                return;
            }

            if (Seats == null)
            {
                Seats = new List<Seat>();
            }

            if (Seats.Any(x => x.Number == seat.Number))
            {
                return;
            }

            Seats.Add(seat);
        }

        //public bool HasFreeSeats
        //{
        //    get
        //    {
        //        if(Seats == null)
        //        {
        //            return false;
        //        }

        //        return Seats.Any(x => !x.IsBusy);
        //    }
        //}

        //public List<Seat> FreeSeats
        //{
        //    get
        //    {
        //        if (Seats == null)
        //        {
        //            return new List<Seat>();
        //        }

        //        return Seats.Where(x => !x.IsBusy).ToList();
        //    }
        //}
    }
}
