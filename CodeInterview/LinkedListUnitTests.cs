
namespace SetComprehension
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    [TestClass]
    public class LinkedListUnitTests
    {
        [TestMethod]
        public void TestConstructionWith1Item()
        {
            LinkedList<int> l1 = new LinkedList<int>(1);
            Assert.AreEqual(1, l1.Count);
            int count = 0;
            foreach(int i in l1.Items)
            {
                Assert.AreEqual(i, 1);
                count++;
            }
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void TestConstructionWithNItem()
        {
            int[] expectedValues = new int[5] { 1, 23, 2, 5, 1 };

            LinkedList<int> l1 = new LinkedList<int>(1, 5, 2, 23, 1);
            Assert.AreEqual(5, l1.Count);
            int index = 4;
            foreach(int i in l1.Items)
            {
                Assert.AreEqual(expectedValues[index--], i);
            }
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void TestConstructionWith0Item()
        {
            LinkedList<int> l1 = new LinkedList<int>();
            Assert.AreEqual(0, l1.Count);
            foreach (int i in l1.Items)
            {
                Assert.Fail("no items");
            }
        }

        [TestMethod]
        public void TestUnion0andNItems()
        {
            int[] expectedValues = new int[] { 1, 5, 2};

            LinkedList<int> l1 = new LinkedList<int>();
            LinkedList<int> l2 = new LinkedList<int>(1, 5, 2);
            Assert.AreEqual(0, l1.Count);
            Assert.AreEqual(3, l2.Count);
            int totalCount = l1.Count + l2.Count;

            var l3 = l1.Union(l2);
            int index = 0;
            foreach (int i in l3.Items)
            {
                Assert.AreEqual(expectedValues[index++], i);
            }
            Assert.AreEqual(totalCount, index);

            l3 = l2.Union(l1);
            index = 0;
            foreach (int i in l3.Items)
            {
                Assert.AreEqual(expectedValues[index++], i);
            }
            Assert.AreEqual(totalCount, index);
        }

        [TestMethod]
        public void TestUnionNandMItems()
        {
            int[] expectedValues = new int[] { 1, 5, 5, 2, 100};

            LinkedList<int> l1 = new LinkedList<int>(1, 5);
            LinkedList<int> l2 = new LinkedList<int>(5, 2, 100);
            Assert.AreEqual(2, l1.Count);
            Assert.AreEqual(3, l2.Count);
            int totalCount = l1.Count + l2.Count;

            var l3 = l1.Union(l2);
            int index = 0;
            foreach (int i in l3.Items)
            {
                Assert.AreEqual(expectedValues[index++], i);
            }
            Assert.AreEqual(totalCount, index);

            expectedValues = new int[] { 5, 2, 100, 1, 5 };
            l3 = l2.Union(l1);
            index = 0;
            foreach (int i in l3.Items)
            {
                Assert.AreEqual(expectedValues[index++], i);
            }
            Assert.AreEqual(totalCount, index);
        }

        [TestMethod]
        public void TestIntersectNandMItems()
        {
            Func<int, int, bool> comparer = (i1, i2) => i1 == i2;

            HashSet<int> expectedValues = new HashSet<int>(new int[] { 5 });

            LinkedList<int> l1 = new LinkedList<int>(1, 5);
            LinkedList<int> l2 = new LinkedList<int>(5, 2, 100);
            
            var l3 = l1.Intersect(l2, comparer);
            int index = 0;
            foreach (int i in l3.Items)
            {
                Assert.IsTrue(expectedValues.Contains(i));
            }
            Assert.AreEqual(1, index);

            expectedValues = new HashSet<int>(new int[] { 2, 100 });
            l1 = new LinkedList<int>(2, 100);
            l3 = l2.Intersect(l1, comparer);
            index = 0;
            foreach (int i in l3.Items)
            {
                Assert.IsTrue(expectedValues.Contains(i));
            }
            Assert.AreEqual(2, index);
        }

        [TestMethod]
        public void TestJoinItems()
        {

            LinkedList<Order> orders = new LinkedList<Order>(
                new Order 
                { 
                    Id = 1, 
                    ItemCount = 3, 
                    ShipToZip = "81018",
                    Customer = new Customer { CustomerID = 100, Name = "Bob", Zip = "10192" }
                },
                new Order 
                { 
                    Id = 2, 
                    ItemCount = 3, 
                    ShipToZip = "29281",
                    Customer = new Customer { CustomerID = 101, Name = "Mary", Zip = "29281" }
                }
            );
            LinkedList<Customer> customers = new LinkedList<Customer>(
                new Customer 
                { 
                    CustomerID = 100,
                    Name = "Bob",
                    Zip = "10192"
                },
                new Customer 
                { 
                    CustomerID = 101,
                    Name = "Mary",
                    Zip = "29281"
                }
            );

            var joinedList = orders.Join(
                customers, 
                (o, c) => o.ShipToZip == c.Zip, 
                (o, c) => new { Id = o.Id, CustomerId = c.CustomerID }
            );

            Assert.AreEqual(1, joinedList.Count);
        }

        [TestMethod]
        public void TestOrderByNItems()
        {
            LinkedList<string> values = new LinkedList<string>("a", "A", "abc", "ABC");
            string[] expectedValues = new string[] { "a", "abc", "A", "ABC" };

            var result = values.OrderBy(StringComparer.Create(CultureInfo.InvariantCulture, true));
            int index = 0;
            foreach(string s in result.Items)
            {
                Assert.AreEqual(expectedValues[index++], s);
            }
            Assert.AreEqual(4, index);
        }


        private class Order
        {
            public int Id { get; set; }
            public int ItemCount { get; set; }

            public Customer Customer { get; set; }

            public string ShipToZip { get; set; }
        }

        private class Customer
        {
            public int CustomerID { get; set; }

            public string Name { get; set; }

            public string Zip { get; set; }
        }
    }
}
