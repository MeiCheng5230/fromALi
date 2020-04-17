using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TnetNodeconsigneeaddrMap : EntityTypeConfiguration<TnetNodeconsigneeaddr>
    {
        public TnetNodeconsigneeaddrMap()
        {
            // Primary Key
            this.HasKey(t => t.Consigneeid);
            // Properties
            this.Property(t => t.Consigneeid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Countryid)
                    .IsRequired();
            this.Property(t => t.Provinceid)
                    .IsRequired();
            this.Property(t => t.Cityid)
                    .IsRequired();
            this.Property(t => t.Regionid)
                    .IsRequired();
            this.Property(t => t.Address)
                    .IsRequired()
                    .HasMaxLength(400);
            this.Property(t => t.Postcode)
                    .IsOptional()
                    .HasMaxLength(12);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Consigneename)
                    .IsRequired()
                    .HasMaxLength(60);
            this.Property(t => t.Mobile)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Phone)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.TownId)
                    .IsOptional();
            this.Property(t => t.Isdefault)
                    .IsRequired();
            this.Property(t => t.Isspecial)
                    .IsRequired();
            this.Property(t => t.Longitude)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Latitude)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Longitude2)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Latitude2)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Isdel)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TNET_NODECONSIGNEEADDR", DbContextHelper.GetOwnerByTableName("TNET_NODECONSIGNEEADDR"));
            this.Property(t => t.Consigneeid).HasColumnName("CONSIGNEEID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Countryid).HasColumnName("COUNTRYID");
            this.Property(t => t.Provinceid).HasColumnName("PROVINCEID");
            this.Property(t => t.Cityid).HasColumnName("CITYID");
            this.Property(t => t.Regionid).HasColumnName("REGIONID");
            this.Property(t => t.Address).HasColumnName("ADDRESS");
            this.Property(t => t.Postcode).HasColumnName("POSTCODE");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Consigneename).HasColumnName("CONSIGNEENAME");
            this.Property(t => t.Mobile).HasColumnName("MOBILE");
            this.Property(t => t.Phone).HasColumnName("PHONE");
            this.Property(t => t.TownId).HasColumnName("TOWN_ID");
            this.Property(t => t.Isdefault).HasColumnName("ISDEFAULT");
            this.Property(t => t.Isspecial).HasColumnName("ISSPECIAL");
            this.Property(t => t.Longitude).HasColumnName("LONGITUDE");
            this.Property(t => t.Latitude).HasColumnName("LATITUDE");
            this.Property(t => t.Longitude2).HasColumnName("LONGITUDE2");
            this.Property(t => t.Latitude2).HasColumnName("LATITUDE2");
            this.Property(t => t.Isdel).HasColumnName("ISDEL");
                  }
    }
}
