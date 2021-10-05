using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine
{
   public interface IFerryApplicationSettings
    {
        bool MonoUser { get; }
        string AuthServerAddress { get; }
        string LoginCallbackUrl { get; }
        string LogoutCallbackUrl { get; }
        int TestInt { get; set; }
    }
}
