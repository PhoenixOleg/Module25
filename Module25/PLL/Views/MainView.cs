using Module25.Task_25_2_4.PLL.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.PLL.Views
{
    public class MainView
    {
        CRUDView_Main cCRUDView_Main = new();

        public void Show()
        {
            string answer;
            do
            {
                Console.WriteLine("Выполнить задание 25.2.4 - первая БД (нажмите 1)");
                Console.WriteLine("Выполнить задание 25.3.5 - просто CRUD (нажмите 2)");
                Console.WriteLine("Выполнить задание 25.4.3 - расширение БД (нажмите 3)");
                Console.WriteLine("Выполнить задание 25.4.3 - работа с жанрами, авторами и получение книг (нажмите 4)");
                Console.WriteLine("Следующее задание (нажмите 5)");
                Console.WriteLine("Завершить работу (нажмите 0)");

                answer = Console.ReadLine();
                switch (answer)
                {
                    case "1":
                        {
                            Console.Clear();
                            CreationDBView.Show();
                            break;
                        }

                    case "2":
                        {
                            Console.Clear();
                            cCRUDView_Main.Show();
                            break;
                        }

                    case "3":
                        {
                            Console.Clear();
                            CreationExtenedDBView.Show();
                            break;
                        }

                    case "4":
                        {
                            Console.Clear();

                            break;
                        }
                }
                Console.Clear();
            }
            while (answer != "0");
        }
    }
}
