using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.PLL.Helpers
{
    public static class SuccessMessage
    {
        /// <summary>
        /// Метод вывода сообщения о выполнении действия
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        public static void Show(string message)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message + "\n");
            Console.ForegroundColor = originalColor;
        }
    }
}
