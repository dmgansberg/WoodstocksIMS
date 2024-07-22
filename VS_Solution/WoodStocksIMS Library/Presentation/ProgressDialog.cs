using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Woodstocks.WoodstocksIMS.Presentation
{
    public partial class ProgressDialog : Form
    {     
        /// <summary>
        /// Initialises the <see cref="ProgressDialog"/>.
        /// </summary>
        /// <param name="caption">The title for the dialog window.</param>
        public ProgressDialog(string caption)
        {
            InitializeComponent();
            this.Text = caption;
            this.StartPosition = FormStartPosition.CenterParent;
            btnCancel.DialogResult = DialogResult.Cancel;
        }     

        /// <summary>
        /// Updates the progress displayed by the dialog window.
        /// </summary>
        /// <param name="progressPercentage"></param>
        public void UpdateProgress(int progressPercentage)
        {
             progressBar1.Value = progressPercentage;
             progressBar1.Update();
        }

        /// <summary>
        /// Handles the tick event for the Form's timer.
        /// </summary>
        /// <param name="sender">The progress window.</param>
        /// <param name="e">Event data for the tick event.</param>
        /// <remarks>The timer is started after the task completes without an error occurring or being cancelled.</remarks>
        void FormTimer_Tick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }       

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = btnCancel.DialogResult;
        }

        /// <summary>
        /// Event Listener that can be used to handle progress changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.UpdateProgress(e.ProgressPercentage);
        }

        /// <summary>
        /// Event Listener that can be used to handle task completion events.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Event data sent by the sender.</param>
        public void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {                
                this.DialogResult = DialogResult.Abort;
            }
            else
            {                
                this.UpdateProgress(100);
                //A timer is used to ensure that the window stays open for a minimum amount of 
                //time after the task completes, otherwise the user may never see the window.
                FormTimer.Interval = 1500;
                FormTimer.Tick += FormTimer_Tick;
                FormTimer.Start();                
            }
        }     
    }
}
