using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpcnIndexSettleMap : EntityTypeConfiguration<TpcnIndexSettle>
    {
        public TpcnIndexSettleMap()
        {
            // Primary Key
            this.HasKey(t => t.Settleid);
            // Properties
            this.Property(t => t.Settleid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Infoid)
                    .IsRequired();
            this.Property(t => t.Periods)
                    .IsRequired();
            this.Property(t => t.Num)
                    .IsRequired()
                    .HasPrecision(16, 5 );
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(400);

            // Table & Column Mappings
            this.ToTable("TPCN_INDEX_SETTLE", DbContextHelper.GetOwnerByTableName("TPCN_INDEX_SETTLE"));
            this.Property(t => t.Settleid).HasColumnName("SETTLEID");
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Periods).HasColumnName("PERIODS");
            this.Property(t => t.Num).HasColumnName("NUM");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
