using System.Data.Entity.ModelConfiguration;
using Deje.Core.Model;

namespace Deje.Repository.EF.Configuration
{
    public class SinonimConfiguration : EntityTypeConfiguration<Sinonim>
    {
        public SinonimConfiguration()
        {
            ToTable("Sinonimi");
            Ignore(x => x.BrojArtikala);
            HasMany(x => x.Artikli).WithOptional(x => x.Sinonim).HasForeignKey(x => x.IdSinonima);
            Ignore(x => x.SlikaRaw);
            Ignore(x => x.Score);
        }
    }
}