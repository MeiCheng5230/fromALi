using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinAmountChangeHisMap : EntityTypeConfiguration<TpxinAmountChangeHis>
    {
        public TpxinAmountChangeHisMap()
        {
            // Primary Key
            this.HasKey(t => t.Hisid);
            // Properties
            this.Property(t => t.Hisid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Nodeid)
                .IsRequired();
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Amount)
                    .IsRequired()
                    .HasPrecision(12, 2);
            this.Property(t => t.Reason)
                    .IsRequired();
            this.Property(t => t.Transferid)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Mongodbid)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Amountbefore)
                    .IsRequired()
                    .HasPrecision(12, 2);
            this.Property(t => t.Amountafter)
                    .IsRequired()
                    .HasPrecision(12, 2);

            // Table & Column Mappings
            this.ToTable("TPXIN_AMOUNT_CHANGE_HIS", DbContextHelper.GetOwnerByTableName("TPXIN_AMOUNT_CHANGE_HIS"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Reason).HasColumnName("REASON");
            this.Property(t => t.Transferid).HasColumnName("TRANSFERID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Mongodbid).HasColumnName("MONGODBID");
            this.Property(t => t.Amountbefore).HasColumnName("AMOUNTBEFORE");
            this.Property(t => t.Amountafter).HasColumnName("AMOUNTAFTER");
        }
    }
}
