using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;

namespace Cinema
{
    class Cinema
    {
        public string Name { get; private set; }

        public decimal Money { get; private set; }

        public List<Film> Films { get; private set; }

        public List<Hall> Halls { get; private set; }

        public List<Ticket> Tickets { get; private set; }

        public Cinema(string name)
        {
            this.Name = name;
        }

        public void AddHall(Hall hall)
        {
            if(hall == null)
            {
                return;
            }

            if(Halls == null)
            {
                Halls = new List<Hall>();
            }

            if(Halls.Any(x => x.Number == hall.Number))
            {
                return;
            }

            Halls.Add(hall);
        }

        public void AddFilm(Film film)
        {
            if (film == null)
            {
                return;
            }

            if (Films == null)
            {
                Films = new List<Film>();
            }

            if (Films.Any(x => x.FilmId == film.FilmId))
            {
                return;
            }

            Films.Add(film);
        }


        //список ПроданіКвитки(не клас)

        /*Метод Film[] GetFilms() повинен показуватиІнформацію про усі фільми, які ідуть у кінотеатрі зараз. Мається на увазі у яких endDate>= nowDate.*/
        public List<Film> GetFilms()
        {
            List<Film> availableFilms = new List<Film>();
            DateTime now = DateTime.Now;
            foreach (var film in Films)             //return Films.Where(f => f.EndDate > now).ToList();

            {
                if (film.EndDate >= now)
                {
                   availableFilms.Add(film);
                }
            }
            return availableFilms.ToList();
        }
        
        
        /*Метод Filmscreening[]GetFilmScreenings(int filmId)показувати Інформацію про конкретний фільм, а саме усі покази цього фільму. Метод повинен фільтрувати 
        * масив Films, а саме по полю filmId, яке є у Film(ПоказФільму, Фільм, Зал)і повертати FilmScreenings відфільтрований по даті яка актуальна.
        * Якщо такого фільму не буде знайдено необхідно викинути виняток NotExistException із відповідним повідомленням.*/
        public List<FilmScreening> GetFilmScreenings(int filmId)
        {
            DateTime now = DateTime.Now;
            var film = Films.SingleOrDefault(f => f.FilmId == filmId);
            if (film == null)
            {
                throw new Exception("Такий фільм недоступний.");
            }
            return film.FilmScreenings.Where(f => f.Date >= now).ToList();

        }



        /*Метод FilmscreeningGetFilmscreening(int filmId, int FilmscreeningId) показувати Інформацію про конкретний фільм, а самеконкретний показ цього фільму. Метод повинен фільтрувати масив Films, 
         * а саме по полю filmId(ПоказФільму, Фільм, Зал), після чого діставати FilmScreenings і фільтрувати їх по FilmscreeningId.
         * Показувати інформацію про вільні місця конкретного фільму (ПоказФільму, Фільм, Зал).Якщо такого фільму або показу фільму не буде знайдено необхідно викинути виняток NotExistException із відповідним повідомленням.
         * */
        public FilmScreening GetFilmscreening(int filmId, int filmscreeningId)
        {
            var film = Films.SingleOrDefault(f => f.FilmId == filmId);
            if (film == null)
            {
                throw new Exception("Вказаний фільм недоступний.");
            }

            FilmScreening Filmscreening;


            Filmscreening = film.FilmScreenings.SingleOrDefault(f => f.FilmScreeningId == filmscreeningId); //Показувати інформацію про вільні місця конкретного фільму (ПоказФільму, Фільм, Зал) ?


            if (Filmscreening == null)
            {
                throw new Exception("Обраний сеанс недоступний.");
            }

            return Filmscreening;
        }
        
        
        
        
        /*
         * Метод decimal GetFilmscreeningPrice(int FilmscreeningId, Int seatNumber) повертає інформацію про ціну вибраного фільму і місця. 
         * Ціна обчислюється додаванням цін за фільм, зал і місце якщо додаткова ціна існує.
         * */
        public decimal GetFilmscreeningPrice(int filmScreeningId, int seatNumber)
        {

            var film = Films.Where(x => x.FilmScreenings.SingleOrDefault(sc => sc.FilmScreeningId == filmScreeningId) != null).SingleOrDefault();

            if (film == null)
            {
                throw new Exception("Вказаний фільм недоступний.");
            }

            var screening = film.FilmScreenings.SingleOrDefault(x => x.FilmScreeningId == filmScreeningId);

            if(screening == null)
            {
                throw new Exception("Сеанс недоступний.");
            }

            var seat = screening.Hall.Seats.SingleOrDefault(x => x.Number == seatNumber);

            if (seat == null)
            {
                throw new Exception("Місце недоступне.");
            }
            decimal seatPrice = 0;
            if(seat is VipSeat)
            {
                seatPrice = ((VipSeat)seat).Price;
            }
            decimal totalPrice = film.Price + screening.Hall.Price + seatPrice;


            return totalPrice;
        }


