using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    /// <summary>
    /// 用户中奖信息表
    /// </summary>
    public class TpxinLuckDrawDetailMap : EntityTypeConfiguration<TpxinLuckDrawDetail>
    {
        /// <summary>
        /// 
        /// </summary>
        public TpxinLuckDrawDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Infoid);
            // Properties
            this.Property(t => t.Infoid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Num)
                    .IsRequired();
            this.Property(t => t.Status)
                    .IsRequired()
                    .IsConcurrencyToken();
            this.Property(t => t.Fromtime)
                    .IsRequired();
            this.Property(t => t.Endtime)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TPXIN_LUCK_DRAW_DETAIL", DbContextHelper.GetOwnerByTableName("TPXIN_LUCK_DRAW_DETAIL"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Num).HasColumnName("NUM");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Fromtime).HasColumnName("FROMTIME");
            this.Property(t => t.Endtime).HasColumnName("ENDTIME");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
