using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpcnThirdPartnerMap : EntityTypeConfiguration<TpcnThirdPartner>
    {
        public TpcnThirdPartnerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Accesskeyid)
                    .IsRequired()
                    .HasMaxLength(50);
            this.Property(t => t.Accesssecret)
                    .IsRequired()
                    .HasMaxLength(50);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Storename)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.Logo)
                    .IsOptional()
                    .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TPCN_THIRD_PARTNER", DbContextHelper.GetOwnerByTableName("TPCN_THIRD_PARTNER"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Accesskeyid).HasColumnName("ACCESSKEYID");
            this.Property(t => t.Accesssecret).HasColumnName("ACCESSSECRET");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Storename).HasColumnName("STORENAME");
            this.Property(t => t.Logo).HasColumnName("LOGO");
                  }
    }
}
