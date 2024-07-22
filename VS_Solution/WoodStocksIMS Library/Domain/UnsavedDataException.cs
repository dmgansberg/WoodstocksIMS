using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    /// <summary>
    /// Exception that occurs when stock data imported into <see cref="WoodstocksIMS"/> 
    /// will be discarded without changes being saved.
    /// </summary>
    public class UnsavedDataException: Exception
    {
       
    }
}
