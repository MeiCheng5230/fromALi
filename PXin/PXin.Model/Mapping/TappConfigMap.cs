using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TappConfigMap : EntityTypeConfiguration<TappConfig>
    {
        public TappConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Sid)
                    .IsRequired();
            this.Property(t => t.Typename)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.Propertyname)
                    .IsRequired()
                    .HasMaxLength(50);
            this.Property(t => t.Propertyvalue)
                    .IsOptional()
                    .HasMaxLength(2000);
            this.Property(t => t.Remark)
                    .IsOptional()
                    .HasMaxLength(150);
            this.Property(t => t.Updatetime)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TAPP_CONFIG", DbContextHelper.GetOwnerByTableName("TAPP_CONFIG"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Sid).HasColumnName("SID");
            this.Property(t => t.Typename).HasColumnName("TYPENAME");
            this.Property(t => t.Propertyname).HasColumnName("PROPERTYNAME");
            this.Property(t => t.Propertyvalue).HasColumnName("PROPERTYVALUE");
            this.Property(t => t.Remark).HasColumnName("REMARK");
            this.Property(t => t.Updatetime).HasColumnName("UPDATETIME");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
                  }
    }
}
