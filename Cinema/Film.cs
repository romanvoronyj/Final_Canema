using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    class Film
    {
        #region Props

        public int FilmId { get; private set; }
        public string Name { get; private set; }
        public int MinAge { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public List<FilmScreening> FilmScreenings { get; private set; }

        #endregion

        public Film (int filmId, string name, int minAge, string description, decimal price, DateTime startDate, DateTime endDate)
        {
            this.FilmId = filmId;
            this.Name = name;
            this.MinAge = minAge;
            this.Description = description;
            this.Price = price;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.FilmScreenings = new List<FilmScreening>();
        }

        public void AddScreening(FilmScreening filmScreening)
        {
            if(filmScreening == null || FilmScreenings.Any(x => x.FilmScreeningId == filmScreening.FilmScreeningId))
            {
                return;
            }
            FilmScreenings.Add(filmScreening);
        }
        public override string ToString()
        {
            return $"ID фільму: {FilmId}\nНазва фільму: {Name}\nМінімальний вік: {MinAge}\nОпис: {Description}\nЦіна квитка: {Price}\nУ прокаті з: {StartDate}\tдо: {EndDate}.\n";
        }
        public string ToShortString()
        {
            return $"ID фільму: {FilmId}\tФільм: {Name}\tЦіна квитка: {Price}.";
        }
    }
}
