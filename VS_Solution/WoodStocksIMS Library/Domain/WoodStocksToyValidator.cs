using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Woodstocks.WoodstocksIMS.Domain
{    
    /// <summary>
    /// A static class that contains methods for performing validation of Wood Stocks toy data.
    /// </summary>
    public static class WoodstocksToyValidator
    {
        /// <summary>
        /// Performs validation of a string value to be used for the item code of a <see cref="Toy"/>.
        /// </summary>
        /// <param name="value">The value that is to be validated.</param>
        /// <returns>True if the value is valid.</returns>
        public static bool IsValidItemCode(string value)
        {
            if (value.Length != 5)
                throw new InvalidItemCodeException();
            Regex pattern = new Regex("\\s*[A-Z]\\d{4}");
            Match match = pattern.Match(value);
            if (!match.Success)
                throw new InvalidItemCodeException();
            return true;
        }

        /// <summary>
        /// Performs validation of a string value to be used for the item description of a <see cref="Toy"/>.
        /// </summary>
        /// <param name="value">The value that is to be validated.</param>
        /// <returns>True if the value is valid.</returns>
        public static bool IsValidDescription(string value)
        {
            if (value.Length >= 255)
                throw new InvalidDescriptionException();
            return true;
        }

        /// <summary>
        /// Performs validation of a string value to be used for the current count of a <see cref="Toy"/>.
        /// </summary>
        /// <param name="value">The value that is to be validated.</param>
        /// <returns>True if the value is valid.</returns>
        public static bool IsValidCurrentCount(string value)
        {
            bool validCurrentCount = true;
            try
            {
                int currentCount = int.Parse(value);
            }
            catch (Exception e)
            {
                Exception invalidCurrentCountException = new InvalidCurrentCountException("The current count is invalid",e);
                validCurrentCount = false;
                throw invalidCurrentCountException;
            }
            return validCurrentCount;
        }

        /// <summary>
        /// Performs validation of a int value to be used for the OnOrder status of a <see cref="Toy"/>.
        /// </summary>
        /// <param name="value">The value that is to be validated.</param>
        /// <returns>True if the value is valid.</returns>
        public static bool IsValidOnOrder(int value)
        {
            switch (value)
            {
                //The value of OnOrder.No
                case 1:
                    return true;
                //the value of OnOrder.Yes
                case 2:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Performs validation of a string value to be used for the OnOrder status of a <see cref="Toy"/>.
        /// </summary>
        /// <param name="value">The value that is to be validated.</param>
        /// <returns>True if the value is valid.</returns>
        public static bool IsValidOnOrder(string value)
        {
            Regex pattern = new Regex("\\s*(NO|YES)\\s*\\z");
            Match match = pattern.Match(value.ToUpper());
            if (!match.Success)
                throw new InvalidOnOrderException();
            return true;
        }
    }
}
