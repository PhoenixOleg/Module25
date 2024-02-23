using System;
using System.Collections.Generic;
using System.Text;

namespace Module25.PLL.Helpers
{
    public class AlertMessage
    {
        /// <summary>
        /// Метод вывода сообщения об ошибке
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        public static void Show(string message)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message + "\n");
            Console.ForegroundColor = originalColor;
        }
    }
}
