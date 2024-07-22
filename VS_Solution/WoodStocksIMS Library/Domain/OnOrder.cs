using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    /// <summary>
    /// Represents the order status of a <see cref="Toy"/>.
    /// </summary>
    public enum OnOrder
    {
        ///<summary>Represents that the Toy is not on order. </summary>      
        No = 1,
        ///<summary>Represents that the Toy is on order. </summary>
        Yes = 2
    }
}
