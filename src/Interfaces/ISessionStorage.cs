using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace StroopTest.Interfaces
{
    public interface ISessionStorage
    {
        void Clear();
        void SetObjectAsJson(string key, object value);
        T GetObjectFromJson<T>(string key) where T : new();
    }
}