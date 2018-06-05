using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIZZA.Core;

namespace Tests
{
    [TestClass]
    public class TestPIZZADatatypes
    {
        [TestMethod]
        public void TestPIZZAInt()
        {
            var i = new PIZZAInt()
            {
                Value = 257,
            };

            var bytes = i.GetBytes();
            var otherI = PIZZAInt.FromBytes(bytes);

            Assert.AreEqual(i.Value, otherI.Value);
            Assert.AreEqual(257, otherI.Value);
        }

        [TestMethod]
        public void TestPIZZAInt3()
        {
            var i = new PIZZAInt3()
            {
                Value = 333000,
            };

            var bytes = i.GetBytes();
            var otherI = PIZZAInt3.FromBytes(bytes);

            Assert.AreEqual(i.Value, otherI.Value);
            Assert.AreEqual(333000, otherI.Value);
        }

        [TestMethod]
        public void TestPizzaInz3Frombytes()
        {
            var exp = new byte[] { 0, 0, 17 };
            var res = PIZZAInt3.FromBytes(exp);

            Assert.AreEqual(17, res.Value);
        }

        [TestMethod]
        public void TestPizzaInz3Frombytes1()
        {
            var exp = new byte[] { 0, 0xff, 0x11 };
            var res = PIZZAInt3.FromBytes(exp);

            Assert.AreEqual(65297, res.Value);
        }
        [TestMethod]
        public void TestPizzaInz3Frombytes2()
        {
            var exp = new byte[] { 255, 255, 16 };
            var res = PIZZAInt3.FromBytes(exp);

            Assert.AreEqual(16776976, res.Value);
        }
        [TestMethod]
        public void TestPizzaInz3Getbytes()
        {
            var exp = new byte[] { 255, 255, 16 };
            var res = new PIZZAInt3() { Value = 16776976 };

            Assert.IsTrue(exp.CompareMenberwise(res.GetBytes()));
        }



        [TestMethod]
        public void TestPIZZAInt5()
        {
            var i = new PIZZAInt5()
            {
                Value = 999999999,
            };

            var bytes = i.GetBytes();
            var otherI = PIZZAInt5.FromBytes(bytes);

            Assert.AreEqual(i.Value, otherI.Value);
            Assert.AreEqual(999999999, otherI.Value);
        }

        [TestMethod]
        public void TestPIZZAString()
        {
            var i = new PIZZAString()
            {
                Value = "lollollol",
            };

            var bytes = i.GetBytes();
            var otherI = PIZZAString.FromBytes(bytes);

            Assert.AreEqual(i.Value, otherI.Value);
            Assert.AreEqual(9, otherI.Length.Value);
        }
    }
}
