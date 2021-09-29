using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365.Shared
{
    public class MemoryCache
    {
        private class CachedObject
        {
            public object Data { get; set; }
            public DateTime ExpiryDate { get; set; }
        }

        private static ConcurrentDictionary<string, CachedObject> cacheList = new ConcurrentDictionary<string, CachedObject>();

        public static T GetItem<T>(string key, Func<T> set, int cachePeriodInMinutes = 15)
        {
            //return from cache if exists and not yet expired.
            cacheList.TryGetValue(key, out CachedObject obj);
            if (obj != null && obj.ExpiryDate > DateTime.Now)
                return (T)obj.Data;
            
            //invoke caller for the new data.
            object result = set.Invoke();

            //put in cache.
            CachedObject newObj = new CachedObject()
            {
                Data = result,
                ExpiryDate = DateTime.Now.AddMinutes(cachePeriodInMinutes)
            };
            cacheList.AddOrUpdate(key, newObj, 
                (string thekey, CachedObject existingObject) => {
                    return newObj;
                });
            return (T)result;
        }        
        
        public static void SetExpiryDate(string key, DateTime expiryDate)
        {
            //return from cache if exists and not yet expired.
            cacheList.TryGetValue(key, out CachedObject obj);
            if (obj != null)
                obj.ExpiryDate = expiryDate;
        }
    }
}
