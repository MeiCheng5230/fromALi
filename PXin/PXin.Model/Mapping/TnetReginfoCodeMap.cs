using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class TnetReginfoCodeMap : EntityTypeConfiguration<TnetReginfoCode>
    {
        public TnetReginfoCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.Infoid);
            // Properties
            this.Property(t => t.Infoid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Nodeid)
                .IsRequired();
            this.Property(t => t.Code)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Usenodeid)
                    .IsRequired()
                    .IsConcurrencyToken();
            this.Property(t => t.Usetime)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TNET_REGINFO_CODE", DbContextHelper.GetOwnerByTableName("TNET_REGINFO_CODE"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Code).HasColumnName("CODE");
            this.Property(t => t.Usenodeid).HasColumnName("USENODEID");
            this.Property(t => t.Usetime).HasColumnName("USETIME");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
        }
    }
}
