using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class TnetUserphotoMap : EntityTypeConfiguration<TnetUserphoto>
    {
        public TnetUserphotoMap()
        {
            // Primary Key
            this.HasKey(t => t.Nodeid);
            // Properties
            this.Property(t => t.Nodeid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Photo)
             .IsOptional();
            this.Property(t => t.Signature)
                    .IsOptional();
            this.Property(t => t.Idcard)
                    .IsOptional();
            this.Property(t => t.Createtime)
                    .IsOptional();
            this.Property(t => t.photostyle)
                    .IsOptional();
            this.Property(t => t.Appphoto)
                    .IsOptional()
                    .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("TNET_USERPHOTO", DbContextHelper.GetOwnerByTableName("TNET_USERPHOTO"));
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Photo).HasColumnName("PHOTO");
            this.Property(t => t.Signature).HasColumnName("SIGNATURE");
            this.Property(t => t.Idcard).HasColumnName("IDCARD");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Appphoto).HasColumnName("APPPHOTO");
            this.Property(t => t.photostyle).HasColumnName("PHOTOSTYLE");
            this.Ignore(t => t.Photostyle);
        }
    }
}
