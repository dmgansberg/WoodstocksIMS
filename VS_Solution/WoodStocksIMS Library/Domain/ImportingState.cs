using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    public partial class WoodstocksIMS: IWoodstocksIMS
    {
        private class ImportingState: WoodstocksIMSState
        {
            public ImportingState(IWoodstocksIMS woodstocksIMS) : base(woodstocksIMS){ }

            /// <summary>
            /// Handles the ProgressChanged event raised by an <see cref="IWoodstocksToyImporter"/>.
            /// </summary>
            /// <param name="sender">The <see cref="IWoodstocksToyImporter"/> that raised the event.</param>
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
            public override void OnImportCompleted(object sender, AsyncCompletedEventArgs e)
            {//Check if there was an error and switch back to the idle state. The error will be propogated.
                if (e.Error != null)
                {
                    //toys and the toy importer cannot be reliably used if an error occurs during importation.
                    SetToyDataSource("");
                    SetToyImporter(null);
                    SetToys(null);
                    woodstocksIMS.SetState(woodstocksIMS.GetIdleState());
                }
                else if ((e.Error == null) && (e.Cancelled == false))
                {
                    SetToys(woodstocksIMS.ToyImporter.GetToys());                    
                }               
                this.ImportCompleted -= OnImportCompleted;
                this.ProgressChanged -= OnProgressChanged;                
                woodstocksIMS.OnImportCompleted(this, e);
            }

            /// <inheritdoc />
            /// <param name="toyImporter"></param>
            /// <remarks>The toy importer is set to null from the importing state when the
            /// toy importer is no longer needed.</remarks>
            public override void SetToyImporter(IWoodstocksToyImporter toyImporter)
            {
                woodstocksIMS.SetToyImporter(toyImporter);
            }

            public override void CancelAsync()
            {
                if (this.GetToyImporter() != null)
                {
                    woodstocksIMS.ToyImporter.ImportCancel();
                    woodstocksIMS.SetState(woodstocksIMS.GetIdleState());
                }              
            }

            public override void SetToys(IToys toys)
            {
                woodstocksIMS.SetToys(toys);
            }

            //Retrieving the imported toy data acts as the trigger to switch from the importing state back to the idle state.
            public override IToys GetToys()
            {
                SetToyImporter(null);
                woodstocksIMS.SetState(woodstocksIMS.GetIdleState());
                return woodstocksIMS.GetToys();
            }

            public override IWoodstocksToyImporter GetToyImporter()
            {
                return woodstocksIMS.GetToyImporter();
            }

            public override void SetToyDataSource(string source)
            {
                woodstocksIMS.SetToyDataSource(source);
            }
        }
    }
}
