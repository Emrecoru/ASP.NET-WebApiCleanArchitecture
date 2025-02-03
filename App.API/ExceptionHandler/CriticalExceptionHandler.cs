using App.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace App.API.ExceptionHandlers
{
    public class CriticalExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if(exception is CriticalException)
            {
                Console.WriteLine("Hata ile ilgili sms gönderildi");
            }
            
            return ValueTask.FromResult(false);
        }
    }
}
