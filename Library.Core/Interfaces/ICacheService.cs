using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Core.Interfaces
{
    public interface ICacheService
    {
        T? Get<T>(string key);  
        void Set<T>(string key, T value, TimeSpan exp);
        void Remove(string key);
        bool Exists(string key);
    }
}
