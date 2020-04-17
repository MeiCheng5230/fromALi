using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TssoUsercodeMap : EntityTypeConfiguration<TssoUsercode>
    {
        public TssoUsercodeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
                   this.Property(t => t.Usercode)
                    .IsRequired();
            this.Property(t => t.Status)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TSSO_USERCODE", DbContextHelper.GetOwnerByTableName("TSSO_USERCODE"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Usercode).HasColumnName("USERCODE");
            this.Property(t => t.Status).HasColumnName("STATUS");
                  }
    }
}
