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
    public class VpxinOctoberActivityMap : EntityTypeConfiguration<VpxinOctoberActivity>
    {
        public VpxinOctoberActivityMap()
        {
            this.HasKey(t => t.Hisid);
            this.Property(t => t.Pnodeid)
                .IsRequired();
            this.Property(t => t.Nodeid)
                .IsRequired();
            this.Property(t => t.Typeid)
                .IsRequired();
            this.Property(t => t.Note)
                .IsRequired();
            this.Property(t => t.Pnote)
                .IsRequired();
            this.Property(t => t.Amount)
                .IsRequired();
            this.Property(t => t.Pamount)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("VPXIN_OCTOBER_ACTIVITY", DbContextHelper.GetOwnerByTableName("VPXIN_OCTOBER_ACTIVITY"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Pnodeid).HasColumnName("PNODEID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Note).HasColumnName("NOTE");
            this.Property(t => t.Pnote).HasColumnName("PNOTE");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Pamount).HasColumnName("PAMOUNT");
        }
    }
}
