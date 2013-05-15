using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Deje.Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Deje.Repository.EF.Configuration
{
    public class DelatnostConfiguration : EntityTypeConfiguration<Delatnost>
    {
        public DelatnostConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("Delatnosti");
            Property(x => x.Naziv).IsRequired().HasMaxLength(255);
        }
    }
}
