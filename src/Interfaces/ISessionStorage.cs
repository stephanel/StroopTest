using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace StroopTest.Interfaces
{
    public interface ISessionStorage
    {
        void Clear();
        void SetObjectAsJson(string key, object value);
        void SetString(string key, string value);
        T GetObjectFromJson<T>(string key) where T : new();
        string GetString(string key);
    }
}