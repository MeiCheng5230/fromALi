using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    /// <summary>
    /// 相信A点竟拍抽奖
    /// </summary>
    public class TpxinLuckDrawMap : EntityTypeConfiguration<TpxinLuckDraw>
    {
        /// <summary>
        /// 
        /// </summary>
        public TpxinLuckDrawMap()
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
            this.Property(t => t.Num)
                    .IsRequired();
            this.Property(t => t.Usednum)
                    .IsRequired();
            this.Property(t => t.Starttime)
                    .IsRequired();
            this.Property(t => t.Endtime)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TPXIN_LUCK_DRAW", DbContextHelper.GetOwnerByTableName("TPXIN_LUCK_DRAW"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Num).HasColumnName("NUM");
            this.Property(t => t.Usednum).HasColumnName("USEDNUM");
            this.Property(t => t.Starttime).HasColumnName("STARTTIME");
            this.Property(t => t.Endtime).HasColumnName("ENDTIME");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
        }
    }
}
