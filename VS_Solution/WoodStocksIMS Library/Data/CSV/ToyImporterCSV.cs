using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Woodstocks.WoodstocksIMS.Data.CSV
{
    using Woodstocks.WoodstocksIMS.Domain;    

    /// <summary>
    /// An importer to import toy data from a csv data file.
    /// </summary>
    public class ToyImporterCSV : IWoodstocksToyImporter
    {
        /// <summary>
        /// A <see cref="System.ComponentModel.BackgroundWorker"/> to carry out an asynchronous importation.
        /// </summary>
        private BackgroundWorker backgroundWorker = null;

        /// <summary>
        /// Toy data imported by the importer.
        /// </summary>
        private IToys importedData = null;

        /// <summary>
        /// Initialises a <see cref="ToyImporterCSV"/>.
        /// </summary>
        public ToyImporterCSV() { }

        #region IWoodstocksToyImporter Members

        /// <inheritdoc/>
        public event ImportCompletedHandler ImportCompleted;

        /// <inheritdoc/>
        public event ImportProgressChanged ImportProgressChanged;

        /// <inheritdoc/>
        public void ImportAsync(string source)
        {
            if ((backgroundWorker != null) && (backgroundWorker.IsBusy == true))
            {
                throw new InvalidOperationException("Importer is busy", new ImportException());
            }
            if (backgroundWorker == null)
            {
                backgroundWorker = new BackgroundWorker();
                backgroundWorker.DoWork += new DoWorkEventHandler(DoImport);
                backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompleteImport);
                backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(UpdateProgress);
                backgroundWorker.WorkerSupportsCancellation = true;
                backgroundWorker.WorkerReportsProgress = true;
            }
            backgroundWorker.RunWorkerAsync(source);
        }

        /// <inheritdoc/>
        public void ImportCancel()
        {
            //cancel the backgroundWorker if it still exists 
            if (backgroundWorker != null)
            {
                backgroundWorker.CancelAsync();
            }
            else //checks whether imported data exists after the worker has finished. 
            {
                if (importedData != null)
                {
                    importedData = null;
                    ImportCompletedHandler handler = ImportCompleted;
                    if (handler != null)
                        handler(this, new AsyncCompletedEventArgs(null, true, null));
                }
            }
        }

        /// <inheritdoc/>
        public bool IsBusy()
        {
            if (backgroundWorker == null) //if there is no BackgroundbackgroundWorker it cannot be busy.
                return false;
            return backgroundWorker.IsBusy;
        }

        /// <summary>
        /// Retrieves the imported data from the <see cref="ToyImporterCSV"/>.
        /// </summary>
        /// <returns>An interface reference to the toy data imported, or null
        /// if no data was imported. The returned value should be checked for
        /// null before attempting to use the data.</returns>
        public IToys GetToys()
        {
            return importedData;
        }

        /// <summary>
        /// Handles the ProgressChanged event raised by the BackgroundbackgroundWorker carrying out an
        /// asynchronous import.
        /// </summary>
        /// <param name="sender">The BackgroundbackgroundWorker that raised the event.</param>
        /// <param name="e">A <see cref="ProgressChangedEventArgs"/> containing the progress change data.</param>
        ///<remarks>Handles the backgroundWorkers progress by raising the ImportProgressChanged event of the <see cref="ToyImporterCSV"/>.</remarks>
        protected virtual void UpdateProgress(object sender, ProgressChangedEventArgs e)
        {
            OnImportProgressChanged(e);
        }

        /// <summary>
        /// Handles the RunbackgroundWorkerCompleted event of the BackgroundbackgroundWorker performing an 
        /// asynchronous import.
        /// </summary>
        /// <param name="sender">The BackgroundbackgroundWorker that raised the evvent.</param>
        /// <param name="e">The event data sent by the BackgroundWorker.</param>
        /// <remarks>The event is handled by raising the ImportCompleted event of the 
        /// <see cref="ToyImporterCSV"/>.</remarks>
       
        protected virtual void CompleteImport(object sender, RunWorkerCompletedEventArgs e)
        {
            this.backgroundWorker = null; //the backgroundWorker is no longer needed.
            //ensure that an error wasn't encountered during importation, and 
            //if there is no error store reference to importedData so it can be
            //retrieved.          
            if ((e.Error == null) && (e.Cancelled == false))
                importedData = e.Result as IToys;
            //Signal completion of the import operation.
            OnImportCompleted(new AsyncCompletedEventArgs(e.Error,e.Cancelled, null));                
            
        }

        /// <summary>
        /// Performs an asynchronous importation of toy data from a csv data file.
        /// </summary>
        /// <param name="sender">A <see cref="System.ComponentModel.BackgroundWorker"/>that invoked the method asynchronously.</param>
        /// <param name="e">Event data passed by the BackgroundWorker.</param>
        /// <remarks>The Argument member of the event data contains the source (filepath) of the csv data file
        /// from which toy data should be imported.</remarks>
        
        //The method creates a CSVReader that is used to read the header and 
        //the records for toys contained in the file. For each record
        //retrieved a new toy is created and added to a new Toys 
        //collection. 
        //NB: Exceptions that occur during the excecution of this method are returned
        //in the ImportCompleted event 
        //To enable reporting of progress the number of records is retrieved.
        //As it is assumed that a header line is always present in the data
        //file the number of toy records contained in the file is 1 less 
        //the amount of records returned by csvReader.Records.
        //

        protected virtual void  DoImport(object sender, DoWorkEventArgs e)
        {            
               
                BackgroundWorker backgroundWorker = sender as BackgroundWorker;
                Thread.Sleep(100);
                IToys toys = new Toys();
                //The Argument member of the event data contains the filepath
                //passed by the caller from which toy data should be imported.
                CSVReader csvReader = new CSVReader(e.Argument as string, FileMode.Open, false);
                int totalRecords = csvReader.Records;
                if (totalRecords > 0) //must contain at least a header record.
                {
                    CSVHeader header = csvReader.ReadHeader(true);
                    for (int i = 1; i < totalRecords; i++)
                    {
                        CSVDataRecord record = null;
                        //Check if user issued a cancellation and cancel import                   
                        if (backgroundWorker.CancellationPending)
                        {
                            e.Cancel = true;
                            break;
                        }
                        else
                        {
                            record = csvReader.ReadDataRecord(i, true);
                            toys.Add(new Toy(record["Item Code"], record["Item Description"], record["Current Count"], record["On Order"]));
                            int progress = (i * 100) / (totalRecords - 1);
                            backgroundWorker.ReportProgress(progress); //don't count the header in the records to import.
                        }
                    }
                }
                e.Result = toys;    
        }
              

        /// <summary>
        /// Raises the <see cref="ImportProgressChanged"/> event of the <see cref="ToyImporterCSV"/>.
        /// </summary>
        /// <param name="e">The progress change data for the event.</param>
        protected virtual void OnImportProgressChanged(ProgressChangedEventArgs e)
        {
            ImportProgressChanged handler = this.ImportProgressChanged;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the ImportCompleted event of the <see cref="ToyImporterCSV"/>.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnImportCompleted(AsyncCompletedEventArgs e)
        {           
            ImportCompletedHandler handler = this.ImportCompleted;
            if (handler != null)
                handler(this, e);
        }

        /// <inheritdoc />
        public void Close()
        {
            Dispose();
        }
        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Implements the IDisposable.Dispose() method.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (backgroundWorker != null)
            {
                backgroundWorker.Dispose();
                backgroundWorker = null;
            }            
        }

        #endregion
    }
       
}
