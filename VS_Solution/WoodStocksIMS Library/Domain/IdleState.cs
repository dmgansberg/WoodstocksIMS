using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    using Woodstocks.WoodstocksIMS.Data.CSV;

    /// <summary>
    /// Implementation of the Wood Stocks Inventory Management System.
    /// </summary>
    public partial class WoodstocksIMS : IWoodstocksIMS
    {
        /// <summary>
        /// Represents the idle state of the <see cref="WoodstocksIMS"/>.
        /// </summary>
        private class IdleState : WoodstocksIMSState
        {
            /// <summary>
            /// Initialises a <see cref="WoodstocksIMS.IdleState"/> for use with the <see cref="WoodstocksIMS"/>.
            /// </summary>
            /// <param name="woodstocksIMS"></param>
            //Only requires the initialisation of the reference to the system.
            public IdleState(IWoodstocksIMS woodstocksIMS)  : base(woodstocksIMS){ }
     
            /// <summary>
            /// 
            /// </summary>
            /// <param name="source"></param>
            public override void ImportToysAsync(string source)
            {
                WoodstocksIMSState importing = null;
                try
                {      
                    if (this.GetToyDataSource() == "")
                    {
                        woodstocksIMS.SetToyDataSource(source);
                        woodstocksIMS.ToyImporter = new ToyImporterCSV();
                        importing = woodstocksIMS.GetImportingState();
                        woodstocksIMS.ToyImporter.ImportCompleted += importing.OnImportCompleted;
                        woodstocksIMS.ToyImporter.ImportProgressChanged += importing.OnProgressChanged;
                        woodstocksIMS.ToyImporter.ImportAsync(source);
                        woodstocksIMS.SetState(woodstocksIMS.GetImportingState());
                    }
                    else if (this.GetToyDataSource() != "")
                    {
                         if (woodstocksIMS.UnsavedChanges())
                         {
                                throw new UnsavedDataException();
                         }
                         else 
                         { //the data imported hasn't changed so there is no need to do anything other than
                             //act as if the import went ahead and completed successfully.
                             OnImportCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(null,false,null));
                             return;
                         }
                    }                   
                }
                catch (Exception)
                {
                    if (importing != null)
                    {
                        woodstocksIMS.ToyImporter.ImportCompleted -= importing.OnImportCompleted;
                        woodstocksIMS.ToyImporter.ImportProgressChanged -= importing.OnProgressChanged;
                        woodstocksIMS.SetToyDataSource("");
                    }                    
                    throw;
                }                
            }



            /// <inheritdoc />           
            /// <remarks>The method is overriden because the IdleState allows exportation of 
            /// modified toy data.</remarks>
            public override void ExportToysAsync()
            {
                try
                {
                    IToys modifiedToys = GetModifiedToys();
                    if ((modifiedToys != null) && (modifiedToys.Count > 0))
                    {
                        IWoodstocksToyExporter exporter = new ToyExporterCSV();
                        WoodstocksIMSState exporting = woodstocksIMS.GetExportingState();
                        exporter.ExportCompleted += exporting.OnExportCompleted;
                        exporter.ExportProgressChanged += exporting.OnProgressChanged;                        
                        exporter.ExportAsync(GetToyDataSource(), GetModifiedToys());
                        modifiedToys.Clear();
                        //A copy of the modified toy data was made by 
                        //the exporter, and clearing the list allows the application to continue 
                        //modifying data even though an export is in progress.                       
                        woodstocksIMS.SetToyExporter(exporter);
                        woodstocksIMS.SetState(exporting);
                    }
                    else
                    {
                        throw new InvalidOperationException("Nothing to export");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            /// <inheritdoc />
            /// <remarkts>Clients of the <see cref="WoodstocksIMS"/> should be
            /// able to retrieve imported data for toys if the system is idle.
            /// The data is returned by directed the call to the system GetToy() method
            /// which returns the data.</remarkts>
            public override IToys GetToys()
            {
                return woodstocksIMS.GetToys();
            }
            /// <inheritdoc/>
            /// <remarks>The system is not busy if it is in its idle state.</remarks>
            public override bool IsBusy()
            {
                return false;
            }

            /// <inheritdoc />
            /// <remarks>The idle state should allow retrieval of the unsaved changes status
            /// of the application.</remarks>            
            public override bool UnsavedChanges()
            {
                return GetUnsavedChanges();
            }

            /// <summary>
            /// Gets the toy data that has been modified and has not been saved.
            /// </summary>
            /// <returns>A reference to the toy data that has been modified and not saved.</returns>
            /// <remarks>This method overrideds the default implementation to ensure that the toy
            /// data can be retrieved from the idle state.</remarks>
            public override IToys GetModifiedToys()
            {
                return woodstocksIMS.GetModifiedToys();
            }

            /// <summary>
            /// Gets toy data source.
            /// </summary>
            /// <returns>The toy data source.</returns>
            /// <remarks>This method overrideds the default implementation to ensure that data
            /// source can be retrieved from the idle state which is needed to export successfully.</remarks>
            public override string GetToyDataSource()
            {
                return woodstocksIMS.GetToyDataSource();
            }

            public override void DiscardImportedToyData()
            {
                woodstocksIMS.DiscardImportedToyData(true);
            }

            public override void OnImportCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
            {
                woodstocksIMS.OnImportCompleted(this, e);
            }
        }       
    }
}


