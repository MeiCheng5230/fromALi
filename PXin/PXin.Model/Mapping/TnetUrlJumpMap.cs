using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class TnetUrlJumpMap : EntityTypeConfiguration<TnetUrlJump>
    {
        public TnetUrlJumpMap()
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
            this.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            this.Property(t => t.Rule)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TNET_URL_JUMP", DbContextHelper.GetOwnerByTableName("TNET_URL_JUMP"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Sid).HasColumnName("SID");
            this.Property(t => t.Name).HasColumnName("NAME");
            this.Property(t => t.Rule).HasColumnName("RULE");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
        }
    }
}
