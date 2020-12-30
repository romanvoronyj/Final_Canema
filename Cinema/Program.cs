using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    class Program
    {
        const string YES = "y";
        static Cinema cinema;
        static void Initialize()
        {
            cinema = new Cinema("Кінопалац на Театральній");

            var hall = new Hall(1, 100);
            for (int i = 0; i < 500; i++)
            {
                if(i < 450)
                {
                    hall.AddSeat(new NormalSeat(i + 1));
                }
                else
                {
                    hall.AddSeat(new VipSeat(i + 1, 100));
                }

            }

            var hall2 = new Hall(2, 100);
            for (int i = 0; i < 220; i++)
            {
                if (i < 200)
                {
                    hall2.AddSeat(new NormalSeat(i + 1));
                }
                else
                {
                    hall2.AddSeat(new VipSeat(i + 1, 100));
                }

            }

            var hall3 = new Hall(3, 120);
            for (int i = 0; i < 150; i++)
            {
                if (i < 130)
                {
                    hall3.AddSeat(new NormalSeat(i + 1));
                }
                else
                {
                    hall3.AddSeat(new VipSeat(i + 1, 100));
                }

            }

            var hall4 = new Hall(4, 80);
            for (int i = 0; i < 100; i++)
            {
                if (i < 85)
                {
                    hall4.AddSeat(new NormalSeat(i + 1));
                }
                else
                {
                    hall4.AddSeat(new VipSeat(i + 1, 100));
                }

            }


            cinema.AddHall(hall);
            cinema.AddHall(hall2);
            cinema.AddHall(hall3);
            cinema.AddHall(hall4);

            var film1 = (new Film(1, "Хоробре серце", 12, "Бойовик, історичний", 150.0M, DateTime.Now.Date.AddDays(-2).AddHours(-6), DateTime.Now.Date.AddDays(11).AddHours(22)));
            FilmScreening filmScreening11 = new FilmScreening(1,film1,hall, DateTime.Now.Date.AddDays(1).AddHours(14));
            FilmScreening filmScreening12 = new FilmScreening(2,film1,hall2, DateTime.Now.Date.AddDays(1).AddHours(20).AddMinutes(30));
            FilmScreening filmScreening13 = new FilmScreening(3,film1,hall4, DateTime.Now.Date.AddDays(2).AddHours(18).AddMinutes(45));
            film1.AddScreening(filmScreening11);
            film1.AddScreening(filmScreening12);
            film1.AddScreening(filmScreening13);
            cinema.AddFilm(film1);

            var film2 = new Film(2, "Врятувати рядового Раяна", 12, "Воєнний, історичний", 120.0M, DateTime.Now.Date.AddDays(-5).AddHours(-9), DateTime.Now.Date.AddDays(14).AddHours(18));
            FilmScreening filmScreening21 = new FilmScreening(4, film2, hall, DateTime.Now.Date.AddDays(1).AddHours(16));
            FilmScreening filmScreening22 = new FilmScreening(5, film2, hall4, DateTime.Now.Date.AddDays(2).AddHours(16).AddMinutes(15));
            FilmScreening filmScreening23 = new FilmScreening(6, film2, hall3, DateTime.Now.Date.AddDays(4).AddHours(14).AddMinutes(45));
            film2.AddScreening(filmScreening21);
            film2.AddScreening(filmScreening22);
            film2.AddScreening(filmScreening23);
            cinema.AddFilm(film2);

            var film3 = new Film(3, "Форсаж", 12, "Перегони, пригодницький", 200.0M, DateTime.Now.Date.AddDays(-2).AddHours(-7), DateTime.Now.Date.AddDays(8).AddHours(22));
            FilmScreening filmScreening31 = new FilmScreening(7, film3, hall4, DateTime.Now.Date.AddHours(16).AddMinutes(10));
            FilmScreening filmScreening32 = new FilmScreening(8, film3, hall2, DateTime.Now.Date.AddDays(1).AddHours(14).AddMinutes(45));
            FilmScreening filmScreening33 = new FilmScreening(9, film3, hall, DateTime.Now.Date.AddDays(4).AddHours(14).AddMinutes(45));
            film3.AddScreening(filmScreening31);
            film3.AddScreening(filmScreening32);
            film3.AddScreening(filmScreening33);
            cinema.AddFilm(film3);
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Initialize();

    
            while (true) 
            {
                RunCinema();
                Task.Delay(3000).Wait();
                Console.Clear();
            }

            Console.ReadKey();

        }
        static void RunCinema()         // весь процес придбання квитка
        {
            Console.WriteLine("Оберіть дію: 1 - покупка; 2 - повернення; \tАбо введіть пароль адміна:\nДля скасування операцій в подальшому, введіть: 'cancel'");
            string response = Console.ReadLine();
            int.TryParse(response, out int customerProcess);
            if (customerProcess == 1)
            {
                try
                {
                    BuyTicketProcess();
                }
                catch(InvalidAgeException e)
                {
                    Console.WriteLine(e.Message+"Будь ласка, спробуйте ще раз.");
                }
            }
            else if(customerProcess == 2)
            {
                ReturnTicketProcess();
            }

            if (response == "0000")
            {
                ShowAdminMenu();
            }
        }

        static void ShowAdminMenu()
        {
            Console.WriteLine("Показати залишок коштів у касі, натисніть: 1;");
            int.TryParse(Console.ReadLine(), out int customerProcess);
            if (customerProcess == 1)
            {
                Console.WriteLine($"{cinema.Money.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)}"); //https://docs.microsoft.com/ru-ru/dotnet/api/system.globalization.cultureinfo.invariantculture?redirectedfrom=MSDN&view=net-5.0#System_Globalization_CultureInfo_InvariantCulture
            }
        }

        static void BuyTicketProcess()
        {
            var film = SelectFilm();
            if (film == null)
            {
                return;
            }

            var screening = SelectScreen(film);
            if (screening == null)
            {
                return;
            }
            
            int seatNumber = -1;

            Seat selectedSeat = null; screening.SeatPlaces.Where(x => x.Number == seatNumber).FirstOrDefault();
            do
            {
                seatNumber = GetSeatNumber(screening);

                selectedSeat = screening.SeatPlaces.Where(x => x.Number == seatNumber).FirstOrDefault();

                if(selectedSeat == null || selectedSeat.IsBusy)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;

                    Console.WriteLine($"Виберіть вільне місце!");

                    Console.ForegroundColor = ConsoleColor.White;

                    Task.Delay(5000).Wait();

                }

            }
            while (selectedSeat == null || selectedSeat.IsBusy);

            //if (seatNumber <= 0)
            //{
            //    return;
            //}


            var price = cinema.GetFilmscreeningPrice(screening.FilmScreeningId, seatNumber);

            Console.WriteLine($"Вартість обраного місця {price.ToString("#0.00", CultureInfo.InvariantCulture)}");
            Console.WriteLine($"Підтвердити вибір ? (y/n)");

            var confirm = Console.ReadLine();

            if (!string.IsNullOrEmpty(confirm) && confirm.ToLower() == YES)
            {
                var ticket = cinema.BuyTicket(screening.FilmScreeningId, seatNumber, price);

                if (ticket == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неможливо придбати квиток!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();

                    return;
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Дякуємо за покупку.Гарного перегляду!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(ticket);
            }
            else
            {
                //BuyTicketProcess();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Операцію скасовано.");
                Console.ForegroundColor = ConsoleColor.White;

            }
        }

        static void ReturnTicketProcess()
        {
            Console.WriteLine("Введіть номер білета: ");

            string response = Console.ReadLine();

            if(long.TryParse(response, out long number))
            {
                if (cinema.ReturnTicket(number))
                {
                    Console.WriteLine("Квиток повернуто!");
                }
                else
                {
                    Console.WriteLine("Не вдалось повернути квиток!");
                }

            }
            else
            {
                Console.WriteLine("Квиток не знайдено!");
            }
        }

        static Film SelectFilm()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("Доступні фільми:");

            Console.ForegroundColor = ConsoleColor.White;
            


            Film selectedFilm = null;

            var allAvailableFilms = cinema.GetFilms();
            allAvailableFilms.ForEach(f => 
            {
                Console.WriteLine(f.ToShortString());
            });

            int selectedFilmId = 0;
            do
            {
                Console.WriteLine("Введіть ID обраного фільму:");

                string answer = Console.ReadLine();

                if(answer.IsCancel())
                {
                    break;
                }

                int.TryParse(answer, out selectedFilmId); //==== var x = int.Parse(Console.ReadLine());

                selectedFilm = allAvailableFilms.FirstOrDefault(f => f.FilmId == selectedFilmId);

                if (selectedFilm == null)
                {
                    Console.WriteLine("Введіть коректні дані!");
                }
                else
                {
                    Console.WriteLine($"\nВи обрали: {selectedFilm}");
                }
            }
            while (selectedFilmId == 0);

            return selectedFilm;

        }

        static FilmScreening SelectScreen(Film selectedFilm)
        {
            var screenings = cinema.GetFilmScreenings(selectedFilm.FilmId);
            if (screenings == null || screenings.Count == 0)
            {
                Console.WriteLine("Не знайдено доступних сеансів.");
                return null;
            }
            Console.WriteLine("Доступні сеанси:");

            foreach (var screening in screenings)
            {
                Console.WriteLine(screening);
            }
            Console.WriteLine("Введіть обраний сеанс:");
            int.TryParse(Console.ReadLine(), out int filmScreeningId);

            return cinema.GetFilmscreening(selectedFilm.FilmId, filmScreeningId);
            //return screenings.FirstOrDefault(x => x.FilmScreeningId == filmScreeningId);
        }
        
        static int GetSeatNumber(FilmScreening screening) 
        {
            Console.WriteLine($"Оберіть доступне місце:");

            int pagesize = 10;

            var normalFreeSeats = screening.SeatPlaces.Where(s => s.GetType() == typeof(NormalSeat)).ToList();

            var vipFreeSeats = screening.SeatPlaces.Where(s => s.GetType() == typeof(VipSeat)).ToList();


            Console.WriteLine($"Звичайні місця:");


            int row = 1;
            var temp = normalFreeSeats.Take(pagesize).ToList();

            while (temp.Count > 0)
            {
                var seats = temp.Select(s => 
                {
                    //if (s.IsBusy) return "xxx";
                    return new { s.IsBusy, Number = s.Number.ToString() };
                });

                Console.Write($"Ряд {row} => ");

                foreach (var seat in seats)
                {
                    Console.ForegroundColor = seat.IsBusy ? ConsoleColor.Red : ConsoleColor.White;

                    Console.Write($"{seat.Number};");

                    Console.ForegroundColor = ConsoleColor.White;
                }

                temp = normalFreeSeats.Skip(row * pagesize).Take(pagesize).ToList();
                row++;
                Console.Write($"\r\n");
            }

            Console.WriteLine($"VIP місця:");

            row = 1;
            temp = vipFreeSeats.Take(pagesize).ToList();
            while (temp.Count > 0)
            {
                var seats = temp.Select(s =>
                {
                    //if (s.IsBusy) return "xxx";
                    return new { s.IsBusy, Number = s.Number.ToString() };
                });

                Console.Write($"Ряд {row} => ");

                foreach (var seat in seats)
                {
                    Console.ForegroundColor = seat.IsBusy ? ConsoleColor.Red : ConsoleColor.White;

                    Console.Write($"{seat.Number};");

                    Console.ForegroundColor = ConsoleColor.White;
                }

                temp = vipFreeSeats.Skip(row * pagesize).Take(pagesize).ToList();
                row++;
                Console.Write($"\r\n");
            }


            int.TryParse(Console.ReadLine(), out int seatNumber);

            return seatNumber;
        }

    }

    //public static class CConsole
    //{
    //    public static void WriteLine(string line)
    //    {
    //        Console.WriteLine(line + " (або \"cancel\" для відміни)");
    //    }
    //    public static void WriteLine(object obj)
    //    {
    //        Console.WriteLine(obj.ToString());
    //    }
    //}
}

