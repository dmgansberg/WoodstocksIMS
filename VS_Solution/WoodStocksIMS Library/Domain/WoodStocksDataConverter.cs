using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    /// <summary>
    /// Contains data conversion methods.
    /// </summary>
    public static class WoodstocksDataConverter
    {
        /// <summary>
        /// Converts a value string into a OnOrder value.
        /// </summary>
        /// <param name="value">The value that is to be converted to an OnOrder value.</param>
        /// <returns>The corresponding <see cref="OnOrder"/> value of the string if 
        /// the value can be converted successfully. Throws an InvalidCastException
        /// if the value cannot be converted.</returns>
        public static OnOrder ToOnOrder(string value)
        {           
            value = value.ToUpper();
            if (value == "YES")
                return OnOrder.Yes;
            if (value == "NO")
                return OnOrder.No;
            throw new InvalidCastException("Could not cast value to OnOrder");
        }

        /// <summary>
        /// Converts an OnOrder value to a string value.
        /// </summary>
        /// <param name="onOrder">The <see cref="OnOrder"/>value that is to be converted to an 
        /// string value.</param>
        /// <returns>"Yes" if the OnOrder value is <see cref="OnOrder.Yes"/>, "No"
        /// if the OnOrder value is<see cref="OnOrder.No"/></returns>
        /// <remarks>This method throws an <see cref="ArgumentException"/> if the 
        /// argument passed to the method is not a value of <see cref="OnOrder"/>.</remarks>
        public static string OnOrderToString(OnOrder onOrder)
        {
            if (onOrder == OnOrder.No)
            {
                return "No";
            }

            if (onOrder == OnOrder.Yes)
            {
                return "Yes";
            }
            throw new ArgumentException("Could not convert to OnOrder");
        }
    }
}
