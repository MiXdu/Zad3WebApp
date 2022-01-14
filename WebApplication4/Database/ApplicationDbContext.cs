using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Database.Entities;

namespace WebApplication4.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<PersonEntity> People { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

    }
}
