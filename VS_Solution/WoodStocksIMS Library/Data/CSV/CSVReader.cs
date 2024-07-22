using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Woodstocks.WoodstocksIMS.Data.CSV
{
    /// <summary>
    /// A CSVReader that is used to read records from a csv file.
    /// </summary>
    public class CSVReader : IDisposable
    {
        /// <summary>
        /// The number of records that can be read from the file.
        /// </summary>
        private int recordCount = 0;
        /// <summary>
        /// The index of the next record in the file.
        /// </summary>
        private int nextRecord = 0;

        /// <summary>
        /// The file path of the file, that includes the name of the file.
        /// </summary>
        private string filepath = "";

        /// <summary>
        /// The file mode for accessing the file.
        /// </summary>
        private FileMode fileMode = 0;

        /// <summary>
        /// A csv header that contains the names of fields for records in the file.
        /// </summary>
        private CSVHeader header = null;

        /// <summary>
        /// Specifies whether empty strings are allowed as values in a csv string.
        /// </summary>
        private bool allowEmptyStringValues = false;

        /// <summary>
        /// Whether the file from which the reader reads is open.
        /// </summary>
        private bool isOpen = false;

        /// <summary>
        /// A FileStream for accessing the file.
        /// </summary>
        private FileStream fileStream = null;

        /// <summary>
        /// A StreamReader for reading the content of the file.
        /// </summary>
        private StreamReader streamReader = null;

        /// <summary>
        /// Whether file should be left open by the CSVReader.
        /// </summary>
        private bool leaveFileOpen = true;
       
        /// <summary>
        /// Initialises a CSVReader for reading from a file containing csv records.
        /// </summary>
        /// <param name="filepath">The file path of the file that the CSVReader
        /// should read records from.</param>
        /// <param name="mode">The file mode in which the CSVReader should open the file.</param>
        /// <param name="allowEmptyStringValues">Indicates whether the reader when reading values
        /// should detect a missing value, denoted by an empty string. If set to true then 
        /// upon detecting a missing value the reader will raise a <see cref="MissingValueException"/></param>
        public CSVReader(string filepath, FileMode mode, bool allowEmptyStringValues)
        {
   
            if ((filepath == null) || (filepath == ""))
                throw new ArgumentException("Filepath is null");
            this.filepath = filepath;
            this.fileMode = mode;
            this.recordCount = GetRecordCount();
        }

        /// <summary>
        /// Gets or Sets whether the <see cref="CSVReader"/> should allow empty string values
        /// in a CSVRecord (either <see cref="CSVHeader"/> or <see cref="CSVDataRecord"/>
        /// </summary>
        public bool AllowEmptyStringValues
        {
            get { return allowEmptyStringValues; }
            set { allowEmptyStringValues = value; }
        }

        /// <summary>
        /// Gets the path of the file that the CSVReader should read data from.
        /// </summary>
        public string FilePath
        {
            get { return filepath; }
            set
            {
                if ((filepath == null) || (filepath == ""))
                    throw new Exception();
                this.filepath = value;
            }
        }

        /// <summary>
        /// Returns the number of records available to be read by the <see cref="CSVReader"/>.
        /// </summary>
        public int Records
        {
            get { return recordCount; }
        }

        /// <summary>
        /// Gets whether the file is open or not.
        /// </summary>
        public bool IsOpen
        {
            get { return isOpen; }
        }

        /// <summary>
        /// Gets the zero-based index for the next record to be read. A value of -1 is
        /// returned if the current record is the last readable record.
        /// </summary>
        public int NextRecord
        {
            get
            {
                if (nextRecord >= recordCount)
                {
                    return -1;
                }
                return nextRecord;
            }           
        }

        /// <summary>
        /// Sets the value of the nextRecord field.
        /// </summary>
        /// <param name="recordIndex">The zero-based index of the next record that the
        /// reader should read from the file.</param>
       /*This method is defined, instead of simply defining a set on the NextRecord property,
        *so that users of the reader cannot set the NextRecord value. The reader initialises
        *the nextRecord field by attempting to determine the records stored in the file. */        
        protected void SetNextRecord(int recordIndex)
        {
            nextRecord = recordIndex;
        }

        /// <summary>
        /// Opens the file that the CSVReader should read from.
        /// </summary>
        public void Open()
        {
            if (IsOpen)
                return;
            if (filepath == null)
                throw new ArgumentException("Filepath is null");
            if (filepath == "")
                throw new ArgumentException("Filepath is empty");
            try
            {
                fileStream = new FileStream(filepath, fileMode, FileAccess.Read);
                try
                {
                    streamReader = new StreamReader(fileStream);
                    SetNextRecord(0);
                    isOpen = true;
                }
                catch (Exception)
                {
                    throw;
                }               
            }
            catch (Exception)
            {
                if (streamReader != null)
                {
                    streamReader.Close();                    
                    streamReader = null;
                    fileStream = null; //closing streamReader results in the fileStream being closed as well.
                }
                if (fileStream != null)
                {
                   fileStream.Dispose();
                   fileStream = null;
                }
                throw;            
            }                 
        }

        /// <summary>
        /// Closes the file that the CSVReader has open.
        /// </summary>
        public void Close()
        {            
            Dispose();
        }

        /// <summary>
        /// Gets the number of csv records that are contained in the file.
        /// </summary>
        /// <returns>The number of records, including any header record, that is 
        /// contained in the file.</returns>
        protected virtual int GetRecordCount()
        {
            try
            {
                Open();
                int records = 0;
                string input = null;
                string line = null;
                Open();
                if (streamReader == null)
                    throw new Exception();
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        input += line;
                        if (!IsRecordIncomplete(input))
                            records++;
                    }
                }                
                return records;
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Reads the header record from a file containing csv records.
        /// </summary>
        /// <returns>A <see cref="CSVHeader"/> that contains the values of the header or
        /// null if the header record could not be read.</returns>
        /// <remarks>The method reads the first record of the file and therefore assumes that
        /// the optional header is the first record in the file containing csv records.</remarks>
        ///
        //The header is read by reading the first record contained in tbe file 
        //from which the CSVReader should read csv records.

        //As records are viewed as being zero-indexed, the first record
        //is read by calling ReadRecord passing a recordIndex of 0. 
        //If null is received from the call to ReadRecord null is 
        //returned to indicate that a header could not be read.

        //If the record is read then the record is parsed into its component values
        //which are used to create a CSVHeader. 
        //If the value of allowEmptyStringValues is false, then the values are tested
        //to ensure that they are not empty strings before the CSVHeader is created.
        public CSVHeader ReadHeader(bool removeEscapeCharacter)
        {
            CSVHeader header = null;
            int recordIndex = 0;
            string recordString = ReadRecord(recordIndex);
            if (recordString == null)
                return null;
            CSVParser csvParser = new CSVParser();
            List<string> headerValues = csvParser.Parse(recordString,removeEscapeCharacter);
            if (!allowEmptyStringValues)
            {
                foreach (string value in headerValues)
                {
                    if (value == "")
                        throw new MissingValueException();
                }
            }
            header = new CSVHeader(headerValues);
            this.header = header;
            return this.header;
        }

        /// <summary>
        /// Reads a data record from a csv file and returns the data as a <see cref="CSVDataRecord"/>.
        /// </summary>
        /// <param name="recordIndex">The index of the record to be read from the file.</param>
        /// <param name="removeEscapeCharacter">Indicates whether escape characters (i.e
        /// double quotes should be removed from escaped values within the csv input string.</param>
        /// <returns>A <see cref="CSVDataRecord"/>that contains the record data.</returns>
        
        //Data records are read by is read by reading the first record contained in tbe file 
        //from which the CSVReader should read csv records.

        //If the record is read then the record is parsed into its component values
        //which are used to create a CSVDataRecord. 
        //If the value of allowEmptyStringValues is false, then the values are tested
        //to ensure that they are not empty strings before the CSVDataRecord is created.
        public CSVDataRecord ReadDataRecord(int recordIndex, bool removeEscapeCharacter)
        {
            string recordString = ReadRecord(recordIndex);
            if (recordString == null)
                return null;
            CSVParser csvParser = new CSVParser();
            List<string> recordValues = csvParser.Parse(recordString, removeEscapeCharacter);
            if (!allowEmptyStringValues)
            {
                foreach (string value in recordValues)
                {
                    if (value == "")
                        throw new MissingValueException("The CSVDataRecord contains an empty value.");
                }
            }

            CSVDataRecord record = null;
            if (header != null)
            {
                //ensure that the record has the same amount of values as 
                //the number of fields defined by the header.
                if (header.Count != recordValues.Count)
                    throw new MissingValueException("The number for the CSVDataRecord are not the same as for the CSVHeader.");
                record = new CSVDataRecord(this.header, recordValues);
            }
            else
            {
                record = new CSVDataRecord(recordValues);
            }
            return record;
        }

        /// <summary>
        /// Reads a record from a file and returns the record as a string.
        /// </summary>
        /// <param name="recordIndex">A zero-based index value that indicates the record that should be read
        /// from the file.</param>
        /// <returns>The record as a string.</returns>
        /// <remarks>
        /// <para>The method reads lines from the file, ignoring empty lines, until the record
        /// wanted from the file is read. When the method reads the record that is wanted
        /// the mehtod returns the record as a string.</para>
        /// <para>
        /// The method ensures that a line read from the file constitutes a complete record
        /// by calling the IsRecordComplete method which checks that the line constitutes a 
        /// complete record for a csv record.
        /// </para>
        /// </remarks>
        virtual protected string ReadRecord(int recordIndex)
        {
            //Any attempt to read a record when the record count is 0, returns null to indicate no data.
            if (recordCount == 0)
                return null;
            //A recordIndex equal to or in excess of the record count is an attempt to read an invalid
            //record, because the records are regarded as zero-indexed so the last record is accessed
            //at one less than the total record count.
            if (recordIndex >= recordCount)
                throw new ArgumentOutOfRangeException("RecordIndex is out of range");
            string input = null;
            try
            {
                Open(); //ensures that the files is open.
                string line = null;
                bool incompleteRecord = true;
                while (((line = streamReader.ReadLine()) != null) && (nextRecord <= recordIndex) && (nextRecord < recordCount))
                {
                    if (line != "") //ignore empty lines from the file.
                    {
                        input += line; //ensures that lines read from the file are concatenated whilst the record is incomplete.
                        incompleteRecord = IsRecordIncomplete(input);
                        if (!incompleteRecord)
                        {
                            ++nextRecord;
                            if (nextRecord <= recordIndex)
                                input = "";
                        }
                    }
                }
                if ((line == null) && (incompleteRecord))
                    throw new Exception();

            }
            finally
            {
                Close();
            }
            if (leaveFileOpen == false)
                Close();
            return input;          
        }

        /// <summary>
        /// Tests if the input string is a complete csv record.
        /// </summary>
        /// <param name="input">The string containing the csv that is to be tested.</param>
        /// <returns>True if the record is complete, otherwise returns false.</returns>
        /// <remarks>A csv record is regarded as incomplete if it ends with a partially escaped
        /// csv value (i.e. The value commences with a double quote (") but no matching double quote
        /// occurs before the end of the string).</remarks>
        virtual protected bool IsRecordIncomplete(string input)
        {
            Regex pattern = new Regex("\"{1}[^\"]*\"");
            input = pattern.Replace(input, "csvToken",1);
            Regex pattern1 = new Regex("\"");
            return pattern1.Match(input).Success;
        }

        #region IDisposable Members

        /// <summary>
        /// Implements IDisposable.Dispose() for the <see cref="CSVReader"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
        /// <summary>
        /// Disposes of the resources of the <see cref="CSVReader"/>.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsOpen)
                return;
            if (disposing)
            {
                if (streamReader != null)
                {
                    streamReader.Dispose();
                    streamReader = null;
                    fileStream = null;
                }
                if (fileStream != null)
                {
                    fileStream.Dispose();
                    fileStream = null;
                }
            }

            isOpen = false;
        }

    }
}
