using DAL.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace DAL
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Meeting> Meeting { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

    }
}
