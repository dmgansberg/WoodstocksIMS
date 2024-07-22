using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    /// <summary>
    /// Contains the event data for a <see cref="IToys.ToyChanged"/>
    /// </summary>
    public class ToyChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The <see cref="IToy"/> that changed.
        /// </summary>
        private IToy toy = null;
        /// <summary>
        /// The field of the toy that changed.
        /// </summary>
        private ToyField field = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toy"></param>
        /// <param name="field"></param>
        public ToyChangedEventArgs(IToy toy, ToyField field)
        {
            this.toy = toy;
            this.field = field;
        }

        /// <summary>
        /// Gets the <see cref="Toy"/>that has changed.
        /// </summary>
        public IToy Toy { get { return toy; } }

        /// <summary>
        /// Gets the field (or property) of the <see cref="Toy"/> that has changed.
        /// </summary>
        public ToyField Field { get { return field; } }
    }

    
}
