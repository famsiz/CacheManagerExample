using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CacheManager_Tests
{
    [TestClass]
    public class UnitTest1
    {
        CacheManager.ICacheManager cacheManager = CacheManager.CacheManager.Instance;
        
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void GetItem_SetBooleanValue_ShouldGetGivenValue()
        {
            //1- Add temp record
            string key = "custName";
            string value = "I am Oz";
            cacheManager.SetItem(key, value);

            //2- call Test Method and Check
            string newValue = (string)cacheManager.GetItem(key);
            Assert.AreEqual(value, newValue, "CacheManager could not get expected value!");

            //3- Remove Temp record
            RemoveAllItems_SetTempValues_removeAll_ShouldGetANewDummyValue();
        }

        [TestMethod]
        public void GetItem_SetThreeDifferentItems_TryToGetFirstOne_MustBeRemovedAsCountIs2()
        {
            //Here we will make a simple test to show that the oldest record will be removed 

            //1- Add temp records
            string key1 = "name";
            string value1 = "Ozgur";
            cacheManager.SetItem(key1, value1);
            string key2 = "surname";
            string value2 = "Goktas";
            cacheManager.SetItem(key2, value2);
            string key3 = "city";
            string value3 = "London";
            cacheManager.SetItem(key3, value3);

            //2- call Test Method and Check
            //first added item was name, so that should be a dummy date according to our code
            string name = (string)cacheManager.GetItem(key1);
            Assert.AreNotEqual(name, value1, "CacheManager could not remove the oldest item!");

            //Check the third item so that we can see it is still there
            string city = (string)cacheManager.GetItem(key3);
            Assert.AreEqual(city, value3, "CacheManager could not update cache items list properly!");

            //3- Remove Temp record
            RemoveAllItems_SetTempValues_removeAll_ShouldGetANewDummyValue();
        }

        [TestMethod]
        public void RemoveAllItems_SetTempValues_removeAll_ShouldGetANewDummyValue()
        {
            cacheManager.RemoveAllItems();
        }
    }
}
