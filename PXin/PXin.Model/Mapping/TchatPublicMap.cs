using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatPublicMap : EntityTypeConfiguration<TchatPublic>
    {
        public TchatPublicMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Createtime)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.PublicId)
                .IsRequired(); 
            this.Property(t => t.PublicName)
                .IsRequired()
                .HasMaxLength(100);
            this.Property(t => t.PublicType)
                .IsRequired();
            this.Property(t => t.Remarks)
                .IsOptional()
                .HasMaxLength(100);
            this.Property(t => t.Createtime)
                .IsRequired();
            this.Property(t => t.PublicLogo)
                .IsOptional()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TCHAT_PUBLIC", DbContextHelper.GetOwnerByTableName("TCHAT_PUBLIC"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.PublicId).HasColumnName("PUBLIC_ID");
            this.Property(t => t.PublicName).HasColumnName("PUBLIC_NAME");
            this.Property(t => t.PublicType).HasColumnName("PUBLIC_TYPE");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.PublicLogo).HasColumnName("PUBLIC_LOGO");
        }
    }
}
