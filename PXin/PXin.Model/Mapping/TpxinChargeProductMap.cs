using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    /// <summary>
    /// 相信可兑换商品
    /// </summary>
    public class TpxinChargeProductMap : EntityTypeConfiguration<TpxinChargeProduct>
    {
        /// <summary>
        /// 
        /// </summary>
        public TpxinChargeProductMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Pic)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Price)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Priceunit)
                    .IsRequired()
                    .HasMaxLength(10);
            this.Property(t => t.Purseconfigid)
                    .IsRequired();
            this.Property(t => t.Seqno)
                    .IsRequired();
            this.Property(t => t.Isdel)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(10);
            this.Property(t => t.Pdtvalue)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Note)
                  .IsOptional()
                  .HasMaxLength(1000);
            // Table & Column Mappings
            this.ToTable("TPXIN_CHARGE_PRODUCT", DbContextHelper.GetOwnerByTableName("TPXIN_CHARGE_PRODUCT"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Name).HasColumnName("NAME");
            this.Property(t => t.Pic).HasColumnName("PIC");
            this.Property(t => t.Price).HasColumnName("PRICE");
            this.Property(t => t.Priceunit).HasColumnName("PRICEUNIT");
            this.Property(t => t.Purseconfigid).HasColumnName("PURSECONFIGID");
            this.Property(t => t.Seqno).HasColumnName("SEQNO");
            this.Property(t => t.Isdel).HasColumnName("ISDEL");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Pdtvalue).HasColumnName("PDTVALUE");
            this.Property(t => t.Note).HasColumnName("NOTE");
        }
    }
}
