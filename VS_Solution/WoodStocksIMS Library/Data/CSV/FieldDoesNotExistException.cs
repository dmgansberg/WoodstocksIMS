using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Data.CSV
{  
    /// <summary>
    /// The exception that is raised when an attempt is made to access a
    /// named field that does not exist.
    /// </summary>
    public class FieldDoesNotExistException : Exception
    {
        /// <summary>
        /// Initialises a <see cref="FieldDoesNotExistException"/>.
        /// </summary>
        public FieldDoesNotExistException() : base() { }

        /// <summary>
        /// Initialises a <see cref="FieldDoesNotExistException"/>.
        /// </summary>
        public FieldDoesNotExistException(string message) : base(message) { }

        /// <summary>
        /// Initialises a <see cref="FieldDoesNotExistException"/>.
        /// </summary>
        public FieldDoesNotExistException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initialises a <see cref="FieldDoesNotExistException"/>.
        /// </summary>
        public FieldDoesNotExistException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
