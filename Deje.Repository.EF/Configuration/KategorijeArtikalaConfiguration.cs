using System.Data.Entity.ModelConfiguration;
using Deje.Core.Model;

namespace Deje.Repository.EF.Configuration
{
    public class KategorijeArtikalaConfiguration : EntityTypeConfiguration<KategorijaArtikla>
    {
        public KategorijeArtikalaConfiguration()
        {
            ToTable("KategorijeArtikala");
            Ignore(x => x.BrojArtikala);
        }
    }
}