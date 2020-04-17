using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TbtcYdTransferHisMap : EntityTypeConfiguration<TbtcYdTransferHis>
    {
        public TbtcYdTransferHisMap()
        {
            // Primary Key
            this.HasKey(t => t.HisId);
            // Properties
            this.Property(t => t.HisId)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.CreateTime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.NodeId)
                    .IsRequired();
            this.Property(t => t.Grade)
                    .IsRequired();
            this.Property(t => t.FromPurseId)
                    .IsRequired();
            this.Property(t => t.Amount)
                    .IsRequired()
                    .HasPrecision(16, 5 );
            this.Property(t => t.ToPurseId)
                    .IsRequired();
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.CreateTime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Reason)
                    .IsRequired();
            this.Property(t => t.Note)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.CurrencyId)
                    .IsOptional();
            this.Property(t => t.BeginTime)
                    .IsRequired();
            this.Property(t => t.EndTime)
                    .IsRequired();
            this.Property(t => t.Sid)
                    .IsRequired();
            this.Property(t => t.Content)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Currencytype)
                    .IsRequired();
            this.Property(t => t.Cv)
                    .IsRequired()
                    .HasPrecision(16, 5 );
            this.Property(t => t.Pic)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Typeid)
                    .IsOptional();
            this.Property(t => t.Rate)
                    .IsOptional()
                    .HasPrecision(13, 2 );
            this.Property(t => t.Ismax)
                    .IsRequired();
            this.Property(t => t.Message)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Terminalcode)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Hblimit)
                    .IsOptional()
                    .HasMaxLength(400);

            // Table & Column Mappings
            this.ToTable("TBTC_YD_TRANSFER_HIS", DbContextHelper.GetOwnerByTableName("TBTC_YD_TRANSFER_HIS"));
            this.Property(t => t.HisId).HasColumnName("HIS_ID");
            this.Property(t => t.NodeId).HasColumnName("NODE_ID");
            this.Property(t => t.Grade).HasColumnName("GRADE");
            this.Property(t => t.FromPurseId).HasColumnName("FROM_PURSE_ID");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.ToPurseId).HasColumnName("TO_PURSE_ID");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.CreateTime).HasColumnName("CREATE_TIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Reason).HasColumnName("REASON");
            this.Property(t => t.Note).HasColumnName("NOTE");
            this.Property(t => t.CurrencyId).HasColumnName("CURRENCY_ID");
            this.Property(t => t.BeginTime).HasColumnName("BEGIN_TIME");
            this.Property(t => t.EndTime).HasColumnName("END_TIME");
            this.Property(t => t.Sid).HasColumnName("SID");
            this.Property(t => t.Content).HasColumnName("CONTENT");
            this.Property(t => t.Currencytype).HasColumnName("CURRENCYTYPE");
            this.Property(t => t.Cv).HasColumnName("CV");
            this.Property(t => t.Pic).HasColumnName("PIC");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Rate).HasColumnName("RATE");
            this.Property(t => t.Ismax).HasColumnName("ISMAX");
            this.Property(t => t.Message).HasColumnName("MESSAGE");
            this.Property(t => t.Terminalcode).HasColumnName("TERMINALCODE");
            this.Property(t => t.Hblimit).HasColumnName("HBLIMIT");
                  }
    }
}
