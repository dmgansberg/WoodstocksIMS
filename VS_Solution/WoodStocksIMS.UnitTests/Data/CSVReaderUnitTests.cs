using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WoodStocksIMS.UnitTests
{
    using NUnit.Framework;
    using Woodstocks.WoodstocksIMS.Data.CSV;

    /// <summary>
    /// Contains the unit tests for <see cref="CSVReader"/>.
    /// </summary>
    [TestFixture]
    public class CSVReaderUnitTests
    {
        /// <summary>
        /// Runs a unit test to ensure that the <see cref="CSVReader"/>
        /// can correctly read a header record that is contained on the first line
        /// of csv data file. 
        /// </summary>
        // Creates a CSVReader and attempts to read the header from a 
        // csv data file. Ensures that the header read by the reader 
        // matches the expected header result. 
        [Test]
        public void ReadHeader_SingleLineHeaderOnFirstLine_MatchingHeader()
        {
            CSVHeader expectedResult = new CSVHeader("Item Code", "Item Description", "Current Count", "On Order");
            CSVReader csvReader = new CSVReader(@"Test Data/csvReader2.csv", FileMode.Open, false);
            CSVHeader testResult = csvReader.ReadHeader(true);
            CollectionAssert.AreEquivalent(expectedResult, testResult, "Retrieved header is incorrect");

        }

        /// <summary>
        /// Runs a unit test to ensure that a <see cref="CSVReader"/> can 
        /// correctly handle reading a header that spans multiple lines of a 
        /// csv data file. 
        /// </summary>
        [Test]
        // Creates a CSVReader and attempts to read the header from a csv data file.
        // The header starts on line 1 and continues onto line 2 of the file.
        // Ensures that the returned header is the same as the expected header.
        public void ReadHeader_MultiLineHeaderStartsLine1ContinuesOnLine2_MatchingHeader()
        {
            CSVHeader expectedResult = new CSVHeader("Item Code", "Item Description", "Current Count", "On Order");
            CSVReader csvReader = new CSVReader(@"Test Data/csvReader3.csv", FileMode.Open, false);
            CSVHeader testResult = csvReader.ReadHeader(true);
            CollectionAssert.AreEquivalent(expectedResult, testResult, "Retrieved header is incorrect");

        }

        /// <summary>
        /// Runs a unit test to ensure that a <see cref="CSVReader"/> can correctly
        /// read a header even though it it preceded by white space (empty lines)
        /// in the csv data file. 
        /// </summary>
        //Creates a CSVReader and attempts to read the header from a csv data file. 
        //The header in the file is preceded by white space (empty lines). 
        //Ensures the header matches the expected header. 
        [Test]
        public void ReadHeader_MultiLineHeaderPrecededByEmptyLines_MatchingHeader()
        {
            CSVHeader expectedResult = new CSVHeader("Item Code", "Item Description", "Current Count", "On Order");
            CSVReader csvReader = new CSVReader(@"Test Data/csvReader4.csv", FileMode.Open, false);
            CSVHeader testResult = csvReader.ReadHeader(true);
            CollectionAssert.AreEquivalent(expectedResult, testResult, "Retrieved header is incorrect");

        }

        /// <summary>
        /// Runs a unit test that reads a CSVDataRecord from a csv data file. 
        /// </summary>
        // Creates a CSVReader and attempts to read a data record from a csv data file
        // that contains 40 csv data records. 
        // Attempts to read the 15th data record from the file. 
        // Ensures that the returned result matches the expected data of the 15th 
        // data record.
        [Test]
        public void ReadDataRecord_ReadRecord15_A0015TrainWith0CarriageNo()
        {
            List<string> values = new List<string>();
            values.Add("A0015");
            values.Add("Train with 0 Carriage");
            values.Add("12");
            values.Add("No");
            CSVDataRecord expectedResult = new CSVDataRecord(values);
            CSVReader csvReader = new CSVReader(@"Test Data/csvReader4.csv", FileMode.Open, false);
            CSVDataRecord testResult = csvReader.ReadDataRecord(15,true);
            CollectionAssert.AreEquivalent(expectedResult, testResult, "Retrieved header is incorrect");

        }

        /// <summary>
        /// Runs a unit test that reads a CSVDataRecord from a csv data file. 
        /// </summary>
        // Creates a CSVReader to read data from a csv data file that contains 40 csv 
        // data records. Attempts to read the last data record from the file. 
        // Ensures that the returned result matches the expected data of the last data
        // record.
        [Test]
        public void ReadDataRecord_ReadRecord40_A0040PetrolStation4Yes()
        {
            List<string> values = new List<string>();
            values.Add("A0040");
            values.Add("Petrol Station");
            values.Add("4");
            values.Add("Yes");
            CSVDataRecord expectedResult = new CSVDataRecord(values);
            CSVReader csvReader = new CSVReader(@"Test Data/csvReader4.csv", FileMode.Open, false);
            CSVDataRecord testResult = csvReader.ReadDataRecord(40, true);
            CollectionAssert.AreEquivalent(expectedResult, testResult, "Retrieved header is incorrect");
        }

        /// <summary>
        /// Runs a unit test that attempts to read a non-existing data record
        /// from a csv data file. 
        /// </summary>
        // Creates a CSVReader and attempts to read a data record from a csv data file
        // that contains 40 csv data records. Attempts to read the 41st record from the 
        // file. 
        // Ensures that an ArgumentOutOfRangeException is thrown.
        [Test]
        public void ReadDataRecord_ReadRecord41_ArgumentOutofRangeException()
        {
            bool expectedException = true;
            bool exceptionThrown = false;
            try
            {
                CSVReader csvReader = new CSVReader(@"Test Data/csvReader4.csv", FileMode.Open, false);
                CSVDataRecord testResult = csvReader.ReadDataRecord(41, true);
            }
            catch (ArgumentOutOfRangeException)
            {
                exceptionThrown = true;
            }
            finally
            {
                Assert.AreEqual(expectedException, exceptionThrown, "An exception wasn't thrown");
            }
        }

        /// <summary>
        /// Runs a unit test that reads a csv data record that contains a missing
        /// field value from a csv data file. 
        /// </summary>
        
        // Creates a CSVReader and attempts to read a data recorrd from a csv data file
        // that contains 40 csv data records. Attempts to read the 23rd data record
        // from the file which has its last field value missing.

        // Ensures that a MissingValueException is thrown.

        // Requires the header be read prior to reading the record so as to detect
        // that a value for a defined field is missing.

        [Test]
        public void ReadDataRecord_Record23MissingFieldValue_MissingValueExceptionThrown()
        {
            bool expectedException = true;
            bool exceptionThrown = false;
            try
            {
                CSVReader csvReader = new CSVReader(@"Test Data/csvReader5.csv", FileMode.Open, false);
                //An exception is only thrown if the header has been read first 
                //because the check is to ensure that the data record has the 
                //same number of values as the number of fields (values) defined by the header.
                CSVHeader header = csvReader.ReadHeader(true);
                CSVDataRecord testResult = csvReader.ReadDataRecord(23, true);
            }
            catch (MissingValueException)
            {
                exceptionThrown = true;
            }
            finally
            {
                Assert.AreEqual(expectedException, exceptionThrown, "An exception wasn't thrown");
            }
        }

        /// <summary>
        /// Runs a unit test that reads a csv data record that contains a missing
        /// field value from a csv data file. 
        /// </summary>
        
        // Creates a CSVReader and attempts to read a data recorrd from a csv data file
        // that contains 40 csv data records. Attempts to read the 23rd data record
        // from the file which has the value of its description field missing.
  
        // Ensures that the MissingValueException is thrown.

        // Requires the header be read prior to reading the record so as to detect
        // that a value for a defined field is missing.
        [Test]
        public void ReadDataRecord_Record23ItemDescriptionValueMissing_MissingValueException()
        {
            bool expectedException = true;
            bool exceptionThrown = false;
            try
            {
                CSVReader csvReader = new CSVReader(@"Test Data/csvReader6.csv", FileMode.Open, false);
                //An exception is only thrown if the header has been read first 
                //because the check is to ensure that the data record has the 
                //same number of values as the number of fields (values) defined by the header.
                CSVHeader header = csvReader.ReadHeader(true);
                CSVDataRecord testResult = csvReader.ReadDataRecord(23, true);
            }
            catch (MissingValueException)
            {
                exceptionThrown = true;
            }
            finally
            {
                Assert.AreEqual(expectedException, exceptionThrown, "An exception wasn't thrown");
            }
        }
    }
}
