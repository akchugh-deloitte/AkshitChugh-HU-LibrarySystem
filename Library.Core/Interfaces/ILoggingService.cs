using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Core.Interfaces
{
    public interface ILoggingService
    {
        Task LogInfoAsync(string message);
        Task LogErrorAsync(string message, Exception? ex = null);
    }
}
