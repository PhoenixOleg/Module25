using System.Runtime.Serialization;

namespace Module25.BLL.Exceptions
{
    public class DateOutOfRangeException : Exception
    {
        public DateOutOfRangeException()
        {
        }

        public DateOutOfRangeException(string? message) : base(message)
        {
        }

        public DateOutOfRangeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}