using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.IS4.Data
{
    public class IsDbContext : IdentityDbContext
    {
        public IsDbContext(DbContextOptions<IsDbContext> options)
            : base(options)
        {

        }
    }
}
