using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    /// <summary>
    /// APP本地H5页面配置
    /// </summary>
    public class TappH5ConfigMap : EntityTypeConfiguration<TappH5Config>
    {
        /// <summary>
        /// 
        /// </summary>
        public TappH5ConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Version)
                    .IsRequired()
                    .HasMaxLength(25);
            this.Property(t => t.Downurl)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Onlineurl)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Updatetime)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TAPP_H5_CONFIG", DbContextHelper.GetOwnerByTableName("TAPP_H5_CONFIG"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("NAME");
            this.Property(t => t.Version).HasColumnName("VERSION");
            this.Property(t => t.Downurl).HasColumnName("DOWNURL");
            this.Property(t => t.Onlineurl).HasColumnName("ONLINEURL");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Updatetime).HasColumnName("UPDATETIME");
                  }
    }
}
