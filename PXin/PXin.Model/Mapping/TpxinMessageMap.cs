using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinMessageMap : EntityTypeConfiguration<TpxinMessage>
    {
        public TpxinMessageMap()
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
            this.Property(t => t.Price)
                    .IsRequired();
            this.Property(t => t.Content)
                    .IsOptional()
                    .HasMaxLength(1000);
            this.Property(t => t.Up)
                    .IsRequired();
            this.Property(t => t.Down)
                    .IsRequired();
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Video)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Sound)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Picurl)
                    .IsOptional()
                    .HasMaxLength(2000);
            this.Property(t => t.Commentnum)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TPXIN_MESSAGE", DbContextHelper.GetOwnerByTableName("TPXIN_MESSAGE"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Price).HasColumnName("PRICE");
            this.Property(t => t.Content).HasColumnName("CONTENT");
            this.Property(t => t.Up).HasColumnName("UP");
            this.Property(t => t.Down).HasColumnName("DOWN");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Video).HasColumnName("VIDEO");
            this.Property(t => t.Sound).HasColumnName("SOUND");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Picurl).HasColumnName("PICURL");
            this.Property(t => t.Commentnum).HasColumnName("COMMENTNUM");
                  }
    }
}
