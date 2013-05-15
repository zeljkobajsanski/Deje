using System.ComponentModel.DataAnnotations.Schema;
using Deje.Core.Model;
using System.Data.Entity.ModelConfiguration;

namespace Deje.Repository.EF.Configuration
{
    public class DobavljacConfiguration : EntityTypeConfiguration<Dobavljac>
    {
        public DobavljacConfiguration()
        {
            ToTable("Dobavljaci");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Naziv).IsRequired().HasMaxLength(255);
            Property(x => x.GpsLokacija).IsRequired();
            Ignore(x => x.GpsLatitude);
            Ignore(x => x.GpsLongitude);
            HasRequired(x => x.Delatnost).WithMany().HasForeignKey(x => x.IdDelatnosti);
            HasRequired(x => x.VrstaDobavljaca).WithMany().HasForeignKey(x => x.IdVrsteDobavljaca);
            HasRequired(x => x.Kontakt).WithMany().HasForeignKey(x => x.IdKontakta).WillCascadeOnDelete(true);
            Property(x => x.Opis).IsOptional().HasMaxLength(1000);
            HasRequired(x => x.Status).WithMany().HasForeignKey(x => x.IdStatusa);
            Ignore(x => x.Udaljenost);
            Ignore(x => x.SlikaRaw);
        }
    }
}