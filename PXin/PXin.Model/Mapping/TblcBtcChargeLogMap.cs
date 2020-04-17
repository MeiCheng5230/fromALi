using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TblcBtcChargeLogMap : EntityTypeConfiguration<TblcBtcChargeLog>
    {
        public TblcBtcChargeLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Logid);
            // Properties
            this.Property(t => t.Logid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
                   this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Amount)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Counts)
                    .IsRequired();
            this.Property(t => t.Guidstr)
                    .IsRequired()
                    .HasMaxLength(25);
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(150);
            this.Property(t => t.State)
                    .IsRequired()
                    .IsConcurrencyToken();
            this.Property(t => t.CreateTime)
                    .IsOptional();
            this.Property(t => t.NoticeTime)
                    .IsOptional();
            this.Property(t => t.Payment)
                    .IsRequired();
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Appstoreid)
                    .IsOptional();

            // Table & Column Mappings
            this.ToTable("TBLC_BTC_CHARGE_LOG", DbContextHelper.GetOwnerByTableName("TBLC_BTC_CHARGE_LOG"));
            this.Property(t => t.Logid).HasColumnName("LOGID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Counts).HasColumnName("COUNTS");
            this.Property(t => t.Guidstr).HasColumnName("GUIDSTR");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.State).HasColumnName("STATE");
            this.Property(t => t.CreateTime).HasColumnName("CREATE_TIME");
            this.Property(t => t.NoticeTime).HasColumnName("NOTICE_TIME");
            this.Property(t => t.Payment).HasColumnName("PAYMENT");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Appstoreid).HasColumnName("APPSTOREID");
                  }
    }
}
