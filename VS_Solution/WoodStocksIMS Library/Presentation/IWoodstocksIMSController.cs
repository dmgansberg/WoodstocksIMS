/*=================================================================
 * 
 * File: IWoodstocksIMSController.cs
 *
 * Description: Defines the IWoodstocksIMSController interface.
 * 
 * (c) Darren Gansberg 2014-2015.
 * 
 * 
 =================================================================*/

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Presentation
{
    using Woodstocks.WoodstocksIMS.Domain;

    /// <summary>
    /// Defines the interface of a controller for a<see cref="WoodstocksIMS"/>.
    /// </summary>
    public interface IWoodstocksIMSController
    {
        /// <summary>
        /// Set the controller's view.
        /// </summary>
        /// <param name="view"></param>
        void SetView(IWoodstocksIMSView view);

        /// <summary>
        /// Raised by the controller when an asynchronous importation has completed. 
        /// </summary>
        event AsyncCompletedEventHandler ImportCompleted;

        /// <summary>
        /// Raised by the controller when an asynchronous exportation has completed.
        /// </summary>
        event AsyncCompletedEventHandler ExportCompleted;

        /// <summary>
        /// Raised by the controller when progress on an asynchronous operation.
        /// </summary>
        event ProgressChangedEventHandler ProgressChanged;

        /// <summary>
        /// Requests the <see cref="WoodstocksIMS"/> to import <see cref="Toy"/> data asynchronously 
        /// into the system.
        /// </summary>
        /// <param name="source">The source from which <see cref="Toy"/> data
        /// should be imported.</param>
        void ImportToysAsync(string source);

        /// <summary>
        /// Informs the <see cref="WoodstocksIMS"/> to cancel an asynchronous operation.
        /// </summary>
        void CancelAsync();

        /// <summary>
        /// Gets whether the <see cref="IWoodstocksIMS"/> has imported data that has not been saved.
        /// </summary>
        /// <returns>True if the system has modified data that has not been saved, otherwise false.</returns>
        bool UnsavedChanges();

        /// <summary>
        /// Requests the <see cref="WoodstocksIMS"/> to export <see cref="Toy"/> data asynchronously
        /// into the system.
        /// </summary>
        void ExportToysAsync();

        //UNDOCUMENTED

        /// <summary>
        /// Gets the toy data from the WoodstocksIMS.
        /// </summary>
        /// <returns>The imported toy data.</returns>
        IToys GetToys();

        /// <summary>
        /// Resets the error status reported by the controller
        /// </summary>       
        void ResetErrorStatus();

        /// <summary>
        /// Gets the error reported by the WoodstocksIMS whne an error occurs.
        /// </summary>
        /// <returns>The <see cref="Exception"/>that reports the error.</returns>
        Exception GetErrorStatus();

        /// <summary>
        /// Resets the cancellation status of the cotnroller.
        /// </summary>
        void ResetCancellationStatus();

        /// <summary>
        /// Gets the cancellation status of an operation.
        /// </summary>
        /// <returns>True if an operation has been cancelled, otherwise false.</returns>
        bool GetCancellationStatus();


        /// <summary>
        /// Causes the <see cref="WoodstocksIMS"/> to discard toy data that has been imported into the system.
        /// </summary>
        void DiscardImportedToyData();

    }
}
