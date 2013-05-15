using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deje.Core.Model;
using Deje.Repository.EF.Configuration;

namespace Deje.Repository.EF
{
    public class ModelContext : DbContext
    {
        public ModelContext() : this("Test")
        {
        }

        public ModelContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new DelatnostConfiguration());
            modelBuilder.Configurations.Add(new DobavljacConfiguration());
            modelBuilder.Configurations.Add(new KontaktConfiguration());
            modelBuilder.Configurations.Add(new SlikeDobavljacaConfiguration());
            modelBuilder.Configurations.Add(new VrstaDobavljacaConfiguration());
            modelBuilder.Configurations.Add(new KategorijeArtikalaConfiguration());
            modelBuilder.Configurations.Add(new ArtikalConfiguration());
            modelBuilder.Configurations.Add(new SinonimConfiguration());
            modelBuilder.Configurations.Add(new KorisnickiNalogConfiguration());
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }

        public DbSet<Delatnost> Delatnosti { get; set; }
        public DbSet<Dobavljac> Dobavljaci { get; set; }
        public DbSet<SlikeDobavljaca> SlikeDobavljaca { get; set; }
        public DbSet<VrstaDobavljaca> VrsteDobavljaca { get; set; }
        public DbSet<StatusDobavljaca> StatusiDobavljaca { get; set; }
        public DbSet<KategorijaArtikla> KategorijeArtikala { get; set; }
        public DbSet<Artikal> Artikli { get; set; }
        public DbSet<Sinonim> Sinonimi { get; set; }
        public DbSet<KorisnickiNalog> KorisnickiNalozi { get; set; }

    }
}
