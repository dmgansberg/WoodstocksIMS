using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    /// <summary>
    /// Defines a client interface for the <see cref="WoodstocksIMS"/>.
    /// </summary>
    public interface IWoodstocksIMSClient
    {
        /// <summary>
        /// Raised when an asynchronous import completes.
        /// </summary>
        event AsyncCompletedEventHandler ImportCompleted;

        /// <summary>
        /// Raised when an asynchronous export completes.
        /// </summary>
        event AsyncCompletedEventHandler ExportCompleted;

        /// <summary>
        /// Raised when progress on an asynchronous operation is made.
        /// </summary>
        event ProgressChangedEventHandler ProgressChanged;

        /// <summary>
        /// Cancels an asynchronous operation.
        /// </summary>
        /// 
        void CancelAsync();
        /// <summary>
        /// Retrieves <see cref="IToys"/> which references Wood Stocks toy data 
        /// after importation.
        /// </summary>
        /// <returns></returns>
        IToys Toys { get; }
        
        /// <summary>
        /// Imports toy data into the <see cref="WoodstocksIMS"/> for use by the system.
        /// </summary>
        /// <param name="source">The source from which toy data should be retrieved.</param>
        /// <remarks>The source is the filepath to the csv data file containing the Wood Stocks toy data.</remarks>
        void ImportToysAsync(string source);

        /// <summary>
        /// Exports modified toy data from the system.
        /// </summary>
        void ExportToysAsync();

        /// <summary>
        /// Indicates if the <see cref="WoodstocksIMS"/> is busy carrying out an asynchronous operation.
        /// </summary>
        /// <returns></returns>
        bool IsBusy();

        /// <summary>
        /// Gets and Sets the data source from which toy data should be imported.
        /// </summary>
        string ToyDataSource { get; set; }

        /// <summary>
        /// Gets whether the toy data contains unsaved changes.
        /// </summary>
        /// <returns>True if the toy data contains unsaved changes, false if it does not.</returns>
        bool UnsavedChanges();

        /// <summary>
        /// Discards imported toy data from the system.
        /// </summary>
        void DiscardImportedToyData();
    }
}
