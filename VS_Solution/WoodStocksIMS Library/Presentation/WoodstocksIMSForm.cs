/*=================================================================
 * 
 * File: WoodstocksIMSForm.cs
 *
 * Description: Defines the WoodstocksIMSForm, which provides the user
 * interface for the WoodstocksIMS.
 * 
 * (c) Darren Gansberg 2014-2015.
 * 
 * 
 =================================================================*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Woodstocks.WoodstocksIMS.Presentation
{
    using Woodstocks.WoodstocksIMS.Domain;

    using Woodstocks.WoodstocksIMS.Data;
    using Woodstocks.WoodstocksIMS.Data.CSV;

    /// <summary>
    /// A view for the <see cref="WoodstocksIMS"/>.
    /// </summary>
    public partial class WoodstocksIMSForm : Form, IWoodstocksIMSView
    {

        AboutWoodstocksIMSForm aboutForm; 

        /// <summary>
        /// The location of the progress window.
        /// </summary>
        Point progressWindowLocation;

        /// <summary>
        /// A reference for a progress window that is to be displayed to the user whilst operations are taking place.
        /// </summary>
        ProgressDialog progressWindow = null;

        /// <summary>
        /// A reference to the model (i.e. Wood Stocks toy data)
        /// </summary>
        private IToys viewData;

        /// <summary>
        /// A reference to a controller to use the <see cref="WoodstocksIMS"/>.
        /// </summary>
        private WoodstocksIMSController woodstocksIMSController;

        /// <summary>
        /// The file name, including the file path, for the file from which data should be imported.
        /// </summary>
        private string toyDataFilename = @"C:\StockFile\stocklist.csv";

        /// <summary>
        /// Initialises a FormView for the <see cref="WoodstocksIMS"/>.
        /// </summary>
        public WoodstocksIMSForm()
        {
            InitializeComponent();            
            woodstocksIMSController = new WoodstocksIMSController();
            if (woodstocksIMSController != null)
            {
                woodstocksIMSController.SetView(this);           
            }
            this.FormClosing += new FormClosingEventHandler(WoodstocksIMSForm_FormClosing);
        }     


        #region IWoodstocksIMSView Interface members

        /// <inheritdoc/>
        public void DisableImport()
        {
            importButton.Enabled = false; //disables the import button on the Form.
        }

        /// <inheritdoc/>
        public void EnableImport()
        {
            importButton.Enabled = true; //enables the import button on the Form.            
        }

        /// <inheritdoc />
        /// <remarks>
        /// <para>The implementation identifies whether the importation has been cancelled or
        /// whether an error has occurred during importation if it completes without being cancelled.
        /// </para>
        /// <para>If the importation was cancelled the user is notified of the cancellation.</para>
        /// <para>If the importation was not cancelled, but an error occurred during the importation,
        /// the user is notified of the error.</para>
        /// <para>If the importation completed successfully without being cancelled then the user is
        /// notified of the successful completion of the importation.</para>
        /// </remarks>
        public void NotifyImportCompletion(Exception ex, bool cancelled, IToys result)
        {
            if (cancelled == true)
            {
                NotifyImportCancellation();
            }
            else if (ex != null)
            {
                NotifyError(ex); //if the user is not notified?
            }
            else
            {
                this.viewData = result;
                progressWindow.UpdateProgress(100);
                //timer1.Interval = 1000;
                //timer1.Tick += new EventHandler(timer1_Tick);
                //timer1.Start();                
            }
        }

        /// <summary>
        /// Notifies the user that exportation has completed.
        /// </summary>
        /// <param name="ex">Any exception that occurred during exportation.</param>
        /// <param name="cancelled">Whether the exportation was cancelled.</param>
        public void NotifyExportCompletion(Exception ex, bool cancelled)
        {
            if (cancelled == true)
            {
                NotifyImportCancellation();
            }
            else if (ex != null)
            {
                NotifyError(ex); //if the user is not notified?
            }
            else
            {
                NotifyExportCompletion();
            }
        }

        /// <summary>
        /// Notifies the user that exportation has been cancelled.
        /// </summary>
        protected virtual void NotifyExportCancellation()
        {
            MessageBox.Show("Export cancelled");
        }

        /// <summary>
        /// Notifies the user that exportation has completed.
        /// </summary>
        protected virtual void NotifyExportCompletion()
        {
            MessageBox.Show("Data has been exported successfully");
            if (!woodstocksIMSController.UnsavedChanges())
            {
                exportButton.Enabled = false;
                DisplayStatus("Ready.");
            }
        }

        /// <summary>
        /// Notifies the user that importation has been cancelled.
        /// </summary>
        /*The user is notified that the importation has been cancelled by:
         * 1. Displaying a message via message box to the user.
         * 2. Displaying Import cancelled in the status label of the Form. 
         * Additionally the progress bar and and import button are reset.
        */
        protected virtual void NotifyImportCancellation()
        {
            //MessageBox.Show("Cancelled");
          //  DisplayStatus("Import cancelled.");
          //  progressBar.Visible = false;
          //  progressBar.Value = 0;            
          //  importButton.Text = "Import";
        }

        /// <summary>
        /// Notifies the user that an error has occurred.
        /// </summary>
        /// <param name="ex">The exception that has occurred.</param>
        /// <returns>True, if the user has been notified of the error.</returns>
        //The method identifies the nature of the error (i.e. Exception) and invokes a method that handles
        //actually notifying the user of the error that has occurred.
        protected bool NotifyError(Exception ex)
        {
            if (ex is FileNotFoundException)
                { NotifyFileNotFound(ex as FileNotFoundException); return true; }
            //if (ex is UnsavedDataException)
            //    { NotifyUnsavedData(ex as UnsavedDataException); return true; }
            if (ex is DirectoryNotFoundException)
                { NotifyDirectoryNotFound(ex as DirectoryNotFoundException); return true; }
            if (ex is DuplicateToyException)
                { return NotifyDuplicateToyException(ex as DuplicateToyException); }
            if (ex is Exception)
                { return NotifyUnknownException(ex); }
            return false;            
        }
     

        #endregion

        #region Error Notification methods

        /// <summary>
        /// Notifies the user that an unknown problem has occurred.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected bool NotifyUnknownException(Exception ex)
        {
            MessageBox.Show(Properties.Resources.UnknownProblemMessage);
            return true;
        }

        /// <summary>
        /// Notifies the user that an duplicate toy has been added to the collection of toys.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>True to indicate that the method has handled the error, in this case by notifying the user.</returns>
        protected bool NotifyDuplicateToyException(DuplicateToyException ex)
        {
            string errorString = String.Format(Properties.Resources.DuplicateToyMessage, ex.Message);
            MessageBox.Show(errorString);
            return true;
        }


        /// <summary>
        /// Notifies the user that a <see cref="FileNotFoundException"/> has occurred.
        /// </summary>
        /// <param name="ex">The FileNotFoundException that was raised by the application.</param>
        /// <returns>True because the method notifies the user that the exception has occurred.</returns>
        /// <remarks>The user is notified by displaying a message to the user.</remarks>
        protected bool NotifyFileNotFound(FileNotFoundException ex)
        {
            string errorString = String.Format(Properties.Resources.FileNotFoundError, toyDataFilename);
            DialogResult result = MessageBox.Show(errorString, "File not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (result == DialogResult.OK)
            {
                Reset();
            }
            return true;
        }
               
        /// <summary>
        /// Displays a message box to the user informing them that imported data that has been modified has not been saved and 
        /// asks user if they would like to save changes.
        /// </summary>
        /// <returns>The result of the users action indicating if they wish to save, or not.</returns>
        protected DialogResult DisplayUnsavedDataDialog()
        {
            return MessageBox.Show(Properties.Resources.UnsavedDataChangesMessage, "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Notifies the user that a <see cref="DirectoryNotFoundException"/> that has occurred.
        /// </summary>
        /// <param name="ex">The DirectoryNotFoundException.</param>
        /// <returns>True because the method notifies the user that the exception by 
        /// has occurred.</returns>
        protected bool NotifyDirectoryNotFound(DirectoryNotFoundException ex)
        {
            string errorString = ex.Message + Properties.Resources.DirectoryNotFoundError;
            DialogResult result = MessageBox.Show(errorString, "Directory not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (result == DialogResult.OK)
            {
                Reset();
            }
            return true;
        }

        #endregion

        /// <summary>
        /// Handles the Closing event of the Form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WoodstocksIMSForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (woodstocksIMSController.UnsavedChanges())
            {                
                DialogResult result = DisplayUnsavedDataDialog();
                if (result == DialogResult.No) return; //Allow the close event to take place without saving changes.
                else if (result == DialogResult.Cancel) //Cancel the Form Closing event and allow the user to control the application.
                {
                    e.Cancel = true;
                }
                else if (result == DialogResult.Yes) //Export data and exit the program.
                {
                    ExportToys();                 
                }               
            }           
        }       

        /// <summary>
        /// Handles the CellValidating event of the dataGridView used to display data on the Form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {                
                if (e.ColumnIndex == 2)
                {
                    string input = e.FormattedValue.ToString();
                    WoodstocksToyValidator.IsValidCurrentCount(input);                   
                }
            }
            catch (InvalidCurrentCountException)
            {
                e.Cancel = true;
                MessageBox.Show(Properties.Resources.InvalidCurrentCountMessage);
            }           
        }             

        /// <summary>
        /// Causes the form to display toy data in the DataGridView of the Form used to display toy data.
        /// </summary>
        /// <param name="woodstocksToys">The toy data to be displayed to the user.</param>
        protected virtual void DisplayToys(IToys woodstocksToys)
        {
            if (woodstocksToys != null)
            {
                //configure the DataGridView
                //set up columns
                dataGridView.AutoGenerateColumns = false;
                dataGridView.Columns["ItemCode"].DataPropertyName = "ItemCode";
                dataGridView.Columns["ItemDescription"].DataPropertyName = "ItemDescription";
                dataGridView.Columns["CurrentCount"].DataPropertyName = "CurrentCount";
                dataGridView.Columns["OnOrderStatus"].DataPropertyName = "OnOrderStatus";

                //capture current count cell changes to enable validation of values entered for current count.
                dataGridView.CellValidating += new DataGridViewCellValidatingEventHandler(DataGridView_CellValidating);

                //Apply a default sort order.
                woodstocksToys.SortByItemCode(SortOrder.Ascending); 
                //create a binding list for the view data that can be bound to the dgv
                BindingList<IToy> list = new BindingList<IToy>(woodstocksToys);

                //capture changes to toys that make up the view data.
                woodstocksToys.ToyChanged += new ToyChangedEventHandler(viewData_Changed);

                //bind the data to be displayed. 
                dataGridView.DataSource = woodstocksToys;
                dataGridView.Refresh();
                //Allow the user to manipulate the control, enter data etc.
                //Ensure tht the default sort order appears in drop-down-box.
                sortSelector.SelectedIndex = 0;
                dataGridView.Enabled = true;
            }
        }

        /// <summary>
        /// When 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //When data in the view changes, check whether the data displayed contains unsaved changes.
        //if the view data contains unsaved changes then the export button is enabled which will 
        //allow the user to export the data.
        void viewData_Changed(IToy sender, ToyChangedEventArgs e)
        {
            if (woodstocksIMSController.UnsavedChanges())
            {
                exportButton.Enabled = true;
            }
            else
            {
                exportButton.Enabled = false;
            }
        }             

        /// <summary>
        /// Updates the status being displayed by the Form to the user.
        /// </summary>
        /// <param name="status">A status to be displayed to the user.</param>
        protected virtual void DisplayStatus(string status)
        {
            statusLabel.Text = status;
        }

        /// <summary>
        /// Gets the status that is displayed by the Form to the user.
        /// </summary>
        /// <returns>The status being displayed by the Form to the user.</returns>
        protected virtual string GetStatus()
        {
            return statusLabel.Text;
        }            
             
        /// <summary>
        /// Handles the SelectedIndexChanged event of the sort selector.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>The event is raised when the selected value index of the sort selector changes
        /// indicating that the user wants to display the data in a different sort order.
        /// <para>The method identifies how the user would like to sort the data based upon the value of 
        /// the selected index and calls the appropriate method to sort the data before refreshing
        /// the dataGridView displaying the data.</para></remarks>
        private void SortSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (viewData != null)
            {
                switch (sortSelector.SelectedIndex)
                {
                    case (int)SortOption.ByItemCodeAscending:
                        viewData.SortByItemCode(SortOrder.Ascending);
                        break;
                    case (int)SortOption.ByItemCodeDescending:
                        viewData.SortByItemCode(SortOrder.Descending);
                        break;
                    case (int)SortOption.ByCurrentCountAscending:
                        viewData.SortByCurrentCount(SortOrder.Ascending);
                        break;
                    case (int)SortOption.ByCurrentCountDescending:
                        viewData.SortByCurrentCount(SortOrder.Descending);
                        break;
                    case (int)SortOption.ByOnOrderAscending:
                        viewData.SortByOnOrder(SortOrder.Ascending);
                        break;
                    case (int)SortOption.ByOnOrderDescending:
                        viewData.SortByOnOrder(SortOrder.Descending);
                        break;
                }
                dataGridView.Refresh();
            }


        }      
        
        /// <summary>
        /// Handles the click event of the import button on the Form.
        /// </summary>
        /// <param name="sender">The import button that raised the click event.</param>
        /// <param name="e">Event data for a button click event</param>
        private void ImportButton_Click(object sender, EventArgs e)
        {            
            try
            {
                ImportToys();
            }
            catch (UnsavedDataException)
            {                
                DialogResult unsavedDataDialogResult = DisplayUnsavedDataDialog();
                if (unsavedDataDialogResult == DialogResult.Yes)
                {
                    woodstocksIMSController.ExportToysAsync();
                    return;
                }
                else if (unsavedDataDialogResult == DialogResult.No)
                {
                    woodstocksIMSController.DiscardImportedToyData();
                    ImportToys();
                }
                else if (unsavedDataDialogResult == DialogResult.Cancel)
                {
                    woodstocksIMSController.ProgressChanged -= progressWindow.ProgressChanged;
                    woodstocksIMSController.ImportCompleted -= progressWindow.Completed;
                    progressWindow.Dispose();
                    progressWindow = null;
                    return;
                }
            }                 
        }

        /// <summary>
        /// Imports data into the <see cref="WoodstocksIMS"/>.
        /// </summary>
        /// <remarks>A progress window is created to report the progress status to the user, before invoking the controller method
        /// ImportToysAsync() to instruct the system to export data.</remarks>
        protected virtual void ImportToys()
        {
            //create progress window and subscribe to the controller events
            progressWindow = new ProgressDialog("Import Progress");
            progressWindow.Shown += new EventHandler(progressWindow_Shown);           
            woodstocksIMSController.ProgressChanged += progressWindow.ProgressChanged;
            woodstocksIMSController.ImportCompleted += progressWindow.Completed;
            woodstocksIMSController.ImportToysAsync(this.toyDataFilename);

            DialogResult result = progressWindow.ShowDialog(this);
            if (progressWindow.DialogResult == DialogResult.Cancel)
            {               
                woodstocksIMSController.CancelAsync();
            }
            else if (progressWindow.DialogResult == DialogResult.Abort)
            {             
                Exception ex = woodstocksIMSController.GetErrorStatus();
                NotifyError(ex);
            }
            else if (progressWindow.DialogResult == DialogResult.OK)
            {              
                this.viewData = woodstocksIMSController.GetToys();
                this.viewData.ToyChanged += new ToyChangedEventHandler(viewData_ToyChanged);
                DisplayToys(viewData);
                DisplayStatus("Ready.");
                sortSelector.Enabled = true;
                if (!woodstocksIMSController.UnsavedChanges())
                    exportButton.Enabled = false;
            }

            //Unsubscribe from controller events and close the progress window.
            woodstocksIMSController.ProgressChanged -= progressWindow.ProgressChanged;
            woodstocksIMSController.ImportCompleted -= progressWindow.Completed;
            progressWindow.Shown -= progressWindow_Shown;
            progressWindow.Close();
            progressWindow = null;

        }

        /// <summary>
        /// Handles the <see cref="IToys.ToyChanged"/> event for the view data (i.e. data being displayed by the Form).
        /// </summary>
        /// <param name="sender">The view data for which the event was raised.</param>
        /// <param name="e">Event data that indicates the toy that changed, and the nature of the change.</param>
        /// <remarks>The method determines whether the data being displayed has unsaved modified changes. If the application data
        /// is modified and unsaved then the status is updated and the export button is enabled.</remarks>
        void viewData_ToyChanged(IToy sender, ToyChangedEventArgs e)
        {
            if (woodstocksIMSController.UnsavedChanges())
            {
                exportButton.Enabled = true;
                DisplayStatus("Unsaved changes.");
            }
            else
            {
                exportButton.Enabled = false;
                DisplayStatus("Ready.");
            }
        }

        /// <summary>
        /// Handles the shown event of a progress window after it is shown to the user for the first time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>The location of the progress window is retrieved and the move event is subscribed to 
        /// to support tracking of movements of the progress window, and moving of the parent Form/Window moved when the user moves 
        /// the progress window.</remarks>
        void progressWindow_Shown(object sender, EventArgs e)
        {
            progressWindowLocation = progressWindow.Location;
            progressWindow.Move += ProgressWindow_Move;
        }

        /// <summary>
        /// Handles move event of a progress window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>When the progress window is moved by the user, the location of the parent window/form is changed resulting in
        /// the parent window being moved with the progress window.</remarks>
        void ProgressWindow_Move(object sender, EventArgs e)
        {
            Form progressWindow = sender as Form;
            if (progressWindow != null)
            {
                if ((progressWindow.Location.X != progressWindowLocation.X) || (progressWindow.Location.Y != progressWindowLocation.Y))
                {
                    int movementX = progressWindow.Location.X - progressWindowLocation.X;
                    int movementY = progressWindow.Location.Y - progressWindowLocation.Y;
                    progressWindowLocation = progressWindow.Location;
                    Point currentLocation = this.Location;
                    Point newLocation = new Point(Location.X + movementX, Location.Y + movementY);
                    this.Location = newLocation;                   
                }
            }
        }
        
        /// <summary>
        /// Handles the click event of the export button on the Form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportButton_Click(object sender, EventArgs e)
        {
            ExportToys();
        }

        /// <summary>
        /// Exports toy data
        /// </summary>
        /// <remarks>A progress window is created to report the progress status to the user, before invoking the controller method
        /// ExportToysAsync() to instruct the system to export data.</remarks>
        protected virtual void ExportToys()
        {
            progressWindow = new ProgressDialog("Export Progress");
            progressWindow.Shown += new EventHandler(progressWindow_Shown);  
            woodstocksIMSController.ProgressChanged += progressWindow.ProgressChanged;
            woodstocksIMSController.ExportCompleted += progressWindow.Completed;
            woodstocksIMSController.ExportToysAsync();

            DialogResult result = progressWindow.ShowDialog(this);
            if (progressWindow.DialogResult == DialogResult.Cancel)
            {
                woodstocksIMSController.CancelAsync();
            }
            else if (progressWindow.DialogResult == DialogResult.Abort)
            {
                Exception ex = woodstocksIMSController.GetErrorStatus();
                NotifyError(ex);
            }
            else if (progressWindow.DialogResult == DialogResult.OK)
            {
                DisplayStatus("Ready.");
                MessageBox.Show("Data has been exported successfully"); //TODO: Replace message.

                if (!woodstocksIMSController.UnsavedChanges())
                    exportButton.Enabled = false;
            }

            //Unsubscribe from controller events and close the progress window.
            woodstocksIMSController.ProgressChanged -= progressWindow.ProgressChanged;
            woodstocksIMSController.ExportCompleted -= progressWindow.Completed;
            progressWindow.Close();
            progressWindow = null;
        }


        /// <inheritdoc />
        public virtual void Reset()
        {
            DisplayStatus("Ready. Click import to begin.");
            importButton.Enabled = true;
            exportButton.Enabled = false;

        }

        /// <summary>
        /// Handles clicking on the About menu item. Displays an About dialog box.
        /// </summary>
        /// <param name="sender">The form that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aboutForm = new AboutWoodstocksIMSForm();         
            DialogResult result = aboutForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                aboutForm.Dispose();
                aboutForm = null;
            }

        }

        /// <summary>
        /// Handles clicking the Exit menu item.
        /// </summary>
        /// <param name="sender">The Form that raised the event.</param>
        /// <param name="e">Event data</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //close the form.
            this.Close();
        }
       

    }
}
