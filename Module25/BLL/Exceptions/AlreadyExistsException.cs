using System.Runtime.Serialization;

namespace Module25.BLL.Exceptions
{
    /// <summary>
    /// Объект уже присутствует
    /// </summary>
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException()
        {
        }

        public AlreadyExistsException(string? message) : base(message)
        {
        }

        public AlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}