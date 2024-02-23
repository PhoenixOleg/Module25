using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.BLL.Exceptions
{
    /// <summary>
    /// Не найдено ни одного объекта (список пуст)
    /// </summary>
    public class NoOneObjectException : Exception
    {
        public NoOneObjectException()
        {
        }

        public NoOneObjectException(string? message) : base(message)
        {
        }

        public NoOneObjectException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
