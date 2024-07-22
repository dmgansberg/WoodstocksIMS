using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Data.CSV
{
    /// <summary>
    /// Represents the header record from a csv file.
    /// </summary>
    public class CSVHeader : CSVRecord
    {
        /// <summary>
        /// Initialises a <see cref="CSVHeader"/>.
        /// </summary>
        public CSVHeader(): base() { }

        /// <summary>
        /// Initialises a <see cref="CSVHeader"/>.
        /// </summary>
        /// <param name="fieldNames">The values of the <see cref="CSVHeader"/>. The values are
        /// the names of the fields for data within the file.</param>
        public CSVHeader(List<string> fieldNames) : base(fieldNames) { }

        /// <summary>
        /// Initialises a <see cref="CSVHeader"/>.
        /// </summary>
        /// <param name="fieldNames">The values of the <see cref="CSVHeader"/>. The values are
        /// the names of the fields for data within the file.</param>
        public CSVHeader(params string[] fieldNames)
            : base(fieldNames) { }

        /// <summary>
        /// Gets the value of a field.
        /// </summary>
        /// <param name="fieldName">The field name for which the value is to be retrieved.</param>
        /// <returns>The value of the field.</returns>
        public int this[string fieldName]
        {
            get 
            { 
                int index = this.IndexOf(fieldName);
                if (index == -1)
                    throw new Exception();
                return index;
            }
        }
          
    }

    

    
}
