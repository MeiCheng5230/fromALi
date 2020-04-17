using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TblcCentcardAuctionHisMap : EntityTypeConfiguration<TblcCentcardAuctionHis>
    {
        public TblcCentcardAuctionHisMap()
        {
            // Primary Key
            this.HasKey(t => t.Infoid);
            // Properties
            this.Property(t => t.Infoid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.PmInfoid)
                    .IsRequired();
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.CentcardHisid)
                    .IsRequired();
            this.Property(t => t.Batch)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TBLC_CENTCARD_AUCTION_HIS", DbContextHelper.GetOwnerByTableName("TBLC_CENTCARD_AUCTION_HIS"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.PmInfoid).HasColumnName("PM_INFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.CentcardHisid).HasColumnName("CENTCARD_HISID");
            this.Property(t => t.Batch).HasColumnName("BATCH");
        }
    }
}
