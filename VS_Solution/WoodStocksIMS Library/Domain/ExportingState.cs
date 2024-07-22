using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Woodstocks.WoodstocksIMS.Domain
{
    public partial class WoodstocksIMS : IWoodstocksIMS
    {
        private class ExportingState : WoodstocksIMSState
        {
            public ExportingState(IWoodstocksIMS woodstocksIMS) : base(woodstocksIMS) { }

            public override void CancelAsync()
            {
                woodstocksIMS.GetToyExporter().ExportCancel();
            }

            /// <summary>
            /// Handles the ProgressChanged event raised by an <see cref="IWoodstocksToyExporter"/>.
            /// </summary>
            /// <param name="sender">The <see cref="IWoodstocksToyExporter"/> that raised the event.</param>
            /// <param name="e">The <see cref="ProgressChangedEventArgs"/> that contains the progress update data.</param>
            /// <remarks>The is handled by causing the <see cref="WoodstocksIMS"/> to raise its ProgressChanged event.</remarks>
            public override void OnProgressChanged(object sender, ProgressChangedEventArgs e)
            {
                woodstocksIMS.OnProgressChanged(this, e);
            }

            /// <summary>
            /// Handles the ImportCompleted event raised by an <see cref="IWoodstocksToyImporter"/>.
            /// </summary>
            /// <param name="sender">The <see cref="IWoodstocksToyImporter"/> that raised the event.</param>
            /// <param name="e">The <see cref="AsyncCompletedEventArgs"/> that contains event completion data.</param>
            /// <remarks>The event is handled by raising the ImportCompleted event of the <see cref="WoodstocksIMS"/>. 
            /// Essentially rebroadcasting the event and propogating it to the controller.</remarks>
            public override void OnExportCompleted(object sender, AsyncCompletedEventArgs e)
            {
                //if ((e.Error == null) && (e.Cancelled == false))
                //{
                //    IList<IToys> modified = GetModifiedToys() as IList<IToys>;
                //    modified.Clear(); //clear the modified toys list after exporting the updated data.
                //}
                SetToyExporter(null);
                this.ExportCompleted -= OnExportCompleted;
                this.ProgressChanged -= OnProgressChanged;
                woodstocksIMS.SetState(woodstocksIMS.GetIdleState());
                woodstocksIMS.OnExportCompleted(this, e);
            }
                     
            public override void SetToyExporter(IWoodstocksToyExporter toyExporter)
            {
                woodstocksIMS.SetToyExporter(toyExporter);
            }

        }

       

       
    }

}
