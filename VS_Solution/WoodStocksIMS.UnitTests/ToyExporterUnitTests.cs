using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace WoodStocksIMS.UnitTests
{
    using Woodstocks.WoodstocksIMS.Domain;
    using Woodstocks.WoodstocksIMS.Data;
    using Woodstocks.WoodstocksIMS.Data.CSV;
    using NUnit.Framework;

    /// <summary>
    /// Unit test class containing unit tests for ToyExporterCSV.
    /// </summary>
    [TestFixture]
    class ToyExporterCSVUnitTests
    {

        delegate void ExportAsyncCaller(string filename, IToys data);
        delegate void ImportAsyncCaller(string filename);
        //Tests that the ExportCompleted event is raised when 
        //the operation has completed. 
        [Test]
        public static void ExportAsync_WorkerCompleted_ExportCompletedRaised()
        {
            bool eventExpected = true;
            bool eventRaised = false;
            ToyExporterCSV exporter = new ToyExporterCSV();
            exporter.ExportCompleted += delegate(object sender,
                System.ComponentModel.AsyncCompletedEventArgs exportCompletedEventData)
            {
                eventRaised = true;
            };
            //pass a collection of at least one toy to ensure that 
            //exception is not thrown due to invalid data argument.
            IToys toys = new Toys();
            toys.Add(new Toy("A0001", "Dog with home", "5", "No"));
            ExportAsyncCaller caller = exporter.ExportAsync;
            IAsyncResult result = caller.BeginInvoke(@"C:\StockFile\filenotfound", toys, null, null);
            Thread.Sleep(1000); //Allow the backgroundworker thread to start
            caller.EndInvoke(result);
            //caller.EndInvoke(result);
            Assert.AreEqual(eventExpected, eventRaised);
        }

        //Tests that the method throws an exception when an error occurs by 
        //invoking the method when the file is not present. The method should
        //throw an exception if the file is not present. The Error property 
        //of the event completion data will not be null and will be a FileNotFoundException
        //if the exception was thrown correctly. The testResult is set to true if so.
        [Test]
        public static void ExportToysAsync_FileNotPresent_FileNotFoundExceptionThrown()
        {
            bool expectedResult = true;
            bool testResult = false;
            ToyExporterCSV exporter = new ToyExporterCSV();
            exporter.ExportCompleted += delegate(object exp,
                System.ComponentModel.AsyncCompletedEventArgs exportCompletedEventData)
            {
                if ((exportCompletedEventData.Error != null) && (exportCompletedEventData.Error is FileNotFoundException))
                {
                    testResult = true;
                }
                Assert.AreEqual(expectedResult, testResult);
            };
            //pass a collection of at least one toy to ensure that 
            //exception is not thrown due to invalid data argument.
            IToys toys = new Toys();
            toys.Add(new Toy("A0001", "Dog with home", "5", "No"));
            exporter.ExportAsync(@"C:\StockFile\filenotfound.csv", toys);
        }

        //Tests that the method throws an ArgumentNullException if the data to export
        //(i.e. data argument) is null.
        [Test]
        public static void ExportAsync_ExportDataIsNull_ArgumentNullExceptionThrown()
        {
            bool expectedResult = true;
            bool testResult = false;
            ToyExporterCSV exporter = new ToyExporterCSV();
            try
            {
                exporter.ExportAsync(@"C:\StockFile\stocklist.csv", null);
            }
            catch (ArgumentNullException)
            {
                testResult = true;
            }
            Assert.AreEqual(expectedResult, testResult);
        }

        //Tests that the method throws an ArgumentNullException if the filename, including
        //the path is null.
        [Test]
        public static void ExportAsync_FilenameIsNull_ArgumentNullExceptionThrown()
        {
            bool expectedResult = true;
            bool testResult = false;
            ToyExporterCSV exporter = new ToyExporterCSV();
            try
            {
                //pass an empty toys so that the exception isn't raised 
                //because data is null.
                exporter.ExportAsync(null, new Toys());
            }
            catch (ArgumentNullException)
            {
                testResult = true;
            }
            Assert.AreEqual(expectedResult, testResult);
        }

        //Tests that the method throws an ArgumentNullException if the filename, 
        //that includes the path, is empty string. 
        [Test]
        public static void ExportAsync_FilenameIsEmptyString_ArgumentNullExceptionThrown()
        {
            bool expectedResult = true;
            bool testResult = false;
            ToyExporterCSV exporter = new ToyExporterCSV();
            try
            {
                //pass a collection of at least one toy to ensure that 
                //exception is not thrown due to invalid data argument.
                IToys toys = new Toys();
                toys.Add(new Toy("A0001", "Dog with home", "5", "No"));
                exporter.ExportAsync(@"C:\StockFile\stocklist.csv", toys);
                exporter.ExportAsync("", toys);
            }
            catch (ArgumentNullException)
            {
                testResult = true;
            }
            Assert.AreEqual(expectedResult, testResult);
        }

        //Tests that the method to ensure that the InvalidOperationException is thrown
        //when there is an existing worker. By design the Exporter only supports 
        //a single export operation at a time so if the worker is present (i.e. non-null)
        //the exporter cannot carry out the operation.
        [Test]
        public static void ExportAsync_WorkerIsNull_NoException()
        {
            bool exceptionExpected = false;
            bool exceptionThrown = true;
            ToyExporterCSV exporter = new ToyExporterCSV();
            try
            {
                //pass a toys collection with at least one toy to ensure other
                //exceptions not thrown due to toys being null or empty.
                IToys toys = new Toys();
                toys.Add(new Toy("A0001", "Dog with home", "5", "No"));
                exporter.ExportAsync(@"C:\StockFile\stocklist.csv", toys);
                exceptionThrown = false;

            }
            catch (InvalidOperationException)
            {
                exceptionThrown = true;
            }
            finally
            {
                Assert.AreEqual(exceptionExpected, exceptionThrown);
            }
        }

        //Tests that the method to ensure that the InvalidOperationException is thrown
        //when there is an existing worker. By design the Exporter only supports 
        //a single export operation at a time so if the worker is present (i.e. non-null)
        //the exporter cannot carry out the operation.
        [Test]
        public static void ExportAsync_WorkerIsNotBusy_NoException()
        {
            bool exceptionExpected = false;
            bool exceptionThrown = true;
            ToyExporterCSV exporter = new ToyExporterCSV();
            try
            {
                //pass a toys collection with one toy to ensure other exceptions 
                //not thrown.
                IToys toys = new Toys();
                toys.Add(new Toy("A0001", "Dog with home", "5", "No"));
                exporter.ExportAsync(@"C:\StockFile\stocklist.csv", toys);
                exceptionThrown = false;
                Assert.AreEqual(exceptionExpected, exceptionThrown);
            }
            catch (InvalidOperationException)
            {
                exceptionThrown = true;
                Assert.AreEqual(exceptionExpected, exceptionThrown);
            }
          
        }

       
    }     
}
