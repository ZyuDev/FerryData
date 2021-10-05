using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RICOMPANY.CommonFunctions;

namespace FerryData.Engine
{

    public interface IGuidBasedStorable
    {
        string Guid { get; set; }

        string ObjectType { get; }
    }
}