using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TblcCentcardInvoiceMap : EntityTypeConfiguration<TblcCentcardInvoice>
    {
        public TblcCentcardInvoiceMap()
        {
            // Primary Key
            this.HasKey(t => t.Infoid);
            // Properties
            this.Property(t => t.Infoid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Idno)
                    .IsRequired();
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Isperson)
                    .IsRequired();
            this.Property(t => t.Head)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Code)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Modifytime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Address)
                    .IsRequired()
                    .HasMaxLength(50);
            this.Property(t => t.Expressno)
                    .IsOptional()
                    .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TBLC_CENTCARD_INVOICE", DbContextHelper.GetOwnerByTableName("TBLC_CENTCARD_INVOICE"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Idno).HasColumnName("IDNO");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Isperson).HasColumnName("ISPERSON");
            this.Property(t => t.Head).HasColumnName("HEAD");
            this.Property(t => t.Code).HasColumnName("CODE");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Modifytime).HasColumnName("MODIFYTIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Address).HasColumnName("ADDRESS");
            this.Property(t => t.Expressno).HasColumnName("EXPRESSNO");
                  }
    }
}
