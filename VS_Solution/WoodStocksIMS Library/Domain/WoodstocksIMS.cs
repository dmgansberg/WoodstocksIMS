using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Woodstocks.WoodstocksIMS.Domain
{
    using Woodstocks.WoodstocksIMS.Presentation;
    using Woodstocks.WoodstocksIMS.Data;

    /// <summary>
    /// The Wood Stocks Inventory Management System. 
    /// </summary>
    public partial class WoodstocksIMS : IWoodstocksIMS
    {
        /// <summary>
        /// A singleton instance of the <see cref="WoodstocksIMS"/>.
        /// </summary>
        private static IWoodstocksIMS woodstocksIMS = null;
        
        /// <summary>
        /// Represents Wood Stocks toy stock.
        /// </summary>
        IToys woodstocksToys = null;

        /// <summary>
        /// A collection of the toys that have modified data.
        /// </summary>
        IToys modifiedToys = null;

        /// <summary>
        /// The source from which the system should, or has, retrieved toy
        /// stock data.
        /// </summary>
        private string toyDataSource = "";

        /// <summary>
        /// A reference to the importer used by the system for importing toy data.
        /// </summary>
        IWoodstocksToyImporter toyImporter = null;

        /// <summary>
        /// A reference to the importer used by the system for importing toy data.
        /// </summary>
        IWoodstocksToyExporter toyExporter = null;

        /// <summary>
        /// The idle state of the WoodstocksIMS.
        /// </summary>
        WoodstocksIMSState idleState;

        /// <summary>
        /// The importing state of the WoodstocksIMS.
        /// </summary>
        WoodstocksIMSState importingState;

        /// <summary>
        /// The exporting state of the WoodstocksIMS.
        /// </summary>
        WoodstocksIMSState exportingState;       

        /// <summary>
        /// The current state of the WoodstocksIMS.
        /// </summary>
        WoodstocksIMSState currentState;  

        /// <summary>
        /// Creates the <see cref="WoodstocksIMS"/> for the application when 
        /// called if not null and returns a client interface reference to the
        /// system.
        /// </summary>
        /// <returns>A client interface to the <see cref="WoodstocksIMS"/>.</returns>
        public static IWoodstocksIMSClient GetApplication()
        {
            if (woodstocksIMS == null)
                woodstocksIMS = new WoodstocksIMS();
            return woodstocksIMS as IWoodstocksIMSClient;

        }

        //To prevent instantiation other than via the GetApplication method.
        private WoodstocksIMS()
        {
            this.idleState = new IdleState(this);
            this.importingState = new ImportingState(this);
            this.exportingState = new ExportingState(this);
            //The initial starting state of the system is its IdleState.
            this.currentState = idleState;
            this.modifiedToys = new Toys();
        }


        #region IWoodstocksIMSClient implementation

        //Events
        /// <inheritdoc />
        public event AsyncCompletedEventHandler ImportCompleted;

        /// <inheritdoc/>
        public event ProgressChangedEventHandler ProgressChanged;

        /// <inheritdoc />
        public event AsyncCompletedEventHandler ExportCompleted;

        /// <inheritdoc/>
        public void CancelAsync()
        {
            currentState.CancelAsync();
        }

        /// <inheritdoc/>
        public IToys Toys
        {
            get { return currentState.GetToys(); }
        }

        /// <inheritdoc/>
        public void ImportToysAsync(string source)
        {
            currentState.ImportToysAsync(source);
        }

        /// <inheritdoc />
        public void ExportToysAsync()
        {
            currentState.ExportToysAsync();
        }

        /// <inheritdoc/>
        public bool IsBusy()
        {
            return currentState.IsBusy();
        }

        /// <inheritdoc/>
        public string ToyDataSource
        {
            get { return currentState.ToyDataSource; }
            set { currentState.ToyDataSource = value; }
        }

        /// <summary>
        /// Gets whether the <see cref="WoodstocksIMS"/> has data imported that is modified 
        /// and has not been saved
        /// </summary>
        /// <returns></returns>
        public bool UnsavedChanges()
        {
            return currentState.UnsavedChanges();
        }

        /// <inheritdoc />
        public void DiscardImportedToyData()
        {
            currentState.DiscardImportedToyData();
        }

        /// <summary>
        /// The system facing method that is used to discard imported toy data from teh system.
        /// </summary>
        /// <param name="disposing">Indicates if the system should discard the data that is imported. If true, the data is discarded.</param>
        public virtual void DiscardImportedToyData(bool disposing)
        {
            if (disposing)
            {
                this.toyDataSource = "";
                this.woodstocksToys.Clear();
                this.modifiedToys.Clear();
            }
        }

        #endregion

        #region IWoodstocksIMS implementation

        /// <inheritdoc />
        public IToys GetModifiedToys()
        {
            return modifiedToys;
        }

        /// <inheritdoc />
        public void SetModifiedToys(IToys modified)
        {
            modifiedToys = modified;                
        }

        /// <inheritdoc />
        public WoodstocksIMSState GetIdleState()
        {
            return idleState;
        }

        /// <inheritdoc />
        public WoodstocksIMSState GetImportingState()
        {
            return importingState;
        }

        /// <inheritdoc />
        public WoodstocksIMSState GetExportingState()
        {
            return exportingState;
        }

        /// <inheritdoc />
        public void SetState(WoodstocksIMSState newState)
        {
            if ((newState != idleState) && (newState != importingState) && (newState != exportingState))
            {
                throw new Exception("InvalidState");
            }
            this.currentState = newState;
        }

        /// <inheritdoc/>
        public IToys GetToys()
        {
            return new Toys(woodstocksToys);
        }

        /// <inheritdoc/>
        public void SetToys(IToys toys)
        {
            if (woodstocksToys != null)
            {
                woodstocksToys.ToyChanged -= woodstocksToys_ToyChanged; //unsubscribe from changes in the old data.
            }
            woodstocksToys = toys;
            //the change may be to set toys imported to null because no toys are imported, 
            //so check that it hasn't been changed to null before subscribing
            //to event to monitor changes in the collection.
            if (woodstocksToys != null) 
            {
               
                woodstocksToys.ToyChanged += new ToyChangedEventHandler(woodstocksToys_ToyChanged);
            }
        }

        /// <inheritdoc/>
        public string GetToyDataSource() { return toyDataSource; }

        /// <inheritdoc/>
        public void SetToyDataSource(string source)
        {
            if (source == null)
                throw new ArgumentNullException("Source is null");
            toyDataSource = source;
        }

        /// <summary>
        /// Raises the <see cref="ProgressChanged"/> event of the <see cref="WoodstocksIMS"/>.
        /// </summary>
        /// <param name="sender">The <see cref="WoodstocksIMS"/> that raised the event.</param>
        /// <param name="e">The <see cref="ProgressChangedEventArgs"/>data for the event.</param>        
        public void OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressChangedEventHandler handler = ProgressChanged;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ImportCompleted"/> event of the <see cref="WoodstocksIMS"/>.
        /// </summary>
        /// <param name="sender">The <see cref="WoodstocksIMS"/> that raised the event.</param>
        /// <param name="e">The <see cref="AsyncCompletedEventArgs"/> data sent from the 
        /// asynchronous thread that carried out the operation.</param>
        public void OnImportCompleted(object sender, AsyncCompletedEventArgs e)
        {
            AsyncCompletedEventHandler handler = ImportCompleted;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ExportCompleted"/> event of the <see cref="WoodstocksIMS"/>.
        /// </summary>
        /// <param name="sender">The <see cref="WoodstocksIMS"/> that raised the event.</param>
        /// <param name="e">The <see cref="AsyncCompletedEventArgs"/> data sent from the 
        /// asynchronous thread that carried out the operation.</param>
        public void OnExportCompleted(object sender, AsyncCompletedEventArgs e)
        {
            AsyncCompletedEventHandler handler = ExportCompleted;
            if (handler != null)
                handler(this, e);
        }

        /// <inheritdoc/>
        public IWoodstocksToyImporter ToyImporter
        {
            get { return GetToyImporter(); }
            set { SetToyImporter(value); }
        }

        /// <inheritdoc/>
        public virtual IWoodstocksToyImporter GetToyImporter()
        {
            return toyImporter;
        }

        /// <inheritdoc/>
        public void SetToyImporter(IWoodstocksToyImporter toyImporter)
        {
            this.toyImporter = toyImporter;
        }

        /// <inheritdoc/>
        public void SetToyExporter(IWoodstocksToyExporter toyExporter)
        {
            this.toyExporter = toyExporter;
        }

        /// <inheritdoc/>
        public IWoodstocksToyExporter GetToyExporter()
        {
            return toyExporter;
        }

        /// <inheritdoc />
        /*If the modifiedToys collection has more than 0 items in it then the
        system has unsaved modified changes.*/
        public bool GetUnsavedChanges()
        {
            if (modifiedToys == null) return false;
            if (modifiedToys.Count > 0) return true;
            else return false;
        }

        #endregion

        /// <summary>
        /// Handles the event raised by the toys collection when a member of the collection has changed.
        /// </summary>
        /// <param name="sender">The toy collection containing data imported into the system.</param>
        /// <param name="e">Event data passed when a member of the toys collection changes.</param>
        void woodstocksToys_ToyChanged(IToy sender, ToyChangedEventArgs e)
        {
            //only respond to changes to the current count at this time.
            if (e.Field == ToyField.CurrentCount)
            {
                  if (!modifiedToys.Contains(e.Toy))
                    {
                        if (e.Toy.InitialCount != e.Toy.CurrentCount)
                            modifiedToys.Add(e.Toy);
                    }
                    else
                    {   //The item will not need to be exported as it is no longer dirty.
                        //otherwise the item is already on the modified list with its current 
                        //(most recent) count so no other action is taken.
                        if (e.Toy.InitialCount == e.Toy.CurrentCount)
                            modifiedToys.Remove(e.Toy);
                    }
            }              
        }           
    }      //end of WoodstockIMS           
}//end of namespace

    