using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    /// <summary>
    /// Defines a delegate to handle the <see cref="IToy.Changed"/> event.
    /// </summary>
    /// <param name="sender">The <see cref="IToy"/> that has changed.</param>
    /// <param name="e">An <see cref="ToyChangedEventArgs"/> that contains the event data.</param>
    public delegate void ToyChangedEventHandler(IToy sender, ToyChangedEventArgs e);

    /// <summary>
    /// Defines the interface of a Toy.
    /// </summary>
    public interface IToy : IComparable<IToy>, IComparable, IEquatable<IToy>
    {
        /// <summary>
        /// Event that is raised by a <see cref="IToy"/> when its count has changed.
        /// </summary>
        event ToyChangedEventHandler Changed;

        /// <summary>
        /// Gets the item code for the <see cref="IToy"/>.
        /// </summary>
        string ItemCode { get; }

        /// <summary>
        /// Gets the description of the <see cref="IToy"/>.
        /// </summary>
        string ItemDescription { get; }     

        /// <summary>
        /// Gets the current count for a <see cref="IToy"/>.
        /// </summary>
        string CurrentCount { get; set; }

        /// <summary>
        /// Gets the initial count for a <see cref="IToy"/>.
        /// </summary>
        string InitialCount { get; }

        /// <summary>
        /// Gets the on order status of an <see cref="Toy"/>
        /// </summary>
        string OnOrderStatus { get; }
       
    }
}
