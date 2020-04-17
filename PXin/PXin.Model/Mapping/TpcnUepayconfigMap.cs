using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class TpcnUepayconfigMap : EntityTypeConfiguration<TpcnUepayconfig>
    {
        public TpcnUepayconfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Paycode)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Notifyurl)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Status)
                    .IsRequired()
                    .IsConcurrencyToken();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Accesskeyid)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.Accesssecret)
                    .IsOptional()
                    .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TPCN_UEPAYCONFIG", DbContextHelper.GetOwnerByTableName("TPCN_UEPAYCONFIG"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Paycode).HasColumnName("PAYCODE");
            this.Property(t => t.Notifyurl).HasColumnName("NOTIFYURL");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Accesskeyid).HasColumnName("ACCESSKEYID");
            this.Property(t => t.Accesssecret).HasColumnName("ACCESSSECRET");
                  }
    }
}
