using Microsoft.EntityFrameworkCore;
using Models.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Context
{
    public class MachineManagementDbContext : DbContext
    {
        public MachineManagementDbContext(DbContextOptions<MachineManagementDbContext> options) 
            : base(options)
        { 
        }

        public DbSet<Messages> Messages { get; set; }
        public DbSet<Machines> Machines { get; set; }
    }
}
