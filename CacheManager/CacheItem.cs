using System;

namespace CacheManager
{
    /// <summary>
    /// This is the object which will keep cached items 
    /// </summary>
    public class CacheItem
    {
        /// <summary>
        /// ** Key can be object as well, and cache list can be a dictionary which includes <object, object>
        /// I here I selected string according to my experience for performance. And I will use Hashtable for collection
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// I selected object type to be able to keep different types of data inside, 
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// this infowill be used internally to remove the oldest item once the list is full. 
        /// We can provide this info also but usually Consumer do not need it.
        /// </summary>
        public DateTime LastUsedTime { get; set; }
    }
}
