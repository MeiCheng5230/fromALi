using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class VnetCityMap:EntityTypeConfiguration<VnetCity>
    {
        public VnetCityMap()
        {
            this.HasKey(t => t.CityId);
            // Properties
            this.Property(t => t.CityId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CityName)
                .IsRequired();

            this.Property(t => t.ProvinceId)
                .IsRequired();

            this.Property(t => t.Status)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("VNET_CITY", DbContextHelper.GetOwnerByTableName("VNET_CITY"));
            this.Property(t => t.CityId).HasColumnName("CITYID");
            this.Property(t => t.CityName).HasColumnName("CITYNAME");
            this.Property(t => t.ProvinceId).HasColumnName("PROVINCE_ID");
            this.Property(t => t.Status).HasColumnName("STATUS");
        }
    }
}
