using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Data.CSV
{
    /// <summary>
    /// A base abstract class for CSV Records.
    /// </summary>
    public abstract class CSVRecord : Collection<string>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="CSVRecord"/>.
        /// </summary>
        public CSVRecord() : base() { }

        /// <summary>
        /// Initialises a new instance of the <see cref="CSVRecord"/>.
        /// </summary>
        /// <param name="values">The inital values of the record.</param>
        public CSVRecord(List<string> values) : base(values){ }

        /// <summary>
        /// Initialises a new instance of a <see cref="CSVRecord"/>.
        /// </summary>
        /// <param name="values">The inital values of the record.</param>
        public CSVRecord(params string[] values): base() 
        {
            if (values == null)
                throw new Exception();
            foreach (string value in values)
            {
                this.Items.Add(value);
            }
        }

    }
}
