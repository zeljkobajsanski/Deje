using System.Data.Entity.ModelConfiguration;
using Deje.Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Deje.Repository.EF.Configuration
{
    public class KontaktConfiguration : EntityTypeConfiguration<Kontakt>
    {
        public KontaktConfiguration()
        {
            ToTable("Kontakti");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Adresa).IsRequired().HasMaxLength(100);
            Property(x => x.Mesto).IsRequired().HasMaxLength(100);
            Property(x => x.FiksniTelefon).IsOptional().HasMaxLength(100);
            Property(x => x.MobilniTelefon).IsOptional().HasMaxLength(100);
            Property(x => x.EMail).IsOptional().HasMaxLength(100);
            Property(x => x.Www).IsOptional().HasMaxLength(100);
        }
    }
}