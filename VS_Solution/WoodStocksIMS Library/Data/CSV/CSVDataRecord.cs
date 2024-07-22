using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Data.CSV
{
    /// <summary>
    /// Represents a csv data record.
    /// </summary>
    public class CSVDataRecord: CSVRecord
    {
        /// <summary>
        /// A csv header for the record.
        /// </summary>
        private CSVHeader header = null;

        /// <summary>
        /// Initialises a <see cref="CSVDataRecord"/>.
        /// </summary>
        public CSVDataRecord() : base() { }

        /// <summary>
        /// Initialises a <see cref="CSVDataRecord"/>.
        /// </summary>
        /// <param name="values">The values of the record.</param>
        public CSVDataRecord(List<string> values) : base(values) { }

        /// <summary>
        /// Initialises a <see cref="CSVDataRecord"/>.
        /// </summary>
        /// <param name="header">A <see cref="CSVHeader"/>that defines the fields for the record.</param>
        /// <param name="values">The values of the record.</param>
        public CSVDataRecord(CSVHeader header, List<string> values) : base()
        {           
            if (header != null)
            {
                if (values.Count != header.Count)
                    throw new Exception();
            }
            this.header = header;
            foreach (string value in values)
            {
                base.Items.Add(value); 
            }
        }

        /// <summary>
        /// Initialises a <see cref="CSVDataRecord"/>.
        /// </summary>
        /// <param name="header">A header, containing the names of fields, for the record.</param>
        public CSVDataRecord(CSVHeader header)
        {
            if (header == null)
                throw new ArgumentNullException("Header cannot be null");
            this.header = header;
            for (int i = 0; i < header.Count; i++)
            {
                this.Items.Add("");
            }
        }

        /// <summary>
        /// Gets a value for a named field of the record.
        /// </summary>
        /// <param name="fieldName">The name of the field of the record for which the value should be returned.</param>
        /// <returns>The value of the named field.</returns>
        /// <remarks>A <see cref="FieldNameArgumentIsNullException"/> is thrown if a null value or an empty string is passed as
        /// the argument of the fieldName parameter.
        /// A <see cref="FieldDoesNotExistException"/> is thrown if an attempt is made to read a
        /// the value of a field and the field name for the record does not exist.
        /// 
        /// </remarks>
        public string this[string fieldName]
        {
            get 
            {
                if ((fieldName == null) || (fieldName == ""))
                    throw new FieldNameArgumentIsNullException("Field name is null or empty");
                if (header == null) 
                    throw new FieldDoesNotExistException();
                int index = header.IndexOf(fieldName);
                if (index == -1)
                    throw new FieldDoesNotExistException();
                return this.Items[index];
            }
            set 
            {
                if ((fieldName == null) || (fieldName == ""))
                    throw new FieldNameArgumentIsNullException("Field name is null or empty");
                if (header == null)
                    throw new FieldDoesNotExistException();
                int index = header.IndexOf(fieldName);
                if (index == -1)
                    throw new FieldDoesNotExistException();
                base.Items[index] = value;
            }
        }

    }
}
