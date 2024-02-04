using Module25.Task_25_2_4.DAL;
using Module25.Task_25_2_4.DAL.Entities;

namespace Module25
{
    internal class Program
    {
        static void Main()
        {

            Console.WriteLine("Задание 25.2.4");
            using (var db = new MyDBContext())
            {
                var user1 = new User { Name = "User1", Email = "user1@gmail.com" };
                var user2 = new User { Name = "User2", Email = "user2@gmail.com" };
                var user3 = new User { Name = "User3", Email = "user3@gmail.com" };

                var book1 = new Book { Title = "Book1", Description = "Intresting book", PublicationDate = DateOnly.Parse("01.03.2015") };
                var book2 = new Book { Title = "Book2", PublicationDate = DateOnly.Parse("15.07.2017") };
                var book3 = new Book { Title = "Book3", PublicationDate = DateOnly.Parse("30.08.2020") };
                var book4 = new Book { Title = "Book4", Description = "Good book", PublicationDate = DateOnly.Parse("20.06.2021") };

                db.AddRange(user1, user2, user3);
                db.AddRange(book1, book2, book3, book4);

                db.SaveChanges();
            }

            Console.WriteLine("Нажмите любую клавишу для выхода");
            Console.ReadKey();
        }
    }
}
