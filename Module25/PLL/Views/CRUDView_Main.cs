using Module25.Task_25_2_4.PLL.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.PLL.Views
{
    public class CRUDView_Main
    {
        CRUDView_User userView = new();
        CRUDView_Book bookView = new();
        public void Show()
        {          
            string answer;

            do
            {
                Console.WriteLine("\nПолучить информацию о пользователях (нажмите 1)");
                Console.WriteLine("Получить информацию о книгах (нажмите 2)");
                Console.WriteLine("Вернуться назад (нажмите 0)");

                answer = Console.ReadLine();
                switch (answer)
                {
                    case "1":
                        {
                            Console.Clear();
                            userView.Show();
                            break;
                        }

                    case "2":
                        {
                            Console.Clear();
                            bookView.Show();
                            break;
                        }
                }
                Console.Clear();
            }
            while (answer != "0");
        }
    }
}

