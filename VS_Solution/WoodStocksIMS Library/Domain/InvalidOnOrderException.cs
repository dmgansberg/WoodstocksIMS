using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    /// <summary>
    /// Defines an exception to be raised when an <see cref="OnOrder"/> value is invalid.
    /// </summary>
    public class InvalidOnOrderException: Exception
    {
        /// <summary>
        /// Initialises an <see cref="InvalidOnOrderException"/>.
        /// </summary>
        public InvalidOnOrderException() : base("Invalid value for OnOrder") { }
    }
}
