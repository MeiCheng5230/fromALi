using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class TfinBilltransMap : EntityTypeConfiguration<TfinBilltrans>
    {
        public TfinBilltransMap()
        {
            // Primary Key
            this.HasKey(t => t.Transid);
            // Properties
            this.Property(t => t.Transid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Createtime)
                .IsRequired();
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Amount)
                    .IsRequired()
                    .HasPrecision(10, 2);
            this.Property(t => t.Helperid)
                    .IsOptional();
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(500);
            this.Property(t => t.Modifytime)
                    .IsOptional();
            this.Property(t => t.Paytype)
                    .IsRequired();
            this.Property(t => t.Chargetype)
                    .IsOptional();
            this.Property(t => t.OpRemarks)
                    .IsOptional()
                    .HasMaxLength(500);
            this.Property(t => t.Transferids)
                    .IsOptional()
                    .HasMaxLength(500);
            this.Property(t => t.Gateid)
                    .IsOptional()
                    .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("TFIN_BILLTRANS", DbContextHelper.GetOwnerByTableName("TFIN_BILLTRANS"));
            this.Property(t => t.Transid).HasColumnName("TRANSID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Helperid).HasColumnName("HELPERID");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Modifytime).HasColumnName("MODIFYTIME");
            this.Property(t => t.Paytype).HasColumnName("PAYTYPE");
            this.Property(t => t.Chargetype).HasColumnName("CHARGETYPE");
            this.Property(t => t.OpRemarks).HasColumnName("OP_REMARKS");
            this.Property(t => t.Transferids).HasColumnName("TRANSFERIDS");
            this.Property(t => t.Gateid).HasColumnName("GATEID");
        }
    }
}
