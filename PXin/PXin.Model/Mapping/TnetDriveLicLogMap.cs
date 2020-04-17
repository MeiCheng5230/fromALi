using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Map
{
    /// <summary>
    /// 
    /// </summary>
    public class TnetDriveLicLogMap : EntityTypeConfiguration<TnetDriveLicLog>
    {
        /// <summary>
        /// 
        /// </summary>
        public TnetDriveLicLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Cardno)
                    .IsRequired()
                    .HasMaxLength(25);
            this.Property(t => t.VehicleType)
                    .IsRequired()
                    .HasMaxLength(25);
            this.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(25);
            this.Property(t => t.Sex)
                    .IsOptional()
                    .HasMaxLength(5);
            this.Property(t => t.Country)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.Addr)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Birthday)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.Firtdate)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.ValidPeriod)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(250);
            this.Property(t => t.Cardimg)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Fileno)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Enddate)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.CardimgAppendix)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TNET_DRIVE_LIC_LOG", DbContextHelper.GetOwnerByTableName("TNET_DRIVE_LIC_LOG"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Cardno).HasColumnName("CARDNO");
            this.Property(t => t.VehicleType).HasColumnName("VEHICLE_TYPE");
            this.Property(t => t.Name).HasColumnName("NAME");
            this.Property(t => t.Sex).HasColumnName("SEX");
            this.Property(t => t.Country).HasColumnName("COUNTRY");
            this.Property(t => t.Addr).HasColumnName("ADDR");
            this.Property(t => t.Birthday).HasColumnName("BIRTHDAY");
            this.Property(t => t.Firtdate).HasColumnName("FIRTDATE");
            this.Property(t => t.ValidPeriod).HasColumnName("VALID_PERIOD");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Cardimg).HasColumnName("CARDIMG");
            this.Property(t => t.Fileno).HasColumnName("FILENO");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Enddate).HasColumnName("ENDDATE");
            this.Property(t => t.CardimgAppendix).HasColumnName("CARDIMG_APPENDIX");
                  }
    }
}
