using System.Runtime.Serialization;

namespace Module25.BLL.Exceptions
{
    public class NameEmptyException : Exception
    {
        public NameEmptyException()
        {
        }

        public NameEmptyException(string? message) : base(message)
        {
        }

        public NameEmptyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}