using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    /// <summary>
    /// Defines event data for the <see cref="IWoodstocksIMSClient.ImportCompleted"/> event.
    /// </summary>
    public class ImportToysCompletedEventArgs : AsyncCompletedEventArgs
    {
        private IToys result = null;

        /// <summary>
        /// Initialises a <see cref="ImportToysCompletedEventArgs"/>.
        /// </summary>
        /// <param name="error">An <see cref="Exception"/> for any exception that has been raised during importation.</param>
        /// <param name="cancelled">A value that indicates if the importation was cancelled.</param>
        /// <param name="userState">A value for identifying an import request</param> //this is unusued how can i get rid of it?
        public ImportToysCompletedEventArgs (Exception error, bool cancelled, object userState) :base(error,cancelled,userState){}

        /// <summary>
        /// Initialises a <see cref="ImportToysCompletedEventArgs"/>.
        /// </summary>
        /// <param name="error">An <see cref="Exception"/> for any exception that has been raised during importation.</param>
        /// <param name="cancelled">A value that indicates if the importation was cancelled.</param>
        /// <param name="userState">A value for identifying an import request</param> //this is unusued how can i get rid of it?
        /// <param name="result">The result of importation (i.e. the toy data imported).</param>
        public ImportToysCompletedEventArgs (Exception error, bool cancelled, object userState, IToys result) :base(error,cancelled,userState)
        {
            this.result = result;
        }

        /// <summary>
        /// Gets the result of an toy importation.
        /// </summary>
        public IToys Result { get { return result; }}
    }
}
