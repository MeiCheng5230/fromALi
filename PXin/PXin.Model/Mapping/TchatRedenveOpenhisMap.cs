using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatRedenveOpenhisMap : EntityTypeConfiguration<TchatRedenveOpenhis>
    {
        public TchatRedenveOpenhisMap()
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
            this.Property(t => t.Hisid)
                .IsRequired();
            this.Property(t => t.Amount)
                .IsRequired()
                .HasPrecision(12, 2);
            this.Property(t => t.Createtime)
                .IsRequired();
            this.Property(t => t.Remarks)
                .IsOptional()
                .HasMaxLength(50);
            this.Property(t => t.Isoptimum)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("TCHAT_REDENVE_OPENHIS", DbContextHelper.GetOwnerByTableName("TCHAT_REDENVE_OPENHIS"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Isoptimum).HasColumnName("ISOPTIMUM");
        }
    }
}
