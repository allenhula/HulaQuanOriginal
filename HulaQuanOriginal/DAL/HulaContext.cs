using HulaQuanOriginal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace HulaQuanOriginal.DAL
{
    public class HulaContext : DbContext
    {
        public HulaContext() : base("HulaContext")
        { }

        public DbSet<Publish> Publishs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Pet> Pets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // specify singular table names
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}