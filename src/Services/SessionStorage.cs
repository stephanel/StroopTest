using Microsoft.AspNetCore.Http;
using StroopTest.Extensions;
using StroopTest.Interfaces;
using System.Collections.Generic;

namespace StroopTest.Services
{
    public class SessionStorage : ISessionStorage
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public void Clear()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }

        public SessionStorage(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetObjectAsJson(string key, object value)
        {
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson(key, value);
        }

        public T GetObjectFromJson<T>(string key)
            where T : new()
        {
            T value = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<T>(key);

            if(value == null)
                value = new T();

            return value;
        }
    }
}