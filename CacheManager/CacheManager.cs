using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CacheManager
{
    public class CacheManager : ICacheManager
    {
        #region Members
        /// <summary>
        /// Static global instance of CacheManager
        /// </summary>
        private static CacheManager cacheManager = null;

        /// <summary>
        /// This is our cached objects collection.
        /// By default we set its size as 100 but user can set it if they want.
        /// </summary>
        private static List<CacheItem> lstCachedItems = new List<CacheItem>();

        private static int cacheItemsMaxCount = 0;
        #endregion

        #region C'tors

        /// <summary>
        /// CacheManager static instance
        /// </summary>
        public static CacheManager Instance
        {
            get
            {
                if (cacheManager == null)
                    cacheManager = new CacheManager();

                return cacheManager;
            }
        }
        #endregion

        #region Public Methods

        #region ICacheManager Implemented Methods
        public void RemoveAllItems()
        {
            try
            {
                lstCachedItems = new List<CacheItem>();
            }
            catch (Exception ex)
            {
                LogAndThrowException(ex);
            }
        }

        public object GetItem(string key)
        {
            object result = null;
            try
            {
                CacheItem cacheItem = (CacheItem)lstCachedItems.FirstOrDefault(x => x.Key.Equals(key));
                if (cacheItem != null)
                {
                    result = cacheItem.Value;
                    cacheItem.LastUsedTime = DateTime.Now;
                }
                else
                {
                    //In here we can get the item from database, file etc according to the design.
                    //For now I will return date time as string

                    result = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    lstCachedItems.Add(new CacheItem()
                    {
                        Key = key,
                        Value = result,
                        LastUsedTime = DateTime.Now
                    });

                    lstCachedItems.OrderBy(x => x.LastUsedTime);

                    for (int i = 0; i < lstCachedItems.Count - cacheItemsMaxCount; i++)
                    {
                        lstCachedItems.RemoveAt(0);
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndThrowException(ex);
            }

            return result;
        }

        public bool RemoveItem(string key)
        {
            bool result = false;
            try
            {
                CacheItem cacheItem = (CacheItem)lstCachedItems.FirstOrDefault(x => x.Key.Contains(key));
                if (cacheItem != null)
                {
                    lstCachedItems.Remove(cacheItem);
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogAndThrowException(ex);
            }

            return result;
        }

        public void SetItem(string key, object value)
        {
            try
            {
                CacheItem cacheItem = (CacheItem)lstCachedItems.FirstOrDefault(x => x.Key.Equals(key));
                if (cacheItem != null)
                {//update
                    cacheItem.Value = value;
                    cacheItem.LastUsedTime = DateTime.Now;
                }
                else
                {//add new record
                    lstCachedItems.Add(new CacheItem()
                    {
                        Key = key,
                        Value = value,
                        LastUsedTime = DateTime.Now
                    });

                    lstCachedItems.OrderBy(x => x.LastUsedTime);

                    for (int i = 0; i < lstCachedItems.Count - GetCacheItemsListCount(); i++)
                    {
                        lstCachedItems.RemoveAt(0);
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndThrowException(ex);
            }
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Logs the exception with the method name info
        /// </summary>
        /// <param name="ex"><see cref="Exception"/> to log</param>
        private void LogAndThrowException(Exception ex)
        {
            //This is first time I am using StackFrame, not sure about its performance but made code more clean.
            try
            {
                StackFrame stackFrame = new StackFrame(1);
                string errMsg = $@"Error received in method: {stackFrame.GetMethod().Name}. Error details: {ex.ToString()}";

                Trace.WriteLine(errMsg);
                throw new Exception(errMsg);

            }
            catch (Exception exc)
            {
                Trace.WriteLine($@"Unexpected error received while logging exception. Error detail: {exc.ToString()}");
            }
        }

        private static int GetCacheItemsListCount()
        {
            if (cacheItemsMaxCount == 0)
            {
                cacheItemsMaxCount = Properties.Settings.Default.CacheItemsListCount;
                if (cacheItemsMaxCount < 2 || cacheItemsMaxCount > 1000) //always beter to limit, these can be parameters as well
                {
                    Trace.WriteLine($"Invalid value for CacheItemsListCount parameter! default value will be used!");
                    cacheItemsMaxCount = 100;
                }
            }

            return cacheItemsMaxCount;
        }

        #endregion
    }
}
