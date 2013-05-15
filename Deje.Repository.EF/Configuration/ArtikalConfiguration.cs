using System.Data.Entity.ModelConfiguration;
using Deje.Core.Model;

namespace Deje.Repository.EF.Configuration
{
    public class ArtikalConfiguration : EntityTypeConfiguration<Artikal>
    {
        public ArtikalConfiguration()
        {
            ToTable("Artikli");
            HasRequired(x => x.KategorijaArtikla).WithMany().HasForeignKey(x => x.IdKategorijeArtikla);
            HasRequired(x => x.Dobavljac).WithMany(x => x.Ponuda).HasForeignKey(x => x.IdDobavljaca);
            Property(x => x.Cena).HasColumnType("money");
            Ignore(x => x.SlikaRaw);
            Ignore(x => x.Score);
        }
    }
}