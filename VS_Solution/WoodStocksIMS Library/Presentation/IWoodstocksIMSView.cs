/*=================================================================
 * 
 * File: IWoodstocksIMSView.cs
 *
 * Description: Defines the IWoodstocksIMSView interface.
 * 
 * (c) Darren Gansberg 2014-2015.
 * 
 * 
 =================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Presentation
{
    using Woodstocks.WoodstocksIMS.Domain;

    /// <summary>
    /// An interface for a View within the Wood Stocks Inventory Management System.
    /// </summary>
    public interface IWoodstocksIMSView
    {
        /// <summary>
        /// Disables import option of the View.
        /// </summary>
        void DisableImport();

        /// <summary>
        /// Enables import option of the View.
        /// </summary>
        void EnableImport();

        ///// <summary>
        ///// Notifies the user of progress of a task.
        ///// </summary>
        ///// <param name="progress">A value between 0 and 100 that indicates the percentage
        ///// progress of the task</param>
        //void UpdateProgress(int progress);

        /// <summary>
        /// Resets the view.
        /// </summary>
        void Reset();

        /// <summary>
        /// Notifies the user that importation has completed.
        /// </summary>
        /// <param name="ex">Exception that occurred during the import operation.</param>
        /// <param name="cancelled">Indicates whether the operation was cancelled by the user.</param>
        /// <param name="result">The result of the import operation.</param>
        /// <remarks>
        /// <para>If the operation was cancelled or an exception occurred whilst completing the operation
        /// then null should be passed as the argument of the result parameter.</para>
        /// <para>If an exception occurs during the operation then the Exception should
        /// be passed as the argument for the ex parameter, otherwise null should be passed for ex.</para>
        /// <para>If the operation was cancelled then true should should be passed as the argument 
        /// for the cancelled parameter, otherwise false should be passed for cancelled.</para></remarks>        
        void NotifyImportCompletion(Exception ex, bool cancelled, IToys result);

        /// <summary>
        /// Notifies the user that exportation has completed.
        /// </summary>
        /// <param name="ex">Any exception that occurred during exportation.</param>
        /// <param name="cancelled">Whether the exportation has been cancelled.</param>
        void NotifyExportCompletion(Exception ex, bool cancelled);

        
    }
}
