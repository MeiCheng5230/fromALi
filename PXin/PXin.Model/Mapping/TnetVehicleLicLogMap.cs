using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Map
{
    /// <summary>
    /// 
    /// </summary>
    public class TnetVehicleLicLogMap : EntityTypeConfiguration<TnetVehicleLicLog>
    {
        /// <summary>
        /// 
        /// </summary>
        public TnetVehicleLicLogMap()
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
            this.Property(t => t.Brandmodel)
                    .IsRequired()
                    .HasMaxLength(25);
            this.Property(t => t.Firtdate)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.Usenature)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.Engineno)
                    .IsRequired()
                    .HasMaxLength(25);
            this.Property(t => t.Licplateno)
                    .IsRequired()
                    .HasMaxLength(25);
            this.Property(t => t.Belonger)
                    .IsRequired()
                    .HasMaxLength(25);
            this.Property(t => t.Address)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Registertime)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.Carliccode)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.Cartype)
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
            this.Property(t => t.Status)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TNET_VEHICLE_LIC_LOG", DbContextHelper.GetOwnerByTableName("TNET_VEHICLE_LIC_LOG"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Brandmodel).HasColumnName("BRANDMODEL");
            this.Property(t => t.Firtdate).HasColumnName("FIRTDATE");
            this.Property(t => t.Usenature).HasColumnName("USENATURE");
            this.Property(t => t.Engineno).HasColumnName("ENGINENO");
            this.Property(t => t.Licplateno).HasColumnName("LICPLATENO");
            this.Property(t => t.Belonger).HasColumnName("BELONGER");
            this.Property(t => t.Address).HasColumnName("ADDRESS");
            this.Property(t => t.Registertime).HasColumnName("REGISTERTIME");
            this.Property(t => t.Carliccode).HasColumnName("CARLICCODE");
            this.Property(t => t.Cartype).HasColumnName("CARTYPE");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Cardimg).HasColumnName("CARDIMG");
            this.Property(t => t.Status).HasColumnName("STATUS");
                  }
    }
}
