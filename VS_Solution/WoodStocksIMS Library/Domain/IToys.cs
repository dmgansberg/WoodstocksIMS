using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    /// <summary>
    /// Defines an interface for a collection of <see cref="IToy"/>.
    /// </summary>
    public interface IToys: IList<IToy>, IList
    {       
        /// <summary>
        /// Event raised when an item in the collection changes.
        /// </summary>
        event ToyChangedEventHandler ToyChanged;

        /// <summary>
        /// Sorts the collection of <see cref="IToy"/> by item code in the specified sort order..
        /// </summary>
        /// <param name="order">The sort order.</param>
        void SortByItemCode(SortOrder order);

        /// <summary>
        /// Sorts the collection of <see cref="IToy"/> by current count in the specified sort order.
        /// </summary>
        /// <param name="order">The sort order.</param>
        void SortByCurrentCount(SortOrder order);

        /// <summary>
        /// Sorts the collection of <see cref="IToy"/> by on order status in the specified sort order.
        /// </summary>
        /// <param name="order">The sort order.</param>        
        void SortByOnOrder(SortOrder order);

        /// <summary>
        /// The total number of <see cref="IToys"/> in the collection.
        /// </summary>
        new int Count { get; }

        /// <summary>
        /// Gets the <see cref="IToy"/> located at the indexed position specified by index.
        /// </summary>
        /// <param name="index">The zero-based index to the collection for the position of the
        /// <see cref="IToy"/>to be retrieved from the collection.</param>
        /// <returns>The <see cref="IToy"/> </returns>
        new IToy this[int index] { get; }

        /// <summary>
        /// Removes the <see cref="IToy"/> from the collection located at index.
        /// </summary>
        /// <param name="index">The position within the collection of the <see cref="IToy"/>
        /// to be removed from the collection.</param>
        new void RemoveAt(int index);

        /// <summary>
        /// Removes all <see cref="IToy"/> from the collection.
        /// </summary>
        new void Clear();
    }
}
