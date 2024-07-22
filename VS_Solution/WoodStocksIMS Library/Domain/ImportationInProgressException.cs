using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Domain
{
    class ImportationInProgressException: InvalidOperationException
    {
        public ImportationInProgressException() : base("Importation in Progress"){}
    }
}
