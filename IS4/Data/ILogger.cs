using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RICOMPANY.CommonFunctions.Logger;
using RICOMPANY.CommonFunctions;

namespace FerryData.IS4.Data
{
    public interface ILogger
    {
        public fn.CommonOperationResult log(object text);
    }
}
