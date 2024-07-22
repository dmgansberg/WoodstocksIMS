using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    /// <summary>
    /// Exception that is raised when it is detected that a duplicate toy is being added to a <see cref="Toys"/> collection.
    /// </summary>
    public class DuplicateToyException: Exception
    {
        /// <summary>
        /// Initialises a <see cref="DuplicateToyException"/>.
        /// </summary>
        /// <param name="itemCode"></param>
        public DuplicateToyException(string itemCode) : base(String.Format("Toy {0} is a duplicate", itemCode)) { }
    }
}
