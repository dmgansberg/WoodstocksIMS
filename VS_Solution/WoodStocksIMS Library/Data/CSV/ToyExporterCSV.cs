using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

namespace Woodstocks.WoodstocksIMS.Data.CSV
{
    using Woodstocks.WoodstocksIMS.Domain;
    
    /// <summary>
    /// An exporter to export Wood Stocks <see cref="Toy"/> stock data to a csv data file.
    /// </summary>
    public class ToyExporterCSV: IWoodstocksToyExporter
    {
        /// <summary>
        /// A <see cref="System.ComponentModel.BackgroundWorker"/> to carry out an asynchronous exportation.
        /// </summary>
        private BackgroundWorker backgroundWorker = null;

        /// <summary>
        /// The file name, including the path, that the exporter should export data to.
        /// </summary>
        private string filename = null;
       
        /// <summary>
        /// Initialises a <see cref="ToyExporterCSV"/>.
        /// </summary>
        public ToyExporterCSV() { }

        #region IWoodstocksToyExporter Members

        /// <inheritdoc />
        public event ProgressChangedEventHandler ExportProgressChanged;

        /// <inheritdoc />
        public event AsyncCompletedEventHandler ExportCompleted;
        
        //Assumes that file extension has not been appended.
        /// <summary>
        /// Exports data asynchronously to a csv data file.
        /// </summary>
        /// <param name="filename">The name of the file that data should be exported to, 
        /// resulting in the data in the csv file being updated.</param>
        /// <param name="data">The data to be written to the file.</param>       
        public void ExportAsync(string filename, IToys data)
        {           
            if ((filename == null) || (filename == "")) { throw new ArgumentNullException("Filename is null or empty"); }
            if ((data == null) || (data.Count < 1)) { throw new ArgumentNullException("Data is empty"); }
            if (backgroundWorker != null)
            {
                throw new InvalidOperationException("The exporter is busy exporting data");
            }
            else
            {
                try
                {                    
                    CreateBackgroundWorker(); //OutOfMemoryException can occur.
                    //checks the filename paramenter for the csv file extension, if missing it is appended.
                    this.filename = AppendCSVFileExtension(filename);
                    IToys copy = CopyDataForExport(data);
                    if (backgroundWorker != null)
                    {
                        backgroundWorker.RunWorkerAsync(copy);                        
                    }                    
                }
                catch(Exception)
                {
                    backgroundWorker.Dispose();
                    backgroundWorker = null;
                    throw;
                }
            }          
        }

        /// <summary>
        /// Creates a <see cref="BackgroundWorker"/> to be used to export data asynchronously.
        /// </summary>
        protected virtual void CreateBackgroundWorker()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
            backgroundWorker.ProgressChanged += UpdateProgress;
            backgroundWorker.DoWork += new DoWorkEventHandler(DoExport);
        }

        /// <summary>
        /// Creates a copy of the data that is to be exported to the csv file.
        /// </summary>
        /// <param name="toys">The data to be exported to the file.</param>
        /// <returns>A copy of the data to be exported.</returns>
        /// <remarks>A copy of the data is made prior to exporting the data to ensure that
        /// the data that is to be exported is not modified whilst it is being exported.</remarks>
        protected IToys CopyDataForExport(IToys toys)
        {
            IToys copy = new Toys();
            foreach (IToy toy in toys)
            {
                    copy.Add(new Toy(toy));
            }
            return copy;
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the <see cref="System.ComponentModel.BackgroundWorker"/> 
        /// used to carry out the exportation.
        /// </summary>
        /// <param name="sender">The BackgroundWorker carrying out the exportation.</param>
        /// <param name="e">The event data for the event.</param>
        protected virtual void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.worker = null;
            OnExportCompleted(new AsyncCompletedEventArgs(e.Error, e.Cancelled, null));
            backgroundWorker = null;
        }
               
       /// <summary>
       /// Cancels an asynchronous export of toy data by the exporter.
       /// </summary>
        public void ExportCancel()
        {
            if (backgroundWorker != null) backgroundWorker.CancelAsync();
        }

        /// <summary>
        /// Indicates if the exporter is busy carrying out an asynchronous exportation of toy data.
        /// </summary>
        /// <returns>True if the exporter is busy exporting data, ,otherwise false.</returns>
        public bool IsBusy
        {
            get
            {
                if (backgroundWorker != null) return backgroundWorker.IsBusy;
                else return false;
            }
        }

        /// <inheritdoc />
        public void Close()
        {
            if (backgroundWorker != null) backgroundWorker.Dispose();
        }

        #endregion

        /// <summary>
        /// Handles the progress changed event of the background worker asynchronously exporting data.
        /// </summary>
        /// <param name="sender">The <see cref="BackgroundWorker"/> that raised the event.</param>
        /// <param name="e">The progress of the exportation.</param>
        protected virtual void UpdateProgress(object sender, ProgressChangedEventArgs e)
        {
            OnExportProgressChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="IWoodstocksToyExporter.ExportProgressChanged"/> event.
        /// </summary>
        /// <param name="e">Event data that indicates the progress of the operation.</param>
        protected virtual void OnExportProgressChanged(ProgressChangedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("Progress data is null");
            }
            ProgressChangedEventHandler handler = ExportProgressChanged;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Handles the completion event of the BackgroundWorker by raising the <see cref="ExportCompleted"/> event.
        /// </summary>     
        /// <param name="e">The completion event data sent by the worker.</param>
        protected virtual void OnExportCompleted(AsyncCompletedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("Event data is null");
            }
            AsyncCompletedEventHandler handler = ExportCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }
       
