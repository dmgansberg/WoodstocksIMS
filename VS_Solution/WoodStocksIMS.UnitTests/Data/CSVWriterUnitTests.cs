using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace WoodStocksIMS.UnitTests
{
    using Woodstocks.WoodstocksIMS.Data.CSV;

    /// <summary>
    /// Contains the unit tests to test a <see cref="CSVWriter"/>.
    /// </summary>
    [TestFixture]
    public class CSVWriterUnitTests
    {
        /// <summary>
        /// Runs a unit test to ensure that a header is written correctly to a new file.
        /// </summary>
        // Creates a CSVWriter for a new file (must ensure that the file doesn't exist).
        // Writes a header and reads it back. Ensures that the header data matches the
        // data that was intended to be written to the file.
        [Test]
        public void WriteCSVRecord_WriteHeaderNewFile_ItemCodeItemDescriptionCurrentCountOnOrder()
        {
            CSVHeader expectedHeader = new CSVHeader("Item Code","Item Description","Current Count","On Order");
            string filepath = @"Test Data/writer1.csv";
            CSVWriter writer = new CSVWriter(filepath, FileMode.Create);
            writer.WriteCSVRecord(expectedHeader);
            writer.Close();
            CSVReader reader = new CSVReader(filepath, FileMode.Open, false);
            CSVHeader testResult = reader.ReadHeader(true);
            reader.Close();
            CollectionAssert.AreEquivalent(expectedHeader, testResult, "Header data is incorrect");
        }

        /// <summary>
        /// Runs a unit test that writes a header and a set of records to a new file.
        /// </summary>
        
        // Creates a set of test data and a writer to write the data to the file. 
        // Reads the test data back after writing and ensures that what is read back
        // matches what was intended to be written to the file.
        [Test]
        public void WriteCSVRecord_WriteHeaderAndRecords_TestCase1Data()
        {
            string filepath = @"Test Data/writer2.csv";
            List<CSVRecord> expectedData = GetTestCase1Data();
            CSVWriter writer = new CSVWriter(filepath, FileMode.Create);
            foreach (CSVRecord record in expectedData)
            {
                writer.WriteCSVRecord(record);
            }
            writer.Close();
            CSVReader reader = new CSVReader(filepath, FileMode.Open, false);
            List<CSVRecord> testResult = new List<CSVRecord>();
            testResult.Add(reader.ReadHeader(true));
            testResult.Add(reader.ReadDataRecord(1, true));
            testResult.Add(reader.ReadDataRecord(2, true));
            testResult.Add(reader.ReadDataRecord(3, true));
            testResult.Add(reader.ReadDataRecord(4, true));
            testResult.Add(reader.ReadDataRecord(5, true));
            testResult.Add(reader.ReadDataRecord(6, true));
            testResult.Add(reader.ReadDataRecord(7, true));
            testResult.Add(reader.ReadDataRecord(8, true));
            testResult.Add(reader.ReadDataRecord(9, true));
            testResult.Add(reader.ReadDataRecord(10, true));
            reader.Close();
            CollectionAssert.AreEquivalent(expectedData, testResult, "Data written to file is incorrect");
        }

        private List<CSVRecord> GetTestCase1Data()
        {
            CSVHeader header = new CSVHeader("Item Code","Item Description","Current Count","On Order");
            CSVDataRecord dataRecord1 = new CSVDataRecord(new List<string>{"A0001","Horse on Wheels","5","No"});
            CSVDataRecord dataRecord2 = new CSVDataRecord(new List<string>{"A0002","Elephant on Wheels","2","No" }); 
            CSVDataRecord dataRecord3 = new CSVDataRecord(new List<string>{"A0012","Cart with Blocks (50)","0","Yes" } );
            CSVDataRecord dataRecord4 = new CSVDataRecord(new List<string>{"A0020","Train with 5 Carriage","2","No"} );
            CSVDataRecord dataRecord5 = new CSVDataRecord(new List<string>{"A0007","Clown","5","No" } );
            CSVDataRecord dataRecord6 = new CSVDataRecord(new List<string> { "A0030", "Barn", "12", "Yes" });
            CSVDataRecord dataRecord7 = new CSVDataRecord(new List<string> { "A0028", "Farmhouse", "6", "Yes" });
            CSVDataRecord dataRecord8 = new CSVDataRecord(new List<string> { "A0021", "Building Blocks (20)", "15", "No" });
            CSVDataRecord dataRecord9 = new CSVDataRecord(new List<string> { "A0037", "Building (tall)", "4", "No" });
            CSVDataRecord dataRecord10 = new CSVDataRecord(new List<string> { "A0029", "Fencing", "22", "Yes" });
            List<CSVRecord> recordSet = new List<CSVRecord>();
            recordSet.Add(header);
            recordSet.Add(dataRecord1);
            recordSet.Add(dataRecord2);
            recordSet.Add(dataRecord3);
            recordSet.Add(dataRecord4);
            recordSet.Add(dataRecord5);
            recordSet.Add(dataRecord6);
            recordSet.Add(dataRecord7);
            recordSet.Add(dataRecord8);
            recordSet.Add(dataRecord9);
            recordSet.Add(dataRecord10);
            return recordSet;
        }
    }
}
