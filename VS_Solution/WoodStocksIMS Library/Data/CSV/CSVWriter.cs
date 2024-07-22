using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Woodstocks.WoodstocksIMS.Data.CSV
{
    /// <summary>
    /// A CSVWriter that is used to write CSVRecords to a file.
    /// </summary>
    public class CSVWriter : IDisposable
    {
        /// <summary>
        /// The file path of the file, that includes the name of the file.
        /// </summary>
        private string filepath = null;

        /// <summary>
        /// The file mode for accessing the file.
        /// </summary>
        FileMode fileMode = 0;

        /// <summary>
        /// Whether the file from which the reader reads is open.
        /// </summary>
        bool isOpen = false;

        /// <summary>
        /// A FileStream for accessing the file.
        /// </summary>
        private FileStream fileStream = null;        

        /// <summary>
        /// A <see cref="System.IO.StreamWriter"/> for writing to the file.
        /// </summary>
        private StreamWriter streamWriter = null;        

        /// <summary>
        /// Whether file should be left open by the CSVReader.
        /// </summary>
        private bool leaveFileOpen = true;

        /// <summary>
        /// The character that is used to separate values in a csv value string. By default it is a comma (,).
        /// </summary>
        private char csvSeparator = ',';        

        /// <summary>
        /// Initialises a <see cref="CSVWriter"/>.
        /// </summary>
        /// <param name="filepath">The filepath of the file that data should be written to.</param>
        /// <param name="fileMode">The mode that the file should be opened in.</param>
        public CSVWriter(string filepath, FileMode fileMode)
        {
            try
            {
                this.filepath = filepath;
                this.fileMode = fileMode;
                Open();
            }
            catch (Exception)
            {
                Dispose();
                throw; //rethrow any exceptions
            }

        }

        /// <summary>
        /// Gets the filepath for the file that the CSVWriter should write data to.
        /// </summary>
        public string Filepath
        {
            get { return filepath; }
        }

        /// <summary>
        /// Gets the status of the file. Returns true if the file is open.
        /// </summary>
        public bool IsOpen
        {
            get { return isOpen; }
        }

        #region IDisposable Members
        /// <summary>
        /// Implements the IDisposable.Dispose() method.
        /// </summary>
        /// <remarks>The implementation calls the protected virtual Dispose() method as per
        /// the Dispose pattern.</remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// Disposes of the resources that are utilised by the <see cref="CSVWriter"/>.
        /// </summary>
        /// <param name="disposing">Indicates if the resources are being disposed. True
        /// if the resources should be disposed.</param>
        /// 
        //Checks whether the file is open. If the file is not open, then the resources
        //should have already been disposed so the function returns immediately 
        //performing no work. 
        //If the file is open and disposal of resources is required, as indicated by
        //disposing, the resources utilised by the CSVWriter are freed, finally isOpen is
        //set to false following disposal to indicate that the file is now closed.
        protected virtual void Dispose(bool disposing)
        {
            if (!IsOpen)
                return;
            //Free managed resouces
            if (disposing)
            {
                if (streamWriter != null)
                {
                    streamWriter.Dispose();
                    streamWriter = null;
                    //Closing StreamWriter closes the FileStream..
                    fileStream = null;
                }
                if (fileStream != null)
                {
                    fileStream.Dispose();
                    fileStream = null;
                }
            }           

            isOpen = false;
        }

        /// <summary>
        /// Opens the file to which data is to be written.
        /// </summary>
        //Checks if the filepath is null or empty and raises an ArgumentException()
        //if either is true. Otherwise, attempts to open a FileStream for the file 
        //using the specified fileMode. If successful, a StreamWriter is created 
        //that is bound to the FileStream to support writing to the file.
        virtual public void Open()
        {
            if (IsOpen)
                return; //nothing do if file is already open.
            if ((filepath == null) || (filepath == ""))
            {
                throw new ArgumentException(); //cannot open the file without a file path. 
            }
            try
            {
                fileStream = new FileStream(filepath, fileMode, FileAccess.Write, FileShare.ReadWrite);
                //fileStream = new FileStream(filepath, fileMode, FileAccess.Write);
                try
                {
                    streamWriter = new StreamWriter(fileStream);
                    isOpen = true;
                }
                catch (Exception)
                {
                    Dispose();
                    throw;
                }
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }             
        }

        /// <summary>
        /// Closes the file the CSVWriter, and its associated file.
        /// </summary>
        virtual public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// Writes the output string to the associated file.
        /// </summary>
        /// <param name="output">The string that is to be written to the file
        /// associated with the CSVWriter.</param>
        protected virtual void WriteToFile(string output)
        {
            try
            {
                if (!IsOpen)
                    Open();
                if (streamWriter != null)
                    streamWriter.WriteLine(output);
                streamWriter.Flush();
                if (leaveFileOpen == false)
                    Close();
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Writes a <see cref="CSVRecord"/> to the file associated with this <see cref="CSVWriter"/>.
        /// </summary>
        /// <param name="record"></param>
        public void WriteCSVRecord(CSVRecord record)
        {
            try
            {
                string output = null;
                for (int i = 0; i < record.Count; i++)
                {
                    if (i >= 1)
                        output += csvSeparator;
                    output += record[i];
                }
                WriteToFile(output);
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
