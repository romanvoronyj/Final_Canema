using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    class FilmScreening : IShortInfo
    {
        int filmScreeningId;
        public Film film;                    //TODO property;
        Hall hall;
        DateTime dateTime;
        public int FilmScreeningId { get { return filmScreeningId; } }

        public List<Seat> SeatPlaces { get; private set; }
        
        public DateTime Date { get { return dateTime; } }

        public Hall Hall
        {
            get
            {
                return this.hall;
            }
        }
        public FilmScreening(int filmScreeningId, Film film, Hall hall,  DateTime dateTime)
        {
            this.filmScreeningId = filmScreeningId;
            this.film = film;
            this.hall = hall;
            this.dateTime = dateTime;

            SeatPlaces = hall.Seats.Select(x => 
            {
                if(x.GetType() == typeof(NormalSeat))
                {
                    return (Seat)new NormalSeat(x.Number);
                }
                else
                {
                    return (Seat)new VipSeat(x.Number, ((VipSeat)x).Price);
                }
            }).ToList();

        }

        public override string ToString()
        {
            return ($"Сеанс: {filmScreeningId}, Назва фільму: {film.Name}, Зал: {hall.Number}, Дата та час: {dateTime}"); //Вільно місць: {hall.FreeSeats.Count}
        }
        public string ToShortString()
        {
            return ($"Назва фільму: {film}, Сеанс: {filmScreeningId}");
        }
    }
}
