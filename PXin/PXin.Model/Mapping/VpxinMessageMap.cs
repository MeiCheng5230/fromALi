using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class VpxinMessageMap : EntityTypeConfiguration<VpxinMessage>
    {
        public VpxinMessageMap()
        {
            // Primary Key
            this.HasKey(t => t.Infoid);
            // Properties
            this.Property(t => t.Infoid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Hisid)
              .IsRequired();
            this.Property(t => t.Status)
            .IsRequired();
            this.Property(t => t.Localnodeid)
                .IsRequired();
            this.Property(t => t.Msgnodeid)
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
            this.Property(t => t.Ispay)
                    .IsRequired();
            this.Property(t => t.Video)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Sound)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Reward)
                    .IsRequired();
            this.Property(t => t.Picurl)
                    .IsOptional()
                    .HasMaxLength(2000);
            this.Property(t => t.Commentnum)
                    .IsRequired();
            this.Property(t => t.IsUp)
                   .IsRequired();
            this.Property(t => t.IsDown)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("VPXIN_MESSAGE", DbContextHelper.GetOwnerByTableName("VPXIN_MESSAGE"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Localnodeid).HasColumnName("LOCALNODEID");
            this.Property(t => t.Msgnodeid).HasColumnName("MSGNODEID");
            this.Property(t => t.Price).HasColumnName("PRICE");
            this.Property(t => t.Content).HasColumnName("CONTENT");
            this.Property(t => t.Up).HasColumnName("UP");
            this.Property(t => t.Down).HasColumnName("DOWN");
            this.Property(t => t.Ispay).HasColumnName("ISPAY");
            this.Property(t => t.Video).HasColumnName("VIDEO");
            this.Property(t => t.Sound).HasColumnName("SOUND");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Reward).HasColumnName("REWARD");
            this.Property(t => t.Picurl).HasColumnName("PICURL");
            this.Property(t => t.Commentnum).HasColumnName("COMMENTNUM");
            this.Property(t => t.IsUp).HasColumnName("ISUP");
            this.Property(t => t.IsDown).HasColumnName("ISDOWN");
            this.Property(t => t.Status).HasColumnName("ISSTATUS");
        }
    }
}

