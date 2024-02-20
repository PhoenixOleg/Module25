using System.Runtime.Serialization;

namespace Module25.BLL.Exceptions
{
    public class InvalidDateIntervalException : Exception
    {
        public InvalidDateIntervalException()
        {
        }

        public InvalidDateIntervalException(string? message) : base(message)
        {
        }

        public InvalidDateIntervalException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}