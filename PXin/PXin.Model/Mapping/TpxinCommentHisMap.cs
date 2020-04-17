using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class TpxinCommentHisMap : EntityTypeConfiguration<TpxinCommentHis>
    {
        public TpxinCommentHisMap()
        {
            // Primary Key
            this.HasKey(t => t.Hisid);
            // Properties
            this.Property(t => t.Hisid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Infoid)
                .IsRequired();
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Content)
                    .IsRequired()
                    .HasMaxLength(400);
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Phisid)
                    .IsRequired();
            this.Property(t => t.Pnodeid)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TPXIN_COMMENT_HIS", DbContextHelper.GetOwnerByTableName("TPXIN_COMMENT_HIS"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Content).HasColumnName("CONTENT");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Phisid).HasColumnName("PHISID");
            this.Property(t => t.Pnodeid).HasColumnName("PNODEID");
        }
    }
}
