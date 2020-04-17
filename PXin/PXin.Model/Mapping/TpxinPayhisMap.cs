using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinPayhisMap : EntityTypeConfiguration<TpxinPayhis>
    {
        public TpxinPayhisMap()
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
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Price)
                    .IsRequired()
                    .HasPrecision(10, 2);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Tonodeid)
                  .IsRequired();

            // Table & Column Mappings
            this.ToTable("TPXIN_PAYHIS", DbContextHelper.GetOwnerByTableName("TPXIN_PAYHIS"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Price).HasColumnName("PRICE");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Tonodeid).HasColumnName("TONODEID");
        }
    }
}
