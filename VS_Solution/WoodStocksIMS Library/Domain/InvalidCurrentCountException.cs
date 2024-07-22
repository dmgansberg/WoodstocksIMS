using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    /// <summary>
    /// Defines an exception that is raised when the value to be used for the
    /// current count of a <see cref="Toy"/> is invalid.
    /// </summary>
    public class InvalidCurrentCountException : Exception
    {
        /// <summary>
        /// Initialises a <see cref="InvalidCurrentCountException"/>.
        /// </summary>
        public InvalidCurrentCountException():base() {}

        /// <summary>
        /// Initialises a <see cref="InvalidCurrentCountException"/>. 
        /// </summary>
        /// <param name="message">A message for the exception.</param>
        public InvalidCurrentCountException(string message): base(message){}

        /// <summary>
        /// Initialises a <see cref="InvalidCurrentCountException"/>.
        /// </summary>
        /// <param name="message">A message for the exception.</param>
        /// <param name="innerException"></param>
        public InvalidCurrentCountException(string message, Exception innerException): base(message, innerException){}
    }
}
