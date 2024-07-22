using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    /// <summary>
    /// Defines a delegate to handle the <see cref="IWoodstocksToyImporter.ImportProgressChanged"/> event.
    /// </summary>
    /// <param name="sender">The <see cref="IWoodstocksToyImporter"/> that raised the event.</param>
    /// <param name="e">The event data for the <see cref="IWoodstocksToyImporter.ImportProgressChanged"/>event.</param>
    public delegate void ImportProgressChanged(IWoodstocksToyImporter sender, ProgressChangedEventArgs e);

    /// <summary>
    /// Defines a delegate to handle the <see cref="IWoodstocksToyImporter.ImportCompleted"/> event.
    /// </summary>
    /// <param name="sender">The <see cref="IWoodstocksToyImporter"/> that raised the event.</param>
    /// <param name="e">The event data for the <see cref="IWoodstocksToyImporter.ImportCompleted"/>event.</param>
    public delegate void ImportCompletedHandler(IWoodstocksToyImporter sender, AsyncCompletedEventArgs e);

    /// <summary>
    /// Defines an interface to import toy data into the <see cref="WoodstocksIMS"/>.
    /// </summary>
    public interface IWoodstocksToyImporter: IDisposable
    {
        
        /// <summary>
        /// Raised upon completion of an asynchronous operation. 
        /// </summary>
        /// <remarks>The <see cref="AsyncCompletedEventArgs.Error"/>
        /// should be checked to ensure that an error did not occur during
        /// the operation. If no error has occurred during the operation
        /// the result of the import operation can be retrieved by calling
        /// the <see cref="GetToys()"/>method.</remarks>
        event ImportCompletedHandler ImportCompleted;

        /// <summary>
        /// The event when progress is made on an asynchronous import.
        /// </summary>
        event ImportProgressChanged ImportProgressChanged;

        /// <summary>
        /// Imports toy data asynchronously.
        /// </summary>
        /// <param name="source">The source from which data should be imported.</param>
        void ImportAsync(string source);

        /// <summary>
        /// Cancels an asynchronous import.
        /// </summary>
        void ImportCancel();

        /// <summary>
        /// Returns whether the importer is busy wilst carrying out an asynhronous import.
        /// </summary>
        /// <returns>True if the importer is busy carrying out an 
        /// asynchronous import. False if the importer is not busy.</returns>
        bool IsBusy();

        /// <summary>
        /// Gets the data for toys that are imported by the importer.
        /// </summary>
        /// <returns>The toy data that was imported.</returns>
        IToys GetToys();

        /// <summary>
        /// Closes the importer.
        /// </summary>
        void Close();
        
        
    
    }
}
