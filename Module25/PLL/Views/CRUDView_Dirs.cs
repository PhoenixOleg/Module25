using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.PLL.Views
{
    public class CRUDView_Dirs
    {
        public void Show()
        {
            string answer;
            do
            {
                Console.WriteLine("1. Работа с авторами (нажмите 1)");
                Console.WriteLine("2. Работа с жанрами (нажмите 2)");
                Console.WriteLine("3. Выдача и возврт книг (нажмите 3)");
                Console.WriteLine("\nЗавершить работу (нажмите 0)");

                answer = Console.ReadLine();
                switch (answer)
                {
                    case "1":
                        {
                            Console.Clear();
                            CRUDView_Author cRUDView_Author = new();
                            cRUDView_Author.Show();
                            break;
                        }

                    case "2":
                        {
                            Console.Clear();
                            CRUDView_Genre cRUDView_Genre = new();
                            cRUDView_Genre.Show();
                            break;
                        }
                        case "3":
                        {
                            Console.Clear();
                            CRUDView_UserService cRUDView_UserService = new();
                            cRUDView_UserService.Show();
                            break;
                        }
                }
                Console.Clear();
            }
            while (answer != "0");
        }
    }
}
