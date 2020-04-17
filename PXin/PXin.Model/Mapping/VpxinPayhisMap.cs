using Common.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
  public   class VpxinPayhisMap : EntityTypeConfiguration<VpxinPayhis>
    {
        public VpxinPayhisMap()
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
            this.Property(t => t.Price)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Nodename)
                   .IsRequired();

            // Table & Column Mappings
            this.ToTable("VPXIN_PAYHIS", DbContextHelper.GetOwnerByTableName("VPXIN_PRAISE"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Price).HasColumnName("PRICE");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Nodename).HasColumnName("NODENAME");
        }
    }
}
