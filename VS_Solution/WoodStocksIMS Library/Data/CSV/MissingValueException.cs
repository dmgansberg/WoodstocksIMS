using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Data.CSV
{
    /// <summary>
    /// The exception that is raised when a value is missing from a <see cref="CSVRecord"/>.
    /// </summary>
    public class MissingValueException: Exception
    {
        /// <summary>
        /// Initializes a <see cref="MissingValueException"/>.
        /// </summary>
        public MissingValueException() : base() { }

        /// <summary>
        /// Initializes a <see cref="MissingValueException"/>.
        /// </summary>
        /// <param name="message">An error message for the exception.</param>
        public MissingValueException(string message) : base(message) { }

        /// <summary>
        ///Initializes a <see cref="MissingValueException"/>. 
        /// </summary>
        /// <param name="message">An error message for the exception.</param>
        /// <param name="innerException">The exception that raised this exception.</param>
        public MissingValueException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        ///Initializes a <see cref="MissingValueException"/>. 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public MissingValueException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
