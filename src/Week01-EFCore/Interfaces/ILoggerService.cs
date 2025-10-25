using System;
using System.Threading.Tasks;

namespace Week01_EFCore.Interfaces
{
    public interface ILoggerService
    {
        Task LogInformationAsync(string message);
        Task LogWarningAsync(string message);
        Task LogErrorAsync(string message, Exception? exception = null);
    }
}