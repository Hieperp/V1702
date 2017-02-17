using System;

namespace TotalModel.Validations
{
    public class DatabaseConcurrencyException : Exception
    {
        public DatabaseConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
