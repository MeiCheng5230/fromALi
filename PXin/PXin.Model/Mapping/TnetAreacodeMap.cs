using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TnetAreacodeMap : EntityTypeConfiguration<TnetAreacode>
    {
        public TnetAreacodeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Areaname)
                    .IsRequired()
                    .HasMaxLength(50);
            this.Property(t => t.Country)
                    .IsRequired()
                    .HasMaxLength(50);
            this.Property(t => t.Code)
                    .IsRequired()
                    .HasMaxLength(5);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.EnCountry)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.Commonuse)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TNET_AREACODE", DbContextHelper.GetOwnerByTableName("TNET_AREACODE"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Areaname).HasColumnName("AREANAME");
            this.Property(t => t.Country).HasColumnName("COUNTRY");
            this.Property(t => t.Code).HasColumnName("CODE");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.EnCountry).HasColumnName("EN_COUNTRY");
            this.Property(t => t.Commonuse).HasColumnName("COMMONUSE");
                  }
    }
}
