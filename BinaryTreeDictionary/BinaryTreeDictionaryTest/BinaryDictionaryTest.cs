using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BinaryTreeDictionary;
using System.Collections.Generic;

namespace BinaryTreeDictionaryTest
{
    [TestClass]
    public class BinaryDictionaryTest
    {
        CustomDictionary<int, int> myCD;

        [TestInitialize]
        public void Initialize()
        {
            myCD = new CustomDictionary<int, int>();
        }

        [TestMethod]
        public void Ctor_Test()
        {
            Assert.AreNotEqual(null, myCD);
        }

        [TestMethod]
        public void Indexer_Test()
        {
            myCD.Add(5, 555);
            myCD.Add(7, 755);
            myCD.Add(9, 955);
            myCD.Add(10, 1055);

            myCD[2] = 855;

            Assert.AreEqual(4, myCD.Count);
            Assert.AreEqual(755, myCD[1]);
            Assert.AreEqual(1055, myCD[3]);
        }

        [TestMethod]
        public void AddItem_Test()
        {
            myCD.Add(5, 555);
            myCD.Add(7, 755);
            myCD.Add(9, 955);
            myCD.Add(10, 1055);

            Assert.AreEqual(4, myCD.Count);
            Assert.AreEqual(true, myCD.ContainsKey(5));
            Assert.AreEqual(true, myCD.ContainsKey(7));
            Assert.AreEqual(true, myCD.ContainsKey(9));
            Assert.AreEqual(true, myCD.ContainsKey(10));
        }

        [TestMethod]
        public void AddPair_Test()
        {
            KeyValuePair<int, int> pair1 = new KeyValuePair<int, int>(01,111);
            KeyValuePair<int, int> pair2 = new KeyValuePair<int, int>(02, 222);
            KeyValuePair<int, int> pair3 = new KeyValuePair<int, int>(03, 333);
            KeyValuePair<int, int> pair4 = new KeyValuePair<int, int>(04, 444);

            myCD.Add(pair1);
            myCD.Add(pair2);
            myCD.Add(pair3);
            myCD.Add(pair4);

            Assert.AreEqual(4, myCD.Count);
            Assert.AreEqual(true, myCD.Contains(pair1));
            Assert.AreEqual(true, myCD.Contains(pair4));
        }


        [TestMethod]
        public void GetKeys_Test()
        {
            myCD.Add(1, 11);
            myCD.Add(2, 22);
            myCD.Add(3, 33);

            ICollection<int> coll = myCD.Keys;

            Assert.AreEqual(myCD.Count, coll.Count);
            Assert.AreEqual(true, coll.Contains(3));
            Assert.AreEqual(true, coll.Contains(1));
            Assert.AreEqual(true, coll.Contains(2));
        }

        [TestMethod]
        public void GetValues_Test()
        {
            myCD.Add(1, 11);
            myCD.Add(2, 22);
            myCD.Add(3, 33);

            ICollection<int> coll = myCD.Values;

            Assert.AreEqual(myCD.Count, coll.Count);
            Assert.AreEqual(true, coll.Contains(33));
            Assert.AreEqual(true, coll.Contains(11));
            Assert.AreEqual(true, coll.Contains(22));
        }

        [TestMethod]
        public void Contains_Test()
        {
            myCD.Add(1, 11);
            myCD.Add(2, 22);
            myCD.Add(3, 33);

            Assert.AreEqual(true, myCD.Contains(new KeyValuePair<int, int>(1, 11)));
            Assert.AreEqual(true, myCD.Contains(new KeyValuePair<int, int>(2, 22)));
            Assert.AreEqual(true, myCD.Contains(new KeyValuePair<int, int>(3, 33)));
            Assert.AreEqual(false, myCD.Contains(new KeyValuePair<int, int>(4, 44)));
        }

        [TestMethod]
        public void CopyTo_Test()
        {
            myCD.Add(1, 11);
            myCD.Add(2, 22);
            myCD.Add(3, 33);

            KeyValuePair<int, int>[] pair1 = new KeyValuePair<int, int>[myCD.Count];
            myCD.CopyTo(pair1, 0);

            Assert.AreEqual(new KeyValuePair<int, int>(1, 11), pair1[0]);
        }

        [TestMethod]
        public void RemoveByKey_Test()
        {
            myCD = new CustomDictionary<int, int>(true);

            myCD.Add(1, 11);
            myCD.Add(2, 22);
            myCD.Add(3, 33);
            myCD.Add(4, 44);

            myCD.Remove(0);

            Assert.AreEqual(3, myCD.Count);
            Assert.AreEqual(true, myCD.ContainsKey(4));
            Assert.AreEqual(false, myCD.Contains(new KeyValuePair<int, int>(1, 11)));
            Assert.AreEqual(44, myCD[2]);
        }

        [TestMethod]
        public void RemoveByItem_Test()
        {
            myCD.Add(1, 11);
            myCD.Add(2, 22);
            myCD.Add(3, 33);
            myCD.Add(4, 44);
            myCD.Add(12, 1145);
            myCD.Add(23, 2256);
            myCD.Add(35, 3367);
            myCD.Add(46, 4478);

            myCD.Remove(new KeyValuePair<int, int>(12, 1145));
            myCD.Remove(new KeyValuePair<int, int>(1, 11));
            myCD.Remove(new KeyValuePair<int, int>(35, 3367));

            Assert.AreEqual(5, myCD.Count);
            Assert.AreEqual(true, myCD.Contains(new KeyValuePair<int, int>(2, 22)));
            Assert.AreEqual(true, myCD.Contains(new KeyValuePair<int, int>(3, 33)));
            Assert.AreEqual(true, myCD.Contains(new KeyValuePair<int, int>(4, 44)));
            Assert.AreEqual(false, myCD.Contains(new KeyValuePair<int, int>(12, 1145)));
            Assert.AreEqual(false, myCD.Contains(new KeyValuePair<int, int>(35, 3367)));
        }

        [TestMethod]
        public void TryGetValue_Test()
        {
            myCD.Add(1, 11);
            myCD.Add(2, 22);
            myCD.Add(3, 33);
            myCD.Add(4, 44);

            int temp;

            Assert.AreNotEqual(true, myCD.TryGetValue(55, out temp));
            Assert.AreEqual(true, myCD.TryGetValue(33, out temp));
            Assert.AreEqual(22, temp);
        }
    }
}
