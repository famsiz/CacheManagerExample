namespace CacheManager
{
    public interface ICacheManager
    {
        /// <summary>
        /// This method returns cached item according to entered key
        /// </summary>
        /// <param name="key">Cached item key</param>
        /// <returns>Cached item value as an <see cref="object"/>. Returns current time value to simulate db, if item does not exist</returns>
        object GetItem(string key);

        /// <summary>
        /// This method add/update item in the cache items list
        /// </summary>
        /// <param name="key">Cache item key</param>
        /// <param name="value">Cache item value</param>
        void SetItem(string key, object value);

        /// <summary>
        /// Removes a specific cache item(if exists) according to the entered key
        /// </summary>
        /// <param name="key">cache item key</param>
        /// <returns>True: for success, False: for fail</returns>
        bool RemoveItem(string key);
        
        /// <summary>
        /// removes all cached items
        /// </summary>
        void RemoveAllItems();

    }
}
