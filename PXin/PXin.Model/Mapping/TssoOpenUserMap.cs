using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TssoOpenUserMap : EntityTypeConfiguration<TssoOpenUser>
    {
        public TssoOpenUserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Opentype)
                    .IsRequired();
            this.Property(t => t.Openid)
                    .IsRequired()
                    .HasMaxLength(50);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TSSO_OPEN_USER", DbContextHelper.GetOwnerByTableName("TSSO_OPEN_USER"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Opentype).HasColumnName("OPENTYPE");
            this.Property(t => t.Openid).HasColumnName("OPENID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
