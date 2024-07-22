using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    /// <summary>
    /// Defines methods for an exporter to export toy data.
    /// </summary>
    public interface IWoodstocksToyExporter
    {
        /// <summary>
        /// Event that is raised upon progress of exportation.
        /// </summary>
        event ProgressChangedEventHandler ExportProgressChanged;

        /// <summary>
        /// Event that is raised upon completion of exportation.
        /// </summary>
        event AsyncCompletedEventHandler ExportCompleted;

        /// <summary>
        /// Exports toy data to the specified destination.
        /// </summary>
        /// <param name="filename">The file name, including the path, of the file.</param>
        /// <param name="data">The toy data to be exported.</param>
        void ExportAsync(string filename, IToys data);

        /// <summary>
        /// Cancels an asynchronous exportation of toy data.
        /// </summary>
        void ExportCancel();

        /// <summary>
        /// Indicates if the exporter is busy carrying out an exportation.
        /// </summary>
        /// <returns></returns>
        bool IsBusy{ get; }

        /// <summary>
        /// Closes the exporter.
        /// </summary>
        void Close();
    }
}
