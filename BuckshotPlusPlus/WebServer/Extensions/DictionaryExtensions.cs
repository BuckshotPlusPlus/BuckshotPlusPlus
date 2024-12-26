using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace BuckshotPlusPlus.WebServer.Extensions
{
    public static class DictionaryExtensions
    {
        public static async Task<TValue> GetOrAddAsync<TKey, TValue>(
    this ConcurrentDictionary<TKey, TValue> dictionary,
    TKey key,
    Func<TKey, Task<TValue>> valueFactory) where TValue : class
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (dictionary.TryGetValue(key, out TValue value) && value != null)
            {
                return value;
            }

            value = await valueFactory(key);
            if (value != null)
            {
                dictionary.TryAdd(key, value);
            }
            return value;
        }
    }
}