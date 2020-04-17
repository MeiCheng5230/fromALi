using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    /// <summary>
    /// 兑换历史表
    /// </summary>
    public class TpxinChargeHisMap : EntityTypeConfiguration<TpxinChargeHis>
    {
        /// <summary>
        /// 
        /// </summary>
        public TpxinChargeHisMap()
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
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Purseconfigid)
                    .IsRequired();
            this.Property(t => t.Price)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Num)
                    .IsRequired();
            this.Property(t => t.Amount)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Outnodeid)
                    .IsRequired();
            this.Property(t => t.Note)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Fkid)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TPXIN_CHARGE_HIS", DbContextHelper.GetOwnerByTableName("TPXIN_CHARGE_HIS"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Purseconfigid).HasColumnName("PURSECONFIGID");
            this.Property(t => t.Price).HasColumnName("PRICE");
            this.Property(t => t.Num).HasColumnName("NUM");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Outnodeid).HasColumnName("OUTNODEID");
            this.Property(t => t.Note).HasColumnName("NOTE");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Fkid).HasColumnName("FKID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
