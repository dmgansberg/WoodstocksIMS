using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    /// <summary>
    /// A collection of Toys.
    /// </summary>
    public class Toys : IToys
    {
        private List<IToy> toyItems = null;

        /// <summary>
        /// Initialises a <see cref="Toys"/>collection.
        /// </summary>
        public Toys()
        {
            toyItems = new List<IToy>();
        }

        /// <summary>
        /// Initialise a <see cref="Toys"/> collection.
        /// </summary>
        /// <param name="toys"></param>
        //register to the changed event of all the toys so the event is caught and 
        //rebroadcasted
        public Toys(IToys toys)
        {
            toyItems = new List<IToy>(toys);
            foreach (IToy toy in toyItems)
            {
                toy.Changed += ItemChanged;
            }
        }

        /// <summary>
        /// Handles the<see cref="IToy.Changed"/> event of a toy in the collection
        /// when it is raised.
        /// </summary>
        /// <param name="sender">The toy for which the <see cref="IToy.Changed"/> event
        /// has been raised.</param>
        /// <param name="e"></param>
        protected virtual void ItemChanged(IToy sender, ToyChangedEventArgs e)
        {
            OnToyChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="IToys.ToyChanged"/> event of the collection when a 
        /// <see cref="IToy"/> within the collection changes.
        /// </summary>        
        /// <param name="e">Event data that indicates the the <see cref="IToy"/>that changed 
        /// and the nature of the change.</param>
        protected virtual void OnToyChanged(ToyChangedEventArgs e)
        {
            ToyChangedEventHandler handler = ToyChanged;
            if (handler != null)
                handler(e.Toy, e);
        }

        #region IToys Members

        /// <inheritdoc/>
        public event ToyChangedEventHandler ToyChanged;

        /// <inheritdoc/>
        public void SortByItemCode(SortOrder order)
        {
            if (order == SortOrder.Ascending)
                toyItems.Sort();
            else if (order == SortOrder.Descending)
            {
                toyItems.Sort();
                toyItems.Reverse();
            }
            else
            {
                throw new ArgumentException("Invalid sort order");
            }

        }

        /// <inheritdoc/>
        public void SortByCurrentCount(SortOrder order)
        {
            if (order == SortOrder.Ascending)
            {
                SortByCurrentCountAscending();

            }
            else if (order == SortOrder.Descending)
            {
                SortByCurrentCountDescending();
            }
            else
            {
                throw new ArgumentException("Invalid sort order");
            }
        }

        /// <summary>
        /// Sorts the collection, in ascending order, by the Current Count for a <see cref="IToy"/>.
        /// </summary>
        protected virtual void SortByCurrentCountAscending()
        {
            toyItems.Sort(delegate(IToy toy1, IToy toy2)
            {
                int result = (int.Parse(toy1.CurrentCount)).CompareTo(int.Parse(toy2.CurrentCount));
                if (result != 0)
                    return result;
                return toy1.ItemCode.ToUpper().CompareTo(toy2.ItemCode.ToUpper());

            });
        }

        /// <summary>
        /// The implemenation to sort a collection of <see cref="Toys"/> by the current count of the
        /// toy in descending order.
        /// </summary>
        
        /* The implementation ensures that the toys in the collection are sorted in descending order
         * of current count by comparing the current count of toy1 with that of toy2
         * and returning the opposite result. The opposite result is returned to 
         * ensure that if toy1, would if sorted in ascending order, precedes toy2, it will follow
         * toy2 in the resulting. In the event that two toys are the same then the toys are compared
         * by item code to ensure that the toys appear in item code, ascending order, if the count
         * of two toys are equal.*/
        protected virtual void SortByCurrentCountDescending()
        {
            toyItems.Sort(delegate(IToy toy1, IToy toy2)
            {
                int result = (int.Parse(toy1.CurrentCount)).CompareTo(int.Parse(toy2.CurrentCount));
                if (result < 0) return 1;
                else if (result > 0) return -1;
                else // == 0
                {

                    return toy1.ItemCode.ToUpper().CompareTo(toy2.ItemCode.ToUpper());
                }
            });
        }
                   
        /// <inheritdoc/>
        public void SortByOnOrder(SortOrder order)
        {
            if (order == SortOrder.Ascending)
            {
                SortByOnOrderAscending();
            }
            else
            {
                SortByOnOrderDescending();
            }
        }

        /// <summary>
        /// The implemenation to sort a collection of <see cref="Toys"/> by on order status 
        /// in ascending order.
        /// </summary>
        
        /* The implementation passes a method to the Sort method which it invokes to sort the 
        * underlying collection. The method passed compares the OnOrder status of two toys and 
         * returns the result of the comparison, utilising the CompareTo
        * method for the OnOrderStatus. If the result of the comparison is 0 indicating that the 
         order status of the two toys are equal then the toys are compared by item code to ensure
         that toys are sorted by ascending order of item code when the order status is the same.*/
        protected void SortByOnOrderAscending()
        {
            toyItems.Sort(delegate(IToy toy1, IToy toy2)
            {
                int result = toy1.OnOrderStatus.CompareTo(toy2.OnOrderStatus);
                if (result != 0)
                    return result;
                return toy1.ItemCode.ToUpper().CompareTo(toy2.ItemCode.ToUpper());

            });
        }

        /// <summary>
        /// The implemenation to sort a collection of <see cref="Toys"/> by on order status 
        /// in descending order.
        /// </summary>

        /* The implementation passes a method to the Sort method of the underlying collection
         * which it invokes to sort the collection. The method passed compares the OnOrder status 
         * of two toys and returns the opposite result of the comparison. thus utilising the CompareTo
       * method for the OnOrderStatus. The opposite result is returned to ensure that if toy1, 
         * would if sorted in ascending order, precedes toy2, it will follow toy2 in the resulting. 
         * If the result of the comparison is 0 indicating that the
         * order status of the two toys are equal then the toys are compared by item code to ensure
         * that toys are sorted by ascending order of item code when the order status is the same.*/
        protected void SortByOnOrderDescending()
        {
            toyItems.Sort(delegate(IToy toy1, IToy toy2)
            {
                int result = toy1.OnOrderStatus.CompareTo(toy2.OnOrderStatus);
                if (result < 0) return 1;
                else if (result > 0) return -1;
                else // == 0
                {

                    return toy1.ItemCode.ToUpper().CompareTo(toy2.ItemCode.ToUpper());

                }

            });
        }

       
        #endregion

        #region IList<IToy> Members

        ///<inheritdoc/>
        public int IndexOf(IToy item)
        {
            return toyItems.IndexOf(item);
        }

        ///<inheritdoc/>
        public void Insert(int index, IToy item)
        {
            toyItems[index].Changed -= ItemChanged;
            item.Changed += ItemChanged;
            toyItems.Insert(index, item);
        }

        ///<inheritdoc/>
        public void RemoveAt(int index)
        {
            toyItems[index].Changed -= ItemChanged;
            toyItems.RemoveAt(index);
        }

        ///<inheritdoc/>
        public IToy this[int index]
        {
            get
            {
                return toyItems[index];
            }
            set
            {
                toyItems[index] = value;
                value.Changed += ItemChanged;
            }
        }

        #endregion

        #region ICollection<IToy> Members

        ///<inheritdoc/>
        public void Add(IToy item)
        {
            //detect any attempt to add a toy with the same item code and raise an exception if it does.
            if (toyItems.Contains(item))
            {
               throw new DuplicateToyException(item.ItemCode);
            }
            toyItems.Add(item);
            item.Changed += new ToyChangedEventHandler(ItemChanged);
        }             

        ///<inheritdoc/>
        public void Clear()
        {
            //unsubscribe from changes in toys that were in the collection, otherwise 
            //events may be received from toys that are no longer in the collection.
            foreach (IToy toy in toyItems)
            {
                toy.Changed -= ItemChanged;
            }
            toyItems.Clear();
        }

        ///<inheritdoc/>
        public bool Contains(IToy item)
        {
            return toyItems.Contains(item);
        }

        ///<inheritdoc/>
        public void CopyTo(IToy[] array, int arrayIndex)
        {
            toyItems.CopyTo(array, arrayIndex);
        }

        ///<inheritdoc/>
        public int Count
        {
            get { return toyItems.Count; }
        }
       

        ///<inheritdoc/>
        public bool IsReadOnly
        {
            get { return false; }
        }
        ///<inheritdoc/>
        public bool Remove(IToy item)
        {
            item.Changed -= ItemChanged;
            return toyItems.Remove(item);
        }

        #endregion

        #region IEnumerable<IToy> Members

        ///<inheritdoc/>
        public IEnumerator<IToy> GetEnumerator()
        {
            return toyItems.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (IEnumerator) this.GetEnumerator();
            //return toyItems.GetEnumerator() as IEnumerator<IToys>;
        }

        #endregion

        //Code for the following two methods amended as necessary for the type
        //from https://msdn.microsoft.com/en-us/library/ms131190(v=vs.110).aspx

        //public static bool operator ==(IToy toy1, IToy toy2)
        //{
        //    if ((object)toy1 == null || ((object)toy2) == null)
        //        return Object.Equals(toy1, toy2);

        //    return toy1.Equals(toy2);
        //}

        //public static bool operator !=(IToy toy1, IToy toy2)
        //{
        //    if (toy1 == null || toy2 == null)
        //        return !Object.Equals(toy1, toy2);

        //    return !(toy1.Equals(toy2));
        //}

        #region IList Members

        /// <inheritdoc />
        public int Add(object value)
        {
            IToy obj = value as IToy;
            if (obj == null) return -1;
            this.Add(obj);
            return (toyItems.Count - 1);
        }

        /// <inheritdoc />
        public bool Contains(object value)
        {
            IToy obj = value as IToy;
            if (obj == null) return false;
            return this.Contains(obj);
        }

        /// <inheritdoc />
        public int IndexOf(object value)
        {
            IToy obj = value as IToy;
            if (obj == null) return -1;
            return this.IndexOf(obj);
           
        }

        /// <inheritdoc />
        public void Insert(int index, object value)
        {
            IToy obj = value as IToy;
            if (obj == null) throw new ArgumentException("Not an IToy");
            this.Insert(index, obj);
        }

        /// <inheritdoc />
        public bool IsFixedSize
        {
            get { return false; }
        }

        /// <inheritdoc />
        public void Remove(object value)
        {
            IToy obj = value as IToy;
            if (obj == null) return;
            this.Remove(obj);
        }

        object IList.this[int index]
        {
            get
            {
                return toyItems[index];
            }
            set
            {
                IToy obj = value as IToy;
                if (obj == null) throw new ArgumentException("Not an IToy");
                toyItems[index] = obj;
            }
        }

        #endregion

        #region ICollection Members

        /// <inheritdoc />
        /// <remarks>This method is not implemented because it is not currently needed in the solution.</remarks>
        public void CopyTo(Array array, int index)
        {
            //if (index < 0) throw new ArgumentOutOfRangeException()
            //if (array.GetLength <
            //array.GetLength
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool IsSynchronized
        {
            get { return false; }
        }

        /// <inheritdoc />
        /// <remarks>This method is not implemented because it is not currently needed in the solution.</remarks>
        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion


    }

}


