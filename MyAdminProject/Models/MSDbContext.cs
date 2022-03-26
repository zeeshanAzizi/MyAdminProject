using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace MyAdminProject.Models
{
    public class MSDbContext:DbContext
    {
       // public MSDbContext() : base(){ }

        public DbSet<Tbl_Users> Tbl_Users { get; set; }
        public DbSet<VerifyAccount> VerifyAccounts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DbConnection.ConnectionStr);
        }
    }
}
