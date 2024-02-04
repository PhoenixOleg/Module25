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
        public void Show()
        {
            string answer;
            do
            {
                Console.WriteLine("Выполнить задание 25.2.4 (нажмите 1)");
                Console.WriteLine("Следующее задание (нажмите 2)");
                Console.WriteLine("Следующее задание (нажмите 3)");
                Console.WriteLine("Следующее задание (нажмите 4)");
                Console.WriteLine("Завершить работу (нажмите 0)");

                answer = Console.ReadLine();
                switch (answer)
                {
                    case "1":
                        {
                            CreationDBView.Show();
                            break;
                        }

                    case "2":
                        {
                            //.Show();
                            break;
                        }

                    case "3":
                        {
                            //.Show();
                            break;
                        }

                    case "4":
                        {
                            //.Show();
                            break;
                        }
                }
                Console.Clear();
            }
            while (answer != "0");
        }
    }
}
