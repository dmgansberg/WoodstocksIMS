using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
        /// <summary>
        /// An abstract class to define state objects for the <see cref="WoodstocksIMS"/>.
        /// </summary>
        /// <remarks>A state object for the <see cref="WoodstocksIMS"/> defines the functionality
        /// of the system according systems state. It is intended that derived classes
        /// of this class define a specific set of functionality for a particular state. For 
        /// example the <see cref="WoodstocksIMS.IdleState"/>defines the systems functionality when the 
        /// system is in an idle state.</remarks>
        public abstract class WoodstocksIMSState : IWoodstocksIMS
        {
            /// <summary>
            /// A reference to the <see cref="WoodstocksIMS"/> so that a defined state can access
            /// functionality of the <see cref="WoodstocksIMS"/>.
            /// </summary>
            private IWoodstocksIMS woodstocksIMS = null;

            /// <summary>
            /// Initialises a <see cref="WoodstocksIMSState"/>.
            /// </summary>
            /// <param name="woodstocksIMS">A reference to the <see cref="WoodstocksIMS"/>.</param>
            public WoodstocksIMSState(IWoodstocksIMS woodstocksIMS)
            {
                if (woodstocksIMS == null)
                    throw new ArgumentNullException();
                this.woodstocksIMS = woodstocksIMS;
            }

            #region IWoodstocksIMS Members

            /// <inheritdoc/>
            public WoodstocksIMSState GetIdleState()
            {
                return woodstocksIMS.GetIdleState();
            }
            /// <inheritdoc/>
            public WoodstocksIMSState GetImportingState()
            {
                return woodstocksIMS.GetImportingState();
            }

            /// <inheritdoc/>
            public WoodstocksIMSState GetExportingState()
            {
                return woodstocksIMS.GetExportingState();
            }

            /// <inheritdoc />           
            public void SetState(WoodstocksIMSState newState)
            {
                woodstocksIMS.SetState(newState);
            }

            /// <inheritdoc /> 
            public virtual void OnProgressChanged(object sender, ProgressChangedEventArgs e)
            {
                throw new InvalidOperationException("Cannot raise on progress event in this state.");
            }

            /// <inheritdoc/>
            public virtual void OnImportCompleted(object sender, AsyncCompletedEventArgs e)
            {
                throw new InvalidOperationException("Cannot raise import completion event in this state.");
            }

            /// <inheritdoc/>
            public virtual void OnExportCompleted(object sender, AsyncCompletedEventArgs e)
            {
                throw new InvalidOperationException("Cannot raise export completion in this state.");
            }

            /// <inheritdoc/>
            public virtual IToys GetModifiedToys()
            {
                throw new InvalidOperationException("Cannot get the modified toys in this state.");
            }

            /// <inheritdoc/>
            public virtual void SetModifiedToys(IToys modified)
            {
                throw new InvalidOperationException("Cannot set the modified toys in this state.");
            }

            /// <inheritdoc/>            
            public IWoodstocksToyExporter GetToyExporter()
            {
                throw new InvalidOperationException("Cannot get the toy exporter in this state.");
            }

            #endregion

            #region IWoodstocksIMSClient Members

            /// <inheritdoc/>
            public event System.ComponentModel.AsyncCompletedEventHandler ImportCompleted;

            /// <inheritdoc/>
            public event System.ComponentModel.ProgressChangedEventHandler ProgressChanged;

            /// <inheritdoc />
            public event System.ComponentModel.AsyncCompletedEventHandler ExportCompleted;

            /// <inheritdoc/>
            ///<remarks>The default implementation is to throw an <see cref="InvalidOperationException"/>.
            ///This is to ensure that the system will only allow states in which a cancellation 
            ///can be performed will occur. If a system state should allow cancellation
            ///then this method shouldl be overriden in the derived state class.</remarks>
            public virtual void CancelAsync()
            {
                throw new InvalidOperationException("Cannot cancel an operation in this state");
            }

            /// <inheritdoc/>
            /// <remarks>The default implementation provided by this method is to 
            /// throw an <see cref="InvalidOperationException"/>. This is to ensure that the
            /// system will only allow importation to be initiated whilst the system is 
            /// in a state that will allow importation. If a particular system state 
            /// should alow importation then this class should be overrided in the 
            /// derived state class.</remarks>
            public virtual void ImportToysAsync(string source)
            {
                throw new InvalidOperationException("Cannot import toys in this state");
            }

            /// <inheritdoc/>
            /// <remarks>The default implementation provided by this method is to 
            /// throw an <see cref="InvalidOperationException"/>. This is to ensure that the
            /// system will only allow exportation to be commenced whilst the system is 
            /// in a state that allows exportation. If a particular system state 
            /// should alow importation then this class should be overrided in the 
            /// derived state class.</remarks>
            public virtual void ExportToysAsync()
            {
                throw new InvalidOperationException("Cannot export toys in this state");
            }

            /// <inheritdoc/>
            /// <remarks>The default implementation provided for the Toys property
            /// is to throw an <see cref="InvalidOperationException"/>.
            /// This is to ensure that toy data can only be accessed in states that allow
            /// the toy data to be accessed. If a particular state should allow for 
            /// the toy data of the system to be accessed then this property should
            /// be overrided in a derived state class.</remarks>
            public virtual IToys Toys 
            {
                get{ return GetToys(); }
            }
           
            /// <inheritdoc/>
            /// <remarks>The default implementation of this property is to 
            /// redirect the get or set opreation to a method that gets or sets the toy 
            /// importer. By default the methods to which the get or set operation 
            /// are directed throw an <see cref="InvalidOperationException"/>. This is
            /// to ensure that by default states do not alter the toy importer in use
            /// by the system. If a particular state should allow the toy importer to be
            /// retrieved or set then the appropriate method should be oveerriden in 
            /// the derived state class that defines the state.</remarks>
            public virtual IWoodstocksToyImporter ToyImporter 
            {
                get { return GetToyImporter(); }
                set { SetToyImporter(value); }
            }

            /// <inheritdoc/>
            /// <remarks>By default this method throws a <see cref="InvalidOperationException"/>. This is
            /// to ensure that by default states cannot access the systems toy importer. If a particular
            /// state should have access to the toy importer then this method should be overriden by the 
            /// derived state class that defines the state to provide the required functionality.</remarks>
            public virtual IWoodstocksToyImporter GetToyImporter()
            {
                throw new InvalidOperationException("Cannot get or set the toy importer in this state.");
            }

            /// <inheritdoc/>
            /// <remarks>By default this method throws a <see cref="InvalidOperationException"/>. This is
            /// to ensure that by default states cannot set toy importer of the system. If a particular
            /// state should set the toy importer then this method should be overriden by the 
            /// derived state class that defines the state to provide the required functionality.</remarks>
            public virtual void SetToyImporter(IWoodstocksToyImporter toyImporter)
            {                
                throw new InvalidOperationException("Cannot get or set the toy importer in this state.");
            }

            /// <inheritdoc/>
            /// <remarks>By default this method throws a <see cref="InvalidOperationException"/>. This is
            /// to ensure that by default states cannot set a toy exporter. If a particular
            /// state should set the toy exporter then this method should be overriden by the 
            /// derived state class that defines the state to provide the required functionality.</remarks>
            public virtual void SetToyExporter(IWoodstocksToyExporter toyExporter)
            {
                throw new InvalidOperationException("Cannot get or set the toy exporter in this state.");
            }

            /// <inheritdoc/>
            /// <remarks>The default implementation of this property redirects 
            /// the get operation to the <see cref="WoodstocksIMSState.GetToyDataSource()"/>
            /// method, whilst the set operation is redirected to the <see cref="WoodstocksIMSState.SetToyDataSource(string)"/>
            /// method</remarks>. Inheritors should note that the default implementations of
            /// the methods to which this property redirects throws an <see cref="InvalidOperationException"/>.
            /// This is to ensure that only states in which the toy data source value should
            /// be capable of being set or retrieved is allowed. If a particular state 
            /// should allow the toy data source to retrieved or set then the derived state
            /// should override the appropriate method which will allow the system to 
            /// get or set the toy data source as required.
            public virtual string ToyDataSource 
            {
                get { return GetToyDataSource(); }
                set { SetToyDataSource(value); }

            }
            
            ///<inheritdoc/>
            ///<remarks>The default implementation of this method throws an 
            ///<see cref="InvalidOperationException"/>. This is to ensure that 
            ///by default states do not allow access to toy data. If a particular
            ///state should allow toy data to be accessed then this method should be
            ///overriden in the derived state class.</remarks>
            public virtual IToys GetToys()
            {
                throw new InvalidOperationException("Cannot get toy data whilst in this state.");
            }

            /// <inheritdoc/>
            /// <remarks>The default implementation of this method causes an <see cref="InvalidOperationException"/>
            /// to be thrown. This is to prevent a state by default from setting the toy data
            /// being used by the system. If a particular state should allow the toy data to be
            /// set then the derived state class for the state should override this method
            /// to provide the required functionality.</remarks>
            public virtual void SetToys(IToys toys)
            {
                throw new InvalidOperationException("Cannot set toy data in this state.");
            }

            /// <inheritdoc />
            /// <remarks>The default implementation throws an <see cref="InvalidOperationException"/>.
            /// This is to prevent a state by default from accessing the toy data source value.
            /// If a particular state should allow the toy data source value to be retrieved then 
            /// the derived state class for the state should override this method to provide the 
            /// required functionality.</remarks>
            public virtual string GetToyDataSource()
            {
                throw new InvalidOperationException("Cannot get the toy data source in this state.");
            }

            /// <inheritdoc/>
            ///<remarks>The default implementation throws an <see cref="InvalidOperationException"/>.
            ///This is to prevent a state by default from being able to set the toy data source.
            ///If a particular state should allow the toy data source to be set then the derived
            ///state class should override this method to provide the required functionality.</remarks>
            public virtual void SetToyDataSource(string source)
            {
                throw new InvalidOperationException("Cannot set the toy data source in this state.");
            }
          

            /// <inhertidoc/>
            /// <remarks>The default implementation of this method is return true to 
            /// indicate that the system is busy whilst carrying out an asynchronous
            /// operation. The method is implemented to return true, because unless the 
            /// system is in its idle state if the system is carrying out an asynchronous
            /// operation then the system will be busy. The default implementation 
            /// is intended to minimise the requirement to override this method when 
            /// by derived state classes that define particular states.</remarks>
            public virtual bool IsBusy()
            {
                return true;
            }

            /// <inheritdoc/>
            /// <remarks>By default calling this method results in an InvalidOperationException.
            /// If a state should allow the retrieval of unsaved changes status, then the state
            /// should override this method and call the GetUnsavedChanges method.</remarks>
            public virtual bool UnsavedChanges()
            {
                throw new InvalidOperationException();
            }

            /// <inheritdoc/>
            /// <remarks>The default implementation returns the result from the system by utilising 
            /// the system implementation.</remarks>
            public virtual bool GetUnsavedChanges()
            {
                return woodstocksIMS.GetUnsavedChanges();
            }

            /// <inheritdoc/>
            /// <remarks>The default implementation throws the InvalidOperationException so as to ensure states that allow
            /// discarding of imported data provide the required implemenation.</remarks>
            public virtual void DiscardImportedToyData()
            {
                throw new InvalidOperationException("Cannot discard toy data whilst in this state");
            }

            /// <inheritdoc/>
            /// <remarks>The default implementation throws the InvalidOperationException so as to ensure states that allow
            /// discarding of imported data provide the required implemenation.</remarks>
            public virtual void DiscardImportedToyData(bool disposing)
            {
                throw new InvalidOperationException("Cannot discard toy data whilst in this state");
            }

            #endregion
        }
    }

