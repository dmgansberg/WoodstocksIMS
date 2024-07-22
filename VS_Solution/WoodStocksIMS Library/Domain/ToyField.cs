using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    /// <summary>
    /// Defines named constants for the fields of an <see cref="IToy"/>.
    /// </summary>
    public enum ToyField
    {
        /// <summary>
        /// Identifies the <see cref="IToy.ItemCode"/> value.
        /// </summary>
        ItemCode = 1,

        /// <summary>
        /// Identifies the <see cref="IToy.ItemDescription"/> value.
        /// </summary>
        ItemDescription = 2,

        /// <summary>
        /// Identifies the <see cref="IToy.CurrentCount"/> value.
        /// </summary>
        CurrentCount = 3,

        /// <summary>
        /// Identfies the <see cref="IToy.InitialCount"/> value.
        /// </summary>
        InitialCount = 4,

        /// <summary>
        /// Identifies the <see cref="IToy.OnOrderStatus"/> value.
        /// </summary>
        OnOrder = 5
    }
}
