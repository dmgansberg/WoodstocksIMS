/*==============================================================
 * File name: TestForm.cs
 *
 * Description: A Windows Form to verify that ToyExporterCSV 
 * from the WoodstocksIMS.Library project correctly writes to 
 * the target data file. 
 * 
 * (c) Darren Gansberg, 2014-2015.
 * 
 * 
 ===============================================================*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WoodstocksIMSTest
{
    using Woodstocks.WoodstocksIMS.Data;
    using Woodstocks.WoodstocksIMS.Data.CSV;
    using Woodstocks.WoodstocksIMS.Domain;

    public partial class TestForm : Form
    {
        ToyExporterCSV exporter = null;
        ToyImporterCSV importer = null;

        public TestForm()
        {
            InitializeComponent();
        }

        //Runs when the export button on the Form is clicked. Creates
        //an exporter and invokes the ExportAsync method to write 
        //test data to the file.
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                exporter = new ToyExporterCSV();
                exporter.ExportCompleted += new AsyncCompletedEventHandler(exporter_ExportCompleted);
                exporter.ExportAsync(@"C:\StockFile\stocklist.csv", Test2Data());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                exporter.ExportCompleted -= exporter_ExportCompleted;
                exporter = null;
            }

        }

        //Handles the completion event of the exporter by displaying a message so as to know that 
        //export has completed.
        void exporter_ExportCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    MessageBox.Show("Export completed");
                }
                else
                {
                    MessageBox.Show(e.Error.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                exporter.ExportCompleted -= exporter_ExportCompleted;
                exporter = null;
            }         
        }

        //Creates a set of test data that can be used for testing.
        private static IToys Test1Data()
        {
            IToys testData = new Toys();
            testData.Add(new Toy("A0001", "Horse on Wheels", "8", "No"));
            testData.Add(new Toy("A0002", "Elephant on Wheels", "20", "No"));
            testData.Add(new Toy("A0003", "Dog on Wheels", "30", "Yes"));
            testData.Add(new Toy("A0004", "Seal on Wheels", "34", "No"));
            testData.Add(new Toy("A0005", "Bear on Wheels", "75", "No"));
            testData.Add(new Toy("A0006", "Teddy Bear", "28", "Yes"));
            testData.Add(new Toy("A0007", "Clown", "51", "No"));
            testData.Add(new Toy("A0008", "Puppy(crouch)", "39", "No"));
            return testData;
        }

        //creates a set the expected result set if the file is written using the 
        //test data set returned by TestData1().
        private static IToys Test1ExpectedResult()
        {
            IToys expectedResult = new Toys();
            expectedResult.Add(new Toy("A0001", "Horse on Wheels", "8", "No"));
            expectedResult.Add(new Toy("A0002", "Elephant on Wheels", "20", "No"));
            expectedResult.Add(new Toy("A0003", "Dog on Wheels", "30", "Yes"));
            expectedResult.Add(new Toy("A0004", "Seal on Wheels", "34", "No"));
            expectedResult.Add(new Toy("A0005", "Bear on Wheels", "75", "No"));
            expectedResult.Add(new Toy("A0006", "Teddy Bear", "28", "Yes"));
            expectedResult.Add(new Toy("A0007", "Clown", "51", "No"));
            expectedResult.Add(new Toy("A0008", "Puppy(crouch)", "39", "No"));
            expectedResult.Add(new Toy("A0009", "Puppy(stand)", "2", "No"));
            expectedResult.Add(new Toy("A0010", "Puppy(jump)", "2", "Yes"));
            expectedResult.Add(new Toy("A0011", "Pupp(lying)", "1", "Yes"));
            expectedResult.Add(new Toy("A0012", "Cart with Blocks (50)", "0", "Yes"));
            expectedResult.Add(new Toy("A0013", "Cart with Blocks (100)", "5", "No"));
            expectedResult.Add(new Toy("A0014", "Cart with Blocks (200)", "4", "No"));
            expectedResult.Add(new Toy("A0015", "Train with 0 Carriage", "12", "No"));
            expectedResult.Add(new Toy("A0016", "Train with 1 Carriage", "10", "No"));
            expectedResult.Add(new Toy("A0017", "Train with 2 Carriage", "5", "Yes"));
            expectedResult.Add(new Toy("A0018", "Train with 3 Carriage", "4", "Yes"));
            expectedResult.Add(new Toy("A0019", "Train with 4 Carriage", "5", "No"));
            expectedResult.Add(new Toy("A0020", "Train with 5 Carriage", "2", "No"));
            expectedResult.Add(new Toy("A0021", "Building Blocks (20)", "15", "No"));
            expectedResult.Add(new Toy("A0022", "Building Blocks (30)", "13", "No"));
            expectedResult.Add(new Toy("A0023", "Building Blocks (40)", "16", "No"));
            expectedResult.Add(new Toy("A0024", "Building Blocks (50)", "5", "Yes"));
            expectedResult.Add(new Toy("A0025", "Building Blocks (100)", "2", "Yes"));
            expectedResult.Add(new Toy("A0026", "Building Blocks (200)", "8", "No"));
            expectedResult.Add(new Toy("A0027", "Windmill", "5", "No"));
            expectedResult.Add(new Toy("A0028", "Farmhouse", "6", "Yes"));
            expectedResult.Add(new Toy("A0029", "Fencing", "22", "Yes"));
            expectedResult.Add(new Toy("A0030", "Barn", "12", "Yes"));
            expectedResult.Add(new Toy("A0031", "Tractor", "6", "Yes"));
            expectedResult.Add(new Toy("A0032", "Animals", "3", "Yes"));
            expectedResult.Add(new Toy("A0033", "House", "9", "No"));
            expectedResult.Add(new Toy("A0034", "Car", "12", "No"));
            expectedResult.Add(new Toy("A0035", "Building (small)", "4", "No"));
            expectedResult.Add(new Toy("A0036", "Building (medium)", "3", "No"));
            expectedResult.Add(new Toy("A0037", "Building (tall)", "4", "No"));
            expectedResult.Add(new Toy("A0038", "Shop", "7", "No"));
            expectedResult.Add(new Toy("A0039", "Traffic Lights", "5", "Yes"));
            expectedResult.Add(new Toy("A0040", "Petrol Station", "4", "Yes"));
            return expectedResult;
        }

        //creates a set the expected result set if the file is written using the 
        //test data set returned by TestData2().
        private static IToys Test2ExpectedResult()
        {
            IToys expectedResult = new Toys();
            expectedResult.Add(new Toy("A0001", "Horse on Wheels", "8", "No"));
            expectedResult.Add(new Toy("A0002", "Elephant on Wheels", "20", "No"));
            expectedResult.Add(new Toy("A0003", "Dog on Wheels", "30", "Yes"));
            expectedResult.Add(new Toy("A0004", "Seal on Wheels", "34", "No"));
            expectedResult.Add(new Toy("A0005", "Bear on Wheels", "75", "No"));
            expectedResult.Add(new Toy("A0006", "Teddy Bear", "26", "Yes"));
            expectedResult.Add(new Toy("A0007", "Clown", "51", "No"));
            expectedResult.Add(new Toy("A0008", "Puppy(crouch)", "39", "No"));
            expectedResult.Add(new Toy("A0009", "Puppy(stand)", "2", "No"));
            expectedResult.Add(new Toy("A0010", "Puppy(jump)", "2", "Yes"));
            expectedResult.Add(new Toy("A0011", "Pupp(lying)", "1", "Yes"));
            expectedResult.Add(new Toy("A0012", "Cart with Blocks (50)", "0", "Yes"));
            expectedResult.Add(new Toy("A0013", "Cart with Blocks (100)", "500", "No"));
            expectedResult.Add(new Toy("A0014", "Cart with Blocks (200)", "84156", "No"));
            expectedResult.Add(new Toy("A0015", "Train with 0 Carriage", "12", "No"));
            expectedResult.Add(new Toy("A0016", "Train with 1 Carriage", "10", "No"));
            expectedResult.Add(new Toy("A0017", "Train with 2 Carriage", "5", "Yes"));
            expectedResult.Add(new Toy("A0018", "Train with 3 Carriage", "4", "Yes"));
            expectedResult.Add(new Toy("A0019", "Train with 4 Carriage", "5", "No"));
            expectedResult.Add(new Toy("A0020", "Train with 5 Carriage", "2", "No"));
            expectedResult.Add(new Toy("A0021", "Building Blocks (20)", "15", "No"));
            expectedResult.Add(new Toy("A0022", "Building Blocks (30)", "13", "No"));
            expectedResult.Add(new Toy("A0023", "Building Blocks (40)", "16", "No"));
            expectedResult.Add(new Toy("A0024", "Building Blocks (50)", "5", "Yes"));
            expectedResult.Add(new Toy("A0025", "Building Blocks (100)", "2", "Yes"));
            expectedResult.Add(new Toy("A0026", "Building Blocks (200)", "8", "No"));
            expectedResult.Add(new Toy("A0027", "Windmill", "5", "No"));
            expectedResult.Add(new Toy("A0028", "Farmhouse", "6", "Yes"));
            expectedResult.Add(new Toy("A0029", "Fencing", "22", "Yes"));
            expectedResult.Add(new Toy("A0030", "Barn", "12", "Yes"));
            expectedResult.Add(new Toy("A0031", "Tractor", "6", "Yes"));
            expectedResult.Add(new Toy("A0032", "Animals", "800", "Yes"));
            expectedResult.Add(new Toy("A0033", "House", "9", "No"));
            expectedResult.Add(new Toy("A0034", "Car", "12", "No"));
            expectedResult.Add(new Toy("A0035", "Building (small)", "4", "No"));
            expectedResult.Add(new Toy("A0036", "Building (medium)", "3", "No"));
            expectedResult.Add(new Toy("A0037", "Building (tall)", "4", "No"));
            expectedResult.Add(new Toy("A0038", "Shop", "7", "No"));
            expectedResult.Add(new Toy("A0039", "Traffic Lights", "5", "Yes"));
            expectedResult.Add(new Toy("A0040", "Petrol Station", "496", "Yes"));
            return expectedResult;
        }

        //Creates a set of test data that can be used for testing.
        private static IToys Test2Data()
        {
            IToys testData = new Toys();
            testData.Add(new Toy("A0014", "Cart with Blocks (200)", "84156", "No"));
            testData.Add(new Toy("A0013", "Cart with Blocks (100)", "500", "No"));
            testData.Add(new Toy("A0006", "Teddy Bear", "26", "Yes"));
            testData.Add(new Toy("A0032", "Animals", "800", "Yes"));
            testData.Add(new Toy("A0040", "Petrol Station", "496", "Yes"));
            return testData;
        }

        //Handles the click event of the verify write button on the Form. Creates an importer that reads the data back in.
        //Subscribes to the ImportCompleted event which will actually verify the data written once imported.
        private void btvVerifyWrite_Click(object sender, EventArgs e)
        {
            try
            {
                importer = new ToyImporterCSV();
                importer.ImportCompleted += new ImportCompletedHandler(importer_ImportCompleted);
                importer.ImportAsync(@"C:\StockFile\stocklist.csv");
            }
            catch (Exception ex)
            {
                importer.ImportCompleted -= importer_ImportCompleted;
                importer = null;
                MessageBox.Show(ex.Message);
            }
        }

        //Verifies the data written and displays a message indicating whether verified or not.
        void importer_ImportCompleted(IWoodstocksToyImporter sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.Message);
                }
                else
                {
                    IToys result = importer.GetToys();
                    //call the verifyWriteResult in order to verify the data written.
                    if (verifyWriteResult(Test2ExpectedResult(), result))
                    {
                        MessageBox.Show("Write verified successfully");
                    }
                    else
                    {
                        MessageBox.Show("Write verification unsuccessful");
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                importer.ImportCompleted -= importer_ImportCompleted;
                importer = null;
            }           
        }

        //verifies that two toy collections contain the same set of toy data.

        bool verifyWriteResult(IToys expected, IToys written)
        {
            if (expected.Count != written.Count)
                return false;
            int verified = 0;
            for (int i = 0; i < expected.Count; i++)
            {
                if (written[i].ItemCode != expected[i].ItemCode)
                    return false;
                if (written[i].ItemDescription != expected[i].ItemDescription)
                    return false;
                if (written[i].CurrentCount != expected[i].CurrentCount)
                    return false;
                if (written[i].OnOrderStatus != expected[i].OnOrderStatus)
                    return false;
                ++verified;
            }
            if (verified == expected.Count)
                return true;                
            return false;
        }
     
    }
}
