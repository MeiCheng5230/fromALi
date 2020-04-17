using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    /// <summary>
    /// TpxinPaiHisOld
    /// </summary>
    public class TpxinPaiHisOldMap : EntityTypeConfiguration<TpxinPaiHisOld>
    {
        /// <summary>
        /// 
        /// </summary>
        public TpxinPaiHisOldMap()
        {
            // Primary Key
            this.HasKey(t => t.Hisid);
            // Properties
            this.Property(t => t.Hisid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Nodeid)
                .IsRequired();
            this.Property(t => t.Num)
                    .IsRequired();
            this.Property(t => t.Price)
                    .IsRequired()
                    .HasPrecision(12, 2);
            this.Property(t => t.Totalprice)
                    .IsRequired()
                    .HasPrecision(12, 2);
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Rankinfo)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Configid)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TPXIN_PAI_HIS_OLD", DbContextHelper.GetOwnerByTableName("TPXIN_PAI_HIS_OLD"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Num).HasColumnName("NUM");
            this.Property(t => t.Price).HasColumnName("PRICE");
            this.Property(t => t.Totalprice).HasColumnName("TOTALPRICE");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Rankinfo).HasColumnName("RANKINFO");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Configid).HasColumnName("CONFIGID");
        }
    }
}
