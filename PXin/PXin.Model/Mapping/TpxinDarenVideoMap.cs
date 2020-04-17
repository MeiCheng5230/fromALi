using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinDarenVideoMap : EntityTypeConfiguration<TpxinDarenVideo>
    {
        public TpxinDarenVideoMap()
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
            this.Property(t => t.Url)
                    .IsRequired()
                    .HasMaxLength(400);
            this.Property(t => t.Browsenum)
                    .IsRequired();
            this.Property(t => t.Praisenum)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Duration)
                    .IsRequired();
            this.Property(t => t.Imageurl)
                    .IsOptional()
                    .HasMaxLength(400);

            // Table & Column Mappings
            this.ToTable("TPXIN_DAREN_VIDEO", DbContextHelper.GetOwnerByTableName("TPXIN_DAREN_VIDEO"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Url).HasColumnName("URL");
            this.Property(t => t.Browsenum).HasColumnName("BROWSENUM");
            this.Property(t => t.Praisenum).HasColumnName("PRAISENUM");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Duration).HasColumnName("DURATION");
            this.Property(t => t.Imageurl).HasColumnName("IMAGEURL");
                  }
    }
}
