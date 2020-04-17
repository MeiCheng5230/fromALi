using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class TpxinUserinfoMap : EntityTypeConfiguration<TpxinUserinfo>
    {
        public TpxinUserinfoMap()
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
            this.Property(t => t.Backpic)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Up)
                    .IsRequired();
            this.Property(t => t.Down)
                    .IsRequired();
            this.Property(t => t.P)
                    .IsRequired()
                    .HasPrecision(10, 2);
            this.Property(t => t.V)
                    .IsRequired()
                    .HasPrecision(10, 2);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Lastbuysvc)
                    .IsRequired()
                    .HasPrecision(12, 2);
            this.Property(t => t.Svc)
                    .IsRequired()
                    .HasPrecision(12, 2);
            this.Property(t => t.Apoint)
                    .IsRequired();
            this.Property(t => t.Autochargevamount)
                    .IsRequired()
                    .HasPrecision(10, 2);

            // Table & Column Mappings
            this.ToTable("TPXIN_USERINFO", DbContextHelper.GetOwnerByTableName("TPXIN_USERINFO"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Backpic).HasColumnName("BACKPIC");
            this.Property(t => t.Up).HasColumnName("UP");
            this.Property(t => t.Down).HasColumnName("DOWN");
            this.Property(t => t.P).HasColumnName("P");
            this.Property(t => t.V).HasColumnName("V");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Lastbuysvc).HasColumnName("LASTBUYSVC");
            this.Property(t => t.Svc).HasColumnName("SVC");
            this.Property(t => t.Apoint).HasColumnName("APOINT");
            this.Property(t => t.Autochargevamount).HasColumnName("AUTOCHARGEVAMOUNT");
        }
    }
}