        public Ticket BuyTicket(int filmScreeningId, int seatNumber, decimal price)
        {
            var viewer = GetViewer();

            if (viewer == null) return null;

            var screening = Films.SelectMany(f => f.FilmScreenings).SingleOrDefault(s => s.FilmScreeningId == filmScreeningId);

            if (viewer.Age < screening.film.MinAge)
            {
                throw new InvalidAgeException("Нажаль, Вам заборонено перегляд фільму!");                                                                                   /////// TODO
            }


            var seat = screening.SeatPlaces.SingleOrDefault(x => x.Number == seatNumber);
            if(seat.IsBusy)
            {
                throw new Exception("");                                                                                   /////// TODO
            }
            var filmPrice = GetFilmscreeningPrice(filmScreeningId, seatNumber);
            if(filmPrice != price)
            {
                throw new Exception("");                                                                                   /////// TODO
            }

            var ticket = new Ticket(screening, seat, viewer, price);

            RegisterTicket(ticket);

            seat.IsBusy = true;

            return ticket;
        }


        public bool ReturnTicket(long number)
        {
            if (Tickets == null || Tickets.Count == 0)
                return false;

            var ticket = Tickets.FirstOrDefault(x => x.Number == number);

            if (ticket == null)
                return false;


            var screening = Films.SelectMany(f => f.FilmScreenings).SingleOrDefault(s => s.FilmScreeningId == ticket.FilmScreening.FilmScreeningId);

            if (screening.Date < DateTime.Now || (screening.Date - DateTime.Now).TotalHours < 3)
            {
                Console.WriteLine("До сеансу залишилось менше 3 год.");
                return false;
            }

            UnRegisterTicket(ticket);

            return true;
        }

        private Viewer GetViewer()
        {
            Viewer viewer = null;

            do
            {
                sbyte age = 0;
                string _age = string.Empty;
                string firstName = string.Empty;
                string lastName = string.Empty;

                Console.WriteLine("Скільки Вам років ?");
                _age = Console.ReadLine();
                sbyte.TryParse(_age, out age);
                if (_age.IsCancel())
                {
                    break;
                }

                Console.WriteLine("Як Вас звати ?");
                firstName = Console.ReadLine();
                if (firstName.IsCancel())
                {
                    break;
                }


                Console.WriteLine("Прізвище ?");
                lastName = Console.ReadLine();
                if (lastName.IsCancel())
                {
                    break;
                }




                string patern = "[0-9]";
                var regex = new Regex(patern);
                if (age == 0 || regex.IsMatch(firstName) || regex.IsMatch(lastName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Некоректно введені персональні дані!Повторіть спробу!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(2500);
                    Console.Clear();
                }
                else
                {

                    viewer = new Viewer(age, firstName, lastName);
                }

            } while (viewer == null);

            return viewer;
        }

        private void RegisterTicket(Ticket ticket)
        {
            if(Tickets == null)
            {
                Tickets = new List<Ticket>();
            }

            this.Tickets.Add(ticket);

            PutMoney(ticket.Price);
        }

        private void PutMoney(decimal amount)
        {
            this.Money += amount;
        }

        private void UnRegisterTicket(Ticket ticket)
        {
            //ticket.Seat.IsBusy = false;

            var screening = Films.SelectMany(f => f.FilmScreenings).SingleOrDefault(s => s.FilmScreeningId == ticket.FilmScreening.FilmScreeningId);

            var seat = screening.SeatPlaces.SingleOrDefault(x => x.IsBusy && x.Number == ticket.Seat.Number);

            seat.IsBusy = false;

            Tickets.Remove(ticket);

            GiveMoney(ticket.Price);
        }

        private void GiveMoney(decimal amount)
        {
            this.Money -= amount;
        }
    }   
}


