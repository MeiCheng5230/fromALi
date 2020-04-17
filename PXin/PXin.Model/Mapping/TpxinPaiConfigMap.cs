using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    /// <summary>
    /// A点竞拍配置表
    /// </summary>
    public class TpxinPaiConfigMap : EntityTypeConfiguration<TpxinPaiConfig>
    {
        /// <summary>
        /// 
        /// </summary>
        public TpxinPaiConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Configid);
            // Properties
            this.Property(t => t.Configid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Month)
                    .IsRequired();
            this.Property(t => t.Num)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Minprice)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Addprice)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Localprice)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Multiple)
                 .IsRequired();

            // Table & Column Mappings
            this.ToTable("TPXIN_PAI_CONFIG", DbContextHelper.GetOwnerByTableName("TPXIN_PAI_CONFIG"));
            this.Property(t => t.Configid).HasColumnName("CONFIGID");
            this.Property(t => t.Month).HasColumnName("MONTH");
            this.Property(t => t.Num).HasColumnName("NUM");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Minprice).HasColumnName("MINPRICE");
            this.Property(t => t.Addprice).HasColumnName("ADDPRICE");
            this.Property(t => t.Localprice).HasColumnName("LOCALPRICE");
            this.Property(t => t.Multiple).HasColumnName("MULTIPLE");
        }
    }
}
