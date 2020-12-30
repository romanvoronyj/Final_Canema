using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    class Ticket
    {
        private static long _number = 0;

        public readonly long Number;

        FilmScreening filmScreening;

        public Seat Seat { get; private set; }

        Viewer viewer;

        public decimal Price { get; private set; }

        public FilmScreening FilmScreening { get { return filmScreening; } }

        public Ticket(FilmScreening filmScreening, Seat seat, Viewer viewer, decimal price)
        {
            _number++;
            this.Number = _number;
            this.filmScreening = filmScreening;
            this.Seat = seat;
            this.viewer = viewer;
            this.Price = price;
        }
        public override string ToString()
        {
            return string.Format($"Квиток №:{Number}, Сеанс:{filmScreening}, Місце №:{Seat}, Глядач:{viewer}, Ціна:{Price}");
        }
    }

}
