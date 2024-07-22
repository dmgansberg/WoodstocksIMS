using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WoodStocksIMS.UnitTests
{
    using NUnit.Framework;
    using Woodstocks.WoodstocksIMS.Domain;
    
    /// <summary>
    /// Class contains unit tests to be used in NUint unit testing of the Toy class from the 
    /// WoodstocksIMS system.
    /// </summary>
    [TestFixture]
    public class ToyUnitTests
    {
        /// <summary>
        /// Tests implementation of the equal method for the Toy to ensure that the method 
        /// correctly returns false if a toy is compared to a null object.
        /// </summary>
        [Test]
        public void Equals_OtherIsNullObject_False()
        {
            bool expectedResult = false;
            Object nullObject = null;
            Toy testObject = new Toy("A0001", "Dog with bone", 10, OnOrder.No);
            bool testResult = testObject.Equals(nullObject);
            Assert.AreEqual(expectedResult, testResult, "True was incorrectly returned by Equals");
        }

        /// <summary>
        /// Tests implementation of Equals method for a Toy to ensure that it correctly returns
        /// false if an object with which a toy is to be compared is not convertible to an IToy.
        /// </summary>
        [Test]
        public void Equals_OtherIsNotConvertibleToIToy_False()
        {
            bool expectedResult = false;
            string nonConvertibleToIToy = "";
            Toy testObject = new Toy("A0001", "Dog with bone", 10, OnOrder.No);
            bool testResult = testObject.Equals(nonConvertibleToIToy);
            Assert.AreEqual(expectedResult, testResult, "True was incorrectly returned by Equals");
        }

        /// <summary>
        /// Tests implementation of the Equals method for a Toy to ensure that it correctly returns
        /// true when two equal toys are compared.
        /// </summary>
        [Test]
        public void Equals_TwoEqualToys_True()
        {
            bool expectedResult = true;
            Toy other = new Toy("A0001", "Dog with bone", 10, OnOrder.No);
            Toy testObject = new Toy("A0001", "Dog with bone", 10, OnOrder.No);
            bool testResult = testObject.Equals(other);
            Assert.AreEqual(expectedResult, testResult, "The two Toys are not equals");
        }

        ///<summary>
        ///Tests the implementation of CompareTo by Toy to ensure that it correctly returns 1 if
        ///a toy is compared with a null object.
        /// </summary>
        ///<remarks>Quote from: https://msdn.microsoft.com/en-us/library/system.icomparable.compareto(v=vs.90).aspx
        ///By definition, any object compares greater than (or follows) null"</remarks>
        [Test]
        public void CompareTo_NullObject_1()
        {
            int expectedResult = 1;
            Object nullObject = null;
            Toy testObject = new Toy("A0001", "Dog with bone", 10, OnOrder.No);
            int testResult = testObject.CompareTo(nullObject);
            Assert.AreEqual(expectedResult, testResult);
        }

        /// <summary>
        /// Tests the implementation of CompareTo by Toy to ensure that it correctly throws an 
        /// ArgumentException if a toy is compared with an object that isn't convertible to an IToy.
        /// </summary>
        [Test]
        public void CompareTo_OtherNotConvertibleToIToy_ArgumentExceptionThrown()
        {
            bool exceptionExpected = true;
            bool exceptionThrown = false;
            string other = "";
            try
            {

                Toy testObject = new Toy("A0001", "Dog with bone", 10, OnOrder.No);
                testObject.CompareTo(other);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }
            finally
            {
                Assert.AreEqual(exceptionExpected, exceptionThrown);
            }

        }

        /// <summary>
        /// Tests the SortByCurrentCount implmentation of a Toy by ensure that toys with the same
        /// current count appear in their item code order.
        /// </summary>
        [Test]
        public void SortByCurrentCount_ItemsWithSameCount_ItemsAppearInItemCodeOrder()
        {
            Toys expectedResult = ExpectedResult(); //retrieve the expected result of the test.
            Toys testObject = new Toys();
            testObject.Add(new Toy("A0003", "Tonka truck", 10, OnOrder.No));
            testObject.Add(new Toy("A0001", "Dog with bone", 10, OnOrder.No));
            testObject.Add(new Toy("A0002", "Bear with phone", 10, OnOrder.No));
            testObject.SortByCurrentCount(SortOrder.Ascending);
            CollectionAssert.AreEqual(expectedResult, testObject);
            
        }

        /// <summary>
        /// Gets the expected result for the above test that tests the implementation of the SortByCurrentCount
        /// method.
        /// </summary>
        /// <returns></returns>
        protected Toys ExpectedResult()
        {
            Toys expectedResult = new Toys();
            expectedResult.Add(new Toy("A0001", "Dog with bone", 10, OnOrder.No));
            expectedResult.Add(new Toy("A0002", "Bear with phone", 10, OnOrder.No));
            expectedResult.Add(new Toy("A0003", "Tonka truck", 10, OnOrder.No));
            return expectedResult;
        }
    }
}
