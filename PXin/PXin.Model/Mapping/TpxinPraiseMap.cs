using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinPraiseMap : EntityTypeConfiguration<TpxinPraise>
    {
        public TpxinPraiseMap()
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
            this.Property(t => t.Fromnodeid)
                    .IsRequired();
            this.Property(t => t.Tonodeid)
                  .IsRequired();
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Reward)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TPXIN_PRAISE", DbContextHelper.GetOwnerByTableName("TPXIN_PRAISE"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Fromnodeid).HasColumnName("FROMNODEID");
            this.Property(t => t.Tonodeid).HasColumnName("TONODEID");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Reward).HasColumnName("REWARD");
                  }
    }
}
