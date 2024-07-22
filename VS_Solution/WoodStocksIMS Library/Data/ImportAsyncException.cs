using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Data
{
    /// <summary>
    /// An <see cref="ImportException"/> is raised when there is an error during importation.
    /// </summary>
    class ImportException: Exception
    {
        /// <summary>
        /// Initialises an <see cref="ImportException"/>.
        /// </summary>
        public ImportException() { }

        /// <summary>
        /// Initialises an <see cref="ImportException"/>.
        /// </summary>
        public ImportException(string message) : base(message) { }

        /// <summary>
        /// Initialises an <see cref="ImportException"/>.
        /// </summary>
        public ImportException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initialises an <see cref="ImportException"/>.
        /// </summary>
        public ImportException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
