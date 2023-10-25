using System.Runtime.Caching;

namespace Students_API.Services
{
    public class CacheService : ICacheService
    {
        private ObjectCache _memoryCache = MemoryCache.Default;
        public T GetData<T>(string key)
        {
            try
            {
                T item = (T) _memoryCache.Get(key);
                return item;
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public object RemoveData(string key)
        {
            try
            {
                
                var item = _memoryCache.Get(key);
                
                _memoryCache.Remove(key);
                return item;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var res = true;

            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    res = false;
                }
                else
                {

                    _memoryCache.Set(key, value, expirationTime);
                }
                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
