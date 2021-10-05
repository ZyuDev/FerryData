using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Models
{
    public class NameValueDescriptionRow
    {
        public Guid Uid { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
