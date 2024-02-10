using Module25.BLL.Services;
using Module25.PLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.PLL.Views
{
    public static class CreationExtenedDBView
    {
        public static void Show()
        {
            string answer;

            Action();

            do
            {
                Console.WriteLine("Повторить задание 25.4.3 (нажмите 1)");
                Console.WriteLine("Вернуться (нажмите 0)");

                answer = Console.ReadLine();
                switch (answer)
                {
                    case "1":
                        {
                            Action();
                            break;
                        }
                }
            }
            while (answer != "0");
        }

        private static void Action()
        {
            CreateExtendedDBService createExtendedDBService = new();

            Console.WriteLine("\nЗадание 25.4.3\nСоздаем БД с четырьмя связанными таблицами...");
            try
            {
                createExtendedDBService.CreateExtendedDB();
            }
            catch (Exception ex)
            {
                AlertMessage.Show("Возникла ошибка:\n" + ex.Message);
            }
        }
    }
}
