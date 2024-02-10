using System.Runtime.Serialization;

namespace Module25.BLL.Exceptions
{
    public class EMailEmptyException : Exception
    {
        public EMailEmptyException()
        {
        }

        public EMailEmptyException(string? message) : base(message)
        {
        }

        public EMailEmptyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}