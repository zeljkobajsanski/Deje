using System.Data.Entity.ModelConfiguration;
using Deje.Core.Model;

namespace Deje.Repository.EF.Configuration
{
    public class KorisnickiNalogConfiguration : EntityTypeConfiguration<KorisnickiNalog>
    {
        public KorisnickiNalogConfiguration()
        {
            ToTable("KorisnickiNalozi");
            HasKey(x => x.KorisnickoIme);
            Property(x => x.KorisnickoIme).HasMaxLength(50);
            Property(x => x.Lozinka).IsRequired();
            Property(x => x.EMail).IsOptional().HasMaxLength(150);
            HasRequired(x => x.Dobavljac).WithMany().HasForeignKey(x => x.IdDobavljaca);
        }
    }
}