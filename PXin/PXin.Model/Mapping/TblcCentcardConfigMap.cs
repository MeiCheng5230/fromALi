using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace PXin.Model.Mapping
{
    public class TblcCentcardConfigMap : EntityTypeConfiguration<TblcCentcardConfig>
    {
        public TblcCentcardConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Configid);
            // Properties
            this.Property(t => t.Configid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Areaid)
                    .IsRequired();
            this.Property(t => t.Showname)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Price)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Bnum)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TBLC_CENTCARD_CONFIG", DbContextHelper.GetOwnerByTableName("TBLC_CENTCARD_CONFIG"));
            this.Property(t => t.Configid).HasColumnName("CONFIGID");
            this.Property(t => t.Areaid).HasColumnName("AREAID");
            this.Property(t => t.Showname).HasColumnName("SHOWNAME");
            this.Property(t => t.Price).HasColumnName("PRICE");
            this.Property(t => t.Bnum).HasColumnName("BNUM");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
