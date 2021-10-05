using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine
{
   public interface IFerryApplication
    {

        FerryApplicationSettings Settings { get; set; }
        CommonOperationResult CreateNewSession(FerryUser UserData);
        bool MonoUserLoggedIn { get; set; }
    }
}
