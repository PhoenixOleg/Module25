using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.BLL.Exceptions
{
    /// <summary>
    /// Объект не найден
    /// </summary>
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException() 
        { 
        }

        public ObjectNotFoundException(string message) : base(message)
        {
        }

        public ObjectNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
