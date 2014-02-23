using NumerosAleatorios.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace NumerosAleatorios.DAL
{

    public class modelo_montecarloEntities2:  DbContext
    {

        public modelo_montecarloEntities2()
            : base("name=numerosaleatoriosEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    
        //public virtual DbSet<test> tests { get; set; }
       
    }
}