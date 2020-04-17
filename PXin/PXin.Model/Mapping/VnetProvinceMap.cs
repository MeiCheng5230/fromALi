using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class VnetProvinceMap:EntityTypeConfiguration<VnetProvince>
    {
        public VnetProvinceMap()
        {
            this.HasKey(t => t.ProvinceId);
            // Properties
            this.Property(t => t.ProvinceId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProvinceName)
                .IsRequired();

            this.Property(t => t.Status)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("VNET_PROVINCE", DbContextHelper.GetOwnerByTableName("VNET_PROVINCE"));
            this.Property(t => t.ProvinceId).HasColumnName("PROVINCE_ID");
            this.Property(t => t.ProvinceName).HasColumnName("PROVINCE_NAME");
            this.Property(t => t.Status).HasColumnName("STATUS");
        }
    }
}
