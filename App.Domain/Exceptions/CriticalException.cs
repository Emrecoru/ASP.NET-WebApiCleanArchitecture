namespace App.Domain.Exceptions
{
    public class CriticalException : Exception
    {
        public CriticalException(string message) : base(message)
        {

        }
    }
}
