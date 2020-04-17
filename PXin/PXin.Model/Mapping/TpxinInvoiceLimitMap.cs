using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinInvoiceLimitMap : EntityTypeConfiguration<TpxinInvoiceLimit>
    {
        public TpxinInvoiceLimitMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Company)
                    .IsRequired()
                    .HasMaxLength(200);
            this.Property(t => t.Taxnum)
                    .IsRequired()
                    .HasMaxLength(200);
            this.Property(t => t.Address)
                    .IsRequired()
                    .HasMaxLength(200);
            this.Property(t => t.Mobile)
                    .IsRequired()
                    .HasMaxLength(200);
            this.Property(t => t.Bank)
                    .IsRequired()
                    .HasMaxLength(200);
            this.Property(t => t.Cardno)
                    .IsRequired()
                    .HasMaxLength(200);
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(200);
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Note)
                    .IsOptional()
                    .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("TPXIN_INVOICE_LIMIT", DbContextHelper.GetOwnerByTableName("TPXIN_INVOICE_LIMIT"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Company).HasColumnName("COMPANY");
            this.Property(t => t.Taxnum).HasColumnName("TAXNUM");
            this.Property(t => t.Address).HasColumnName("ADDRESS");
            this.Property(t => t.Mobile).HasColumnName("MOBILE");
            this.Property(t => t.Bank).HasColumnName("BANK");
            this.Property(t => t.Cardno).HasColumnName("CARDNO");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Note).HasColumnName("NOTE");
                  }
    }
}
