using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    /// <summary>
    /// Represents a Toy sold by Wood Stocks.
    /// </summary>
    public class Toy: IToy
    {
        /// <summary>The item item code of the<see cref="Toy"/>
        /// </summary>
        private string itemCode = null;

        /// <summary>The description of a <see cref="Toy"/>
        /// </summary>
        private string description = null;

        /// <summary>
        /// The initial count of a <see cref="Toy"/>.
        /// </summary>
        private int initialCount = 0;

        /// <summary>The current count of a <see cref="Toy"/>
        /// </summary>
        private int currentCount = 0;

        /// <summary>The on order status of a<see cref="Toy"/>
        /// </summary>
        private OnOrder onOrder = 0;

        /// <summary>
        /// A copy constructor to initialise a toy.
        /// </summary>
        /// <param name="toy">The existing toy to copy.</param>
        public Toy(IToy toy)
        {
            this.itemCode = toy.ItemCode;
            this.description = toy.ItemDescription;
            this.currentCount = int.Parse(toy.CurrentCount);
            this.onOrder = WoodstocksDataConverter.ToOnOrder(toy.OnOrderStatus);
        }

        /// <summary>
        /// Iniialises a <see cref="Toy"/> that is stocked and sold by Wood Stocks.
        /// </summary>
        /// <param name="itemCode">The item code of the toy.</param>
        /// <param name="itemDescription">A description for the toy.</param>
        /// <param name="currentCount">The amount of the toy that Wood Stocks currently has in stock.</param>
        /// <param name="onOrder">Indicates whether the toy is on order.</param>
        public Toy(string itemCode, string itemDescription, int currentCount,
            OnOrder onOrder)
            : base()
        {
            try
            {
                if (WoodstocksToyValidator.IsValidItemCode(itemCode))
                    this.itemCode = itemCode;
                if (WoodstocksToyValidator.IsValidDescription(itemDescription))
                    this.description = itemDescription;
                //Assumption: All int values of currentCount are valid.
                this.currentCount = currentCount;
                this.initialCount = currentCount;
                if (WoodstocksToyValidator.IsValidOnOrder((int)onOrder))
                    this.onOrder = onOrder;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Initialises a <see cref="Toy"/> that is stocked and sold by Wood Stocks.
        /// </summary>
        /// <param name="itemCode">The item code of the toy.</param>
        /// <param name="itemDescription">A description of the toy.</param>
        /// <param name="currentCount">The current count of items in stock.</param>
        /// <param name="onOrder">Indicates whether the item is on order or not.</param>
        public Toy(string itemCode, string itemDescription, string currentCount, string onOrder)
            : base()
        {
            try
            {
                if (WoodstocksToyValidator.IsValidItemCode(itemCode))
                    this.itemCode = itemCode;
                if (WoodstocksToyValidator.IsValidDescription(itemDescription))
                    this.description = itemDescription;
                if (WoodstocksToyValidator.IsValidCurrentCount(currentCount))
                {
                    this.currentCount = int.Parse(currentCount);
                    this.initialCount = this.currentCount;
                }
                if (WoodstocksToyValidator.IsValidOnOrder(onOrder))
                {
                    if (onOrder.ToUpper() == "YES") 
                    {
                        this.onOrder =  OnOrder.Yes;
                    }
                    else if (onOrder.ToUpper() == "NO")
                    {
                        this.onOrder = OnOrder.No;
                    }
                    else
                    {
                        throw new InvalidOnOrderException();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        #region IToy Members

        /// <inheritdoc />
        public event ToyChangedEventHandler Changed;

        /// <inheritdoc />
        public string ItemCode
        {
            get { return itemCode; }
        }

        /// <inheritdoc />
        public string ItemDescription
        {
            get { return description; }
        }

        /// <inheritdoc />
        public string CurrentCount
        {
            get { return GetCurrentCount(); }
            set { SetCurrentCount(value); }
        }

        /// <inheritdoc />
        public string InitialCount
        {
            get { return initialCount.ToString(); }
        }

        /// <inheritdoc />
        public string OnOrderStatus
        {
            get
            {
                return WoodstocksDataConverter.OnOrderToString(onOrder);
            }
        }

        #endregion

        /// <summary>
        /// Sets the current count for the <see cref="Toy"/>.
        /// </summary>
        /// <param name="count">The current count of the <see cref="Toy"/>.</param>
        /// <remarks>This method throws an <see cref="InvalidCurrentCountException"/>if the value of 
        /// count cannot be converted into a integer value.</remarks>
        protected virtual void SetCurrentCount(string count)
        {
            try
            {
                if (WoodstocksToyValidator.IsValidCurrentCount(count))
                    this.currentCount = int.Parse(count);
                OnChanged(new ToyChangedEventArgs(this,ToyField.CurrentCount));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the current count for the <see cref="Toy"/>.
        /// </summary>
        /// <param name="count">The current count of the <see cref="Toy"/>.</param>
        protected virtual void SetCurrentCount(int count)
        {
            this.currentCount = count;
            OnChanged(new ToyChangedEventArgs(this,ToyField.CurrentCount));
        }

        /// <summary>
        /// Gets the current count of the <see cref="Toy"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetCurrentCount()
        {
            return currentCount.ToString();   
        }

        /// <summary>
        /// Raises the <see cref="IToy.Changed"/> event.
        /// </summary>
        /// <param name="e">An <see cref="ToyChangedEventArgs"/> that contains the event data.</param>
        /// <remarks>The <see cref="ToyChangedEventArgs"/> event data for the event 
        /// contains the initial and current counts for a toy. This enables detection as to whether 
        /// the current count differs from the initial count of a <see cref="Toy"/>.</remarks>
        protected virtual void OnChanged(ToyChangedEventArgs e)
        {
            ToyChangedEventHandler handler = Changed;
            if (handler != null)
                handler(this, e);
        }

        #region IComparable<IToy> Members

        /// <summary>
        /// Compares the current <see cref="IToy"/> with another <see cref="IToy"/>. 
        /// </summary>
        /// <param name="other">Another toy to compare the current toy with</param>
        /// <returns>Returns -1 if the current toy should precede other, 0 if the 
        /// two toys occur in the same order, and 1 if the current toy follows other 
        /// in a sort order</returns>.
        /// <remarks>The comparison of two <see cref="IToys"/> is conducted on the basis
        /// of their item codes. Casing for the comparison is ignored. 
        /// This ensures that an toy with the item code A0001 precedes a toy with a0002, 
        /// for example. It is assumed casing withing an item code is non-significant.
        /// </remarks>
        public int CompareTo(IToy other)
        {
            if (other == null) return 1;
            return this.itemCode.ToUpper().CompareTo(other.ItemCode.ToUpper()); 
                
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// Compares the current instance with another object and returns an integer 
        /// that indicates whether the current instance precedes, follows, or occurs 
        /// in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare the current <see cref="IToy"/> with.</param>
        /// <returns>1 if the current <see cref="IToy"/>follows obj in the sort order.
        /// 0 if the current toy occurs in the same position as obj. -1 if the current
        /// toy precedes obj in the sort order.</returns>
        /// <exception cref="ArgumentException">obj is not an <see cref="IToy"/>.</exception>
        /// <remarks>By definition, any object compares greater than (or follows) null. Thus 
        /// if the other object is null then this method returns 1. This method ensures
        /// results are consistent with the implementation of <see cref="IComparable{IToy}"/>
        /// by attempting to convert other to an <see cref="IToy"/>. If the conversion 
        /// fails an <see cref="ArgumentException"/>is thrown. Otherwise the 
        /// CompareTo method of the generic interface is invoked.</remarks>
       
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            IToy otherToy = obj as IToy;
            if (otherToy != null)
                return this.CompareTo(otherToy);
            else
                throw new ArgumentException("Object is not a IToy");
            
        }

        #endregion

        #region IEquatable<IToy> Members

        /// <summary>
        /// Indicates whether the current <see cref="Toy"/> is equal to another <see cref="Toy"/>.
        /// </summary>
        /// <param name="other">A <see cref="IToy"/>to compare the current <see cref="IToy"/> with</param>
        /// <returns>True if the current <see cref="IToy"/> is equal to the other <see cref="IToy"/>, otherwise false.</returns>
        /// <remarks>The current <see cref="IToy"/>is equal to the other <see cref="IToy"/> if the 
        /// item codes of the two objects are the same. It is assumed that the item code
        /// for an <see cref="IToy"/>should be unique to an <see cref="IToy"/> and if equal
        /// to any other <see cref="IToy"/>then the two objects refer to the same toy sold by 
        /// Wood Stocks.</remarks>
        public bool Equals(IToy other)
        {
            if (other == null)
                return false;
            if (this.itemCode == other.ItemCode)
                return true;
            else
                return false;
        }
                
        /// <summary>
        /// Overrides the <see cref="Object.Equals(object)"/>method to provide results comparable to
        /// the <see cref="IEquatable{IToy}.Equals"/> implementation.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>True if two <see cref="Toy"/> are equal, otherwise false.</returns>
        public override bool Equals(object other)
        {
            if (other == null) //a toy that is null is not equal to toy that isn't.
                return false;
            IToy otherAsToy = other as IToy; 
            if (otherAsToy == null) //if other cannot be converted to an IToy it is not a Toy.
                return false;
            else
                return this.Equals(otherAsToy); //utilise the IEquatable<IToy> implementation.          
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Toy"/>.
        /// </summary>
        /// <returns>A hash code for the <see cref="Toy"/>.</returns>
        /// <remarks>The hash code of the item code for a <see cref="Toy"/>is utilised 
        /// for the hash code of a <see cref="Toy"/>. The item code is immutable, as it cannot
        /// be changed following instantiation of a <see cref="Toy"/>. If the item code
        /// becomes changable then this method would need to be altered because the 
        /// item code hash could no longer be reliably used as a hash code of a Toy.</remarks>
        public override int GetHashCode()
        {
            return this.itemCode.GetHashCode();
        }

        #endregion
    }
}


 

