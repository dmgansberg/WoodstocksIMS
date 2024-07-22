using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Woodstocks.WoodstocksIMS.Data.CSV
{
    class FieldNameArgumentIsNullException: Exception
    {
        public FieldNameArgumentIsNullException() : base() { }
        public FieldNameArgumentIsNullException(string message) : base(message) { }
        public FieldNameArgumentIsNullException(string message, Exception innerException) : base(message, innerException) { }
        public FieldNameArgumentIsNullException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
