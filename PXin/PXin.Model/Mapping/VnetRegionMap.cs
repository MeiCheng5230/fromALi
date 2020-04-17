using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class VnetRegionMap : EntityTypeConfiguration<VnetRegion>
    {
        public VnetRegionMap()
        {
            this.HasKey(t => t.Regionid);
            // Properties
            this.Property(t => t.Regionid)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RegionName)
                .IsRequired();

            this.Property(t => t.CityId)
                .IsRequired();

            this.Property(t => t.Status)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("VNET_REGION", DbContextHelper.GetOwnerByTableName("VNET_REGION"));
            this.Property(t => t.Regionid).HasColumnName("REGIONID");
            this.Property(t => t.RegionName).HasColumnName("REGIONNAME");
            this.Property(t => t.CityId).HasColumnName("CITYID");
            this.Property(t => t.Status).HasColumnName("STATUS");
        }
    }
}
