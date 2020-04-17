using Common.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Model.Mapping
{
    public class VpxinPraiseMap : EntityTypeConfiguration<VpxinPraise>
    {
        public VpxinPraiseMap()
        {
            // Primary Key
            this.HasKey(t => t.Hisid);
            this.Property(t => t.Hisid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Infoid)
                .IsRequired();
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Reward)
                    .IsRequired();
            this.Property(t => t.Nodename)
                   .IsRequired();
            this.Property(t => t.Type)
               .IsRequired();

            // Table & Column Mappings
            this.ToTable("VPXIN_PRAISE", DbContextHelper.GetOwnerByTableName("VPXIN_PRAISE"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Reward).HasColumnName("REWARD");
            this.Property(t => t.Nodename).HasColumnName("NODENAME");
            this.Property(t => t.Type).HasColumnName("TYPE");
        }
    }
}
