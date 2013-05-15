using System.Data.Entity.ModelConfiguration;
using Deje.Core.Model;

namespace Deje.Repository.EF.Configuration
{
    public class VrstaDobavljacaConfiguration : EntityTypeConfiguration<VrstaDobavljaca>
    {
        public VrstaDobavljacaConfiguration()
        {
            ToTable("VrsteDobavljaca");
            HasKey(x => x.Id);
            HasRequired(x => x.Delatnost).WithMany().HasForeignKey(x => x.IdDelatnosti);
        }
    }
}