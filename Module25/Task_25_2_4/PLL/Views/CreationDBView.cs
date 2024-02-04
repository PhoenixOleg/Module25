using Module25.PLL.Helpers;
using Module25.Task_25_2_4.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.Task_25_2_4.PLL.Views
{
    public static class CreationDBView
    {
        public static void Show()
        {            
            string answer;

            Action();

            do
            {
                Console.WriteLine("Повторить задание 25.2.4 (нажмите 1)");
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
            CommonService CommonService = new();

            Console.WriteLine("\nЗадание 25.2.4\nСоздаем БД с двумя таблицами...");
            try
            {
                CommonService.CreateFirstDB();
            }
            catch (Exception ex)
            {
                AlertMessage.Show("Возникла ошибка:\n" + ex.Message);
            }
        }
    }
}
