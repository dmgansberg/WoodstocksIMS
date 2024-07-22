using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{   
    using Woodstocks.WoodstocksIMS.Data;
    using Woodstocks.WoodstocksIMS.Presentation;

    /// <summary>
    /// Defines an delegate to handle the ImportToysCompleted event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ImportToysCompletedEventHandler(object sender, AsyncCompletedEventArgs e);
 
    /// <summary>
    /// Defines an interface to the <see cref="WoodstocksIMS"/>.
    /// </summary>
    public interface IWoodstocksIMS: IWoodstocksIMSClient
    {        
       /// <summary>
       /// Get the Idle state of the system.
       /// </summary>
       /// <returns>The idle state of the system.</returns>
       WoodstocksIMSState GetIdleState();

        /// <summary>
        /// Get the Importing state of the system.
        /// </summary>
        /// <returns>The Importing state of the system</returns>
       WoodstocksIMSState GetImportingState();
        /// <summary>
        /// Get the Exporting state of the system
        /// </summary>
        /// <returns></returns>
       WoodstocksIMSState GetExportingState();

        /// <summary>
        /// Set the current state of the system.
        /// </summary>
        /// <param name="newState"></param>
       void SetState(WoodstocksIMSState newState);
       
        /// <summary>
        /// Gets and Sets the <see cref="IWoodstocksToyImporter"/> to be
        /// used by the system to import toy data.
        /// </summary>        
        /// <remarks>This property is intended as a system property that 
        /// that is unavailable to clients of the system. As a result it
        /// does not appear on the client interface.</remarks>
       IWoodstocksToyImporter ToyImporter { get; set; }

        /// <summary>
        /// Gets the toy importer of the system.
        /// </summary>
        /// <returns>The systems toy importer.</returns>
        ///<remarks>This method is to be implemented to provide for an implementation
        ///of the ToyImporter property.</remarks>
       IWoodstocksToyImporter GetToyImporter();

       /// <summary>
       /// Sets the toy importer of the system.
       /// </summary>
       /// <param name="toyImporter">The importer that the system should be set to use.</param>
       ///<remarks>This method is to be implemented to provide for an implementation
       ///of the ToyImporter property.</remarks>
       void SetToyImporter(IWoodstocksToyImporter toyImporter);

       /// <summary>
       /// Sets the toy exporter used for exporting data.
       /// </summary>
       /// <param name="toyExporter">The exporter that the system should use to export toy data.</param>
       void SetToyExporter(IWoodstocksToyExporter toyExporter);

       /// <summary>
       /// Gets the toy exporter used for exporting data.
       /// </summary>
       IWoodstocksToyExporter GetToyExporter();

       /// <summary>
       /// Gets the toy data currently imported into the <see cref="WoodstocksIMS"/>.
       /// </summary>
       /// <returns>A reference to the imported toy data</returns>.
       /// <remarks>This method is intended as a system method and therefore does not
       /// appear on the client interface.</remarks>
       IToys GetToys();

       /// <summary>
       /// Gets the toy data that has been modified and has not been saved.
       /// </summary>
       /// <returns>A reference to the modified toy data.</returns>.
       /// <remarks>This method is intended as a system method and therefore does not
       /// appear on the client interface.</remarks>
       IToys GetModifiedToys();

       /// <summary>
       /// Sets the toy data that has been modified and has not been saved.
       /// </summary>
       /// <param name="modified">The modified toy data.</param>
       /// <returns>A reference to the modified toy data.</returns>.
       /// <remarks>This method is intended as a system method and therefore does not
       /// appear on the client interface.</remarks>
       void SetModifiedToys(IToys modified);

        /// <summary>
        /// Sets the toy data in use by the <see cref="WoodstocksIMS"/>.
        /// </summary>
        /// <param name="toys">The toy data to be used by the system.</param>
       void SetToys(IToys toys);

        /// <summary>
        /// Gets the source from which the system will, or has, imported toy data.
        /// </summary>
        /// <returns>The source of the toy data.</returns>
       string GetToyDataSource();

        /// <summary>
        /// Sets the source from which the system will, or has, imported toy data.
        /// </summary>
        /// <param name="source">The source of the toy data.</param>
        /// <remarks>This method is intended as a system method and therefore
        /// does not appear on the client interface.</remarks>
       void SetToyDataSource(string source);

       /// <summary>
       /// Raises the <see cref="IWoodstocksIMSClient.ProgressChanged"/> event of the <see cref="IWoodstocksIMS"/>
       /// to indicate that progress of an asynchronous operation.
       /// </summary>
       /// <param name="sender">The <see cref="IWoodstocksIMS"/>that raised the event.</param>
       /// <param name="e">The event data for the <see cref="IWoodstocksIMSClient.ProgressChanged"/>event.</param>
       void OnProgressChanged(object sender, ProgressChangedEventArgs e);

        /// <summary>
        /// Raises the <see cref="IWoodstocksIMSClient.ImportCompleted"/> event of the <see cref="IWoodstocksIMS"/>.
        /// </summary>
        /// <param name="sender">The <see cref="IWoodstocksIMS"/>that raised the event.</param>
        /// <param name="e">The event data for the <see cref="IWoodstocksIMSClient.ImportCompleted"/>event.</param>
       void OnImportCompleted(object sender, AsyncCompletedEventArgs e);

       /// <summary>
       /// Raises the <see cref="IWoodstocksIMSClient.ExportCompleted"/> event of the <see cref="IWoodstocksIMS"/>.
       /// </summary>
       /// <param name="sender">The <see cref="IWoodstocksIMS"/>that raised the event.</param>
       /// <param name="e">The event data for the <see cref="IWoodstocksIMSClient.ExportCompleted"/>event.</param>
       void OnExportCompleted(object sender, AsyncCompletedEventArgs e);

       /// <summary>
       /// Gets whether the <see cref="IWoodstocksIMS"/> has imported stock data that has been 
       /// modified but has not been saved. 
       /// </summary>
       /// <returns></returns>
       /// <remarks>This method is to be implemented for "internal system" use. It exists, primarily,
       /// to allow state objects to return the result from the system to clients who have 
       /// called the UnsavedChanges method on the client interface.</remarks>
       bool GetUnsavedChanges();

        /// <summary>
        /// Discards imported data from the system. This method is defined as a system facing interface method.
        /// </summary>
        /// <param name="disposing">Indicates that the data should actually be disposed of.</param>
       void DiscardImportedToyData(bool disposing);
    }
}