        /// <summary>
        /// The method that is called by a <see cref="BackgroundWorker"/> to perform an asynchronous
        /// exportation of <see cref="IToy"/> data to a csv file.
        /// </summary>
        /// <param name="sender">The worker performing the export.</param>
        /// <param name="e">Event data passed by the worker.</param>
        protected virtual void DoExport(object sender, DoWorkEventArgs e)
        {
            CSVReader csvReader = null;
            CSVWriter csvWriter = null;
            try
            {
                //create a backup of the file that contains the data that should be updated and exported.
                CreateBackupFile(filename);
                BackgroundWorker worker = sender as BackgroundWorker;
                if (worker == null) throw new ArgumentException("Sender is not a BackgroundWorker");
                IToys exportItems = e.Argument as IToys;
                if (exportItems == null) throw new ArgumentException("Argument was not a Toys collection");
                csvReader = new CSVReader(filename, FileMode.Open, false);
                int totalRecords = csvReader.Records;
                if (totalRecords > 0)
                {
                    int recordsWritten = 0;
                    CSVRecord record = null;
                    //in order to make it this far, the existing file had to have been opened successfully. otherwise
                    //FileNotFoundException would have been thrown.                                      
                    Regex regex = new Regex(Path.GetFileName(filename) + "\\z");
                    //determine the path and file name for a new file that the existing file, including updates are written to.
                    string newFile = regex.Replace(filename, "newfile.csv");
                    
                    //creation of the new file that is to be written.
                    csvWriter = new CSVWriter(newFile, FileMode.Create);
                    for (int recordIndex = 0; recordIndex < totalRecords; recordIndex++)
                    {
                        if (worker.CancellationPending)
                        {
                            e.Cancel = true;
                            break;
                        }
                        if (recordIndex == 0) //transfer the header
                        {
                            record = csvReader.ReadHeader(true); //missing value exception? //ArgumentOutOfRange...//FileRelated exceptions  ...Exception == IncompleteRecordException
                            //CSVHeader header = csvReader.ReadHeader(true); //missing value exception? //ArgumentOutOfRange...//FileRelated exceptions  ...Exception == IncompleteRecordException
                            if (record == null) throw new Exception("Could not read header");
                        }
                        else
                        {
                            record = csvReader.ReadDataRecord(recordIndex, true);
                            CSVDataRecord csvDataRecord = record as CSVDataRecord;
                            //CSVDataRecord record = csvReader.ReadDataRecord(recordIndex, true);
                            if (csvDataRecord == null) throw new Exception("Could not convert record to CSVDataRecord");
                            IToy storedToy = new Toy(csvDataRecord["Item Code"], csvDataRecord["Item Description"], csvDataRecord["Current Count"], csvDataRecord["On Order"]);
                            if (exportItems.Contains(storedToy))
                            {
                                csvDataRecord["Current Count"] = exportItems[exportItems.IndexOf(storedToy)].CurrentCount;
                                exportItems.Remove(storedToy);
                            }
                        }
                        csvWriter.WriteCSVRecord(record);
                        ++recordsWritten;
                    }
                    csvWriter.Close();
                    csvReader.Close();
                    File.Copy(newFile, filename, true); //replaced the file with the newly written file that includes the updates.
                    File.Delete(newFile);
                }
            }
            catch (Exception)
            {
                if (csvReader != null) csvReader.Close();
                if (csvWriter != null) csvWriter.Close();                
                throw;
            }
        }

        /// <summary>
        /// Checks whehter a file name, assumed to include, the path of the file has a .csv extension.
        /// Appends the .csv extension if the filename string does not have the .csv extension.
        /// </summary>
        /// <param name="filename">The file name, including the path, to the file.</param>
        /// <returns>A file name, with the .csv extension appended, if it does not contain
        /// a .csv extension, otherwise the original filename.</returns>
        protected string AppendCSVFileExtension(string filename)
        {            
            Regex regex = new Regex(".csv\\z");            
            Match matched = regex.Match(filename);            
            if (!matched.Success)
            {                
              filename += ".csv";
            }
            return filename;
        }
               
       
        /// <summary>
        /// Creates a backup file for the file to which data is to be exported.
        /// </summary>
        /// <param name="filename">The file name, including path, of the file that is to be backed up.</param>
        /* Tests whether the filename input has a .csv file extension appended. 
         * If it does the backup file name is created by replacing the .csv extension with _backup.bak.
         * if it doesn't then the backup file is created by appending _backup.bak to the filename
         * and then the .csv file extension is appended to the filename. NB: The order is important if the .csv file ext
         * was appended prior to creating the backup file name the backup file name would end up filename.csv_backup.bak*/
        protected virtual void CreateBackupFile(string filename)
        {
            try
            {
                string backupFile = "";               
                Regex regex = new Regex(".csv\\z");
                Match matched = regex.Match(filename);
                if (matched.Success)
                {
                    backupFile = regex.Replace(filename, "_backup.bak");
                }
                else
                {                    
                    backupFile = filename + "_backup.bak";
                    filename += ".csv"; //append the file .csv file extension to the filename if it doesn't have one.
                }                                  
                File.Copy(filename, backupFile, true);
            }
            catch (Exception)
            {
                throw;
           }

        }
        
    }
}
