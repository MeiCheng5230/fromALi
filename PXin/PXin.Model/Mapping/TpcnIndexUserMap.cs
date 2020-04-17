using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpcnIndexUserMap : EntityTypeConfiguration<TpcnIndexUser>
    {
        public TpcnIndexUserMap()
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
                    .IsRequired()
                    .HasPrecision(13, 2 );
            this.Property(t => t.Periods)
                    .IsRequired();
            this.Property(t => t.Fromid)
                    .IsRequired();
            this.Property(t => t.Pkid)
                    .IsOptional();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Price)
                    .IsRequired()
                    .HasPrecision(13, 2 );
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Settle)
                    .IsRequired();
            this.Property(t => t.Gqid)
                    .IsOptional();
            this.Property(t => t.Localnum)
                    .IsRequired()
                    .HasPrecision(12, 2 );

            // Table & Column Mappings
            this.ToTable("TPCN_INDEX_USER", DbContextHelper.GetOwnerByTableName("TPCN_INDEX_USER"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Num).HasColumnName("NUM");
            this.Property(t => t.Periods).HasColumnName("PERIODS");
            this.Property(t => t.Fromid).HasColumnName("FROMID");
            this.Property(t => t.Pkid).HasColumnName("PKID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Price).HasColumnName("PRICE");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Settle).HasColumnName("SETTLE");
            this.Property(t => t.Gqid).HasColumnName("GQID");
            this.Property(t => t.Localnum).HasColumnName("LOCALNUM");
                  }
    }
}
