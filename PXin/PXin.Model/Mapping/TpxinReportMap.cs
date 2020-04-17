using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class TpxinReportMap : EntityTypeConfiguration<TpxinReport>
    {
        public TpxinReportMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Infoid)
                .IsRequired();
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Reason)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Satatus)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TPXIN_REPORT", DbContextHelper.GetOwnerByTableName("TPXIN_REPORT"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Reason).HasColumnName("REASON");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Satatus).HasColumnName("SATATUS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
        }
    }
}
