using System.Data.Entity.ModelConfiguration;
using Deje.Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Deje.Repository.EF.Configuration
{
    public class SlikeDobavljacaConfiguration : EntityTypeConfiguration<SlikeDobavljaca>
    {
        public SlikeDobavljacaConfiguration()
        {
            ToTable("SlikeDobavljaca");
            HasKey(x => x.Id);
            HasRequired(x => x.Dobavljac).WithMany().HasForeignKey(x => x.IdDobavljaca);
            Property(x => x.Ekstenzija).IsRequired().HasMaxLength(5);
        }
    }
}