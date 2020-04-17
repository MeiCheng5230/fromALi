using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinYaoyiyaoMap : EntityTypeConfiguration<TpxinYaoyiyao>
    {
        public TpxinYaoyiyaoMap()
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
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Photo)
                    .IsRequired()
                    .HasMaxLength(150);
            this.Property(t => t.Nickname)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Longitude)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Latitude)
                    .IsRequired()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TPXIN_YAOYIYAO", DbContextHelper.GetOwnerByTableName("TPXIN_YAOYIYAO"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Photo).HasColumnName("PHOTO");
            this.Property(t => t.Nickname).HasColumnName("NICKNAME");
            this.Property(t => t.Longitude).HasColumnName("LONGITUDE");
            this.Property(t => t.Latitude).HasColumnName("LATITUDE");
        }
    }
}
