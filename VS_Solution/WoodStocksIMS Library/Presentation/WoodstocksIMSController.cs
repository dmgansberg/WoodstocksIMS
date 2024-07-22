/*=================================================================
 * 
 * File: WoodstocksIMSController.cs
 *
 * Description: Defines the WoodstocksIMSController that provides
 * the controller to be used with a View of the WoodstocksIMS.
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
    /// A controller for the <see cref="WoodstocksIMS"/>.
    /// </summary>
    public class WoodstocksIMSController : IWoodstocksIMSController
    {      

        /// <summary>
        /// An <see cref="IWoodstocksIMSClient"/> reference to the WoodstocksIMS.
        /// </summary>
        /// <remarks>The controller uses the client interface of the <see cref="WoodstocksIMS"/>.
        /// The client interface is a simplied interface for the system and exposes 
        /// only the funtionality that a client requires.</remarks>
        // When instantiated the controller is initialised with an interface reference 
        // to the system.
        private IWoodstocksIMSClient woodstocksIMS = WoodstocksIMS.GetApplication();

        /// <summary>
        /// An interface reference to a view to be used with the <see cref="WoodstocksIMS"/>.
        /// </summary>
        private IWoodstocksIMSView view = null;

        /// <summary>
        /// The exception, if any, that has been raised during operation.
        /// </summary>
        private Exception error = null;

        /// <summary>
        /// Indicates whether an operation has been cancelled.
        /// </summary>
        private bool cancelled = false;

        /// <inheritdoc />
        public event AsyncCompletedEventHandler ImportCompleted;

        /// <inheritdoc />
        public event AsyncCompletedEventHandler ExportCompleted;

        /// <inheritdoc/>
        public event ProgressChangedEventHandler ProgressChanged;

        /// <summary>
        /// Initialises a <see cref="WoodstocksIMSController"/>.
        /// </summary>
        public WoodstocksIMSController() 
        {
            if (woodstocksIMS != null)
            {
                woodstocksIMS.ProgressChanged += this.woodstocksIMS_ProgressChanged;
                woodstocksIMS.ImportCompleted += this.woodstocksIMS_ImportCompleted;
                woodstocksIMS.ExportCompleted += this.woodstocksIMS_ExportCompleted;
            }
        
        }   

        /// <inheritdoc />
        public void SetView(IWoodstocksIMSView view)
        {
            //Prevent the controller being set with a null interface reference to the view.
            if (view == null)
                throw new ArgumentNullException();
            this.view = view;
        }

        
        /// <inheritdoc/>
        public void ImportToysAsync(string source)
        {
            try
            {                           
                woodstocksIMS.ImportToysAsync(source);               
            }
            catch (Exception)
            {                
                throw;
            }
        }

        /// <summary>
        /// Handles the <see cref="IWoodstocksIMSClient.ImportCompleted"/> event of an <see cref="WoodstocksIMS"/>.
        /// </summary>
        /// <param name="sender">The WoodstocksIMS.</param>
        /// <param name="e">Event data for the event.</param>
        ///<remarks> 
        ///<para>The event is handled by calling the NotifyImportCompletion method of the view to 
        ///notify it that the impotation has completed.</para>
        ///<para> Unless the importation completed successfully (i.e. the error member is null and the cancelled
        ///member is false) then the error and cancelled members are passed to the method with a null
        ///value for the result parameter. The null value for the result parameter ensures that the result is not
        ///available to the view.</para>
        ///<para>If the importation completed successfully the result is retrieved from the WoodstocksIMS
        ///and passed to the view with false for cancelled, and null for error being passed.</para>
        ///<para>The controller unsubscribes from the ImportProgress and ImportCompleted events 
        ///because it no longer needs to listen for the events.</para></remarks>
        void woodstocksIMS_ImportCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
                this.error = e.Error;
            if (e.Cancelled == true)
                this.cancelled = true;
            AsyncCompletedEventHandler handler = this.ImportCompleted;
            if (handler != null)
                handler(this, e);                            
        }

        /// <summary>
        /// Handles the <see cref="IWoodstocksIMSClient.ProgressChanged"/> event of an <see cref="WoodstocksIMS"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///<remarks> 
        ///<para>The event is handled by calling the UpdateProgress method of the view passing a value
        ///between 0 and 100 that indicates the percentage progress made on asynchronous task/operation.</para>
        ///</remarks>
        void woodstocksIMS_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressChangedEventHandler handler = ProgressChanged;
            if (handler != null)
                handler(this, e);
        }

        /// <inheritdoc/>
        public void CancelAsync()
        {
            woodstocksIMS.CancelAsync();
        }

        /// <inheritdoc/>
        public bool UnsavedChanges()
        {
            return woodstocksIMS.UnsavedChanges();
        } 

        /// <inheritdoc />
        public void ExportToysAsync()
        {
            try
            {
                woodstocksIMS.ExportToysAsync();
                woodstocksIMS.ExportCompleted += new AsyncCompletedEventHandler(woodstocksIMS_ExportCompleted);
                woodstocksIMS.ProgressChanged += woodstocksIMS_ProgressChanged;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Handles the <see cref="IWoodstocksIMSClient.ExportCompleted"/> event of an <see cref="WoodstocksIMS"/>.
        /// </summary>
        /// <param name="sender">The WoodstocksIMS.</param>
        /// <param name="e">Event data for the event.</param>
        ///<remarks> 
        ///<para>The event is handled by calling the NotifyExportCompletion method of the view to 
        ///notify it that the exportation has completed.</para>
        ///<para> The Form is passed any error and the cancellation status of the operation,
        ///before unsubscribing from teh ExportCompleted and ProgressChanged events. 
        ///The controller unsubscribes from the events because it no longer needs to listen for 
        ///them.</para>
        ///</remarks>
        void woodstocksIMS_ExportCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
                this.error = e.Error;
            if (e.Cancelled == true)
                this.cancelled = true;
            AsyncCompletedEventHandler handler = ExportCompleted;
            if (handler != null)
                handler(this, e);         
        }
        
       /// <inheritdoc />
        public IToys GetToys()
        {
            return woodstocksIMS.Toys;
        }

        /// <inheritdoc />
        public Exception GetErrorStatus()
        {
            return error;
        }

        /// <inheritdoc />
        public void ResetErrorStatus()
        {
            error = null;
        }

        /// <inheritdoc />
        public bool GetCancellationStatus()
        {
            return cancelled;
        }

        /// <inheritdoc />
        public void ResetCancellationStatus()
        {
            cancelled = false;
        }

        /// <inheritdoc />
        public void DiscardImportedToyData()
        {
            woodstocksIMS.DiscardImportedToyData();
        }

    }
}
