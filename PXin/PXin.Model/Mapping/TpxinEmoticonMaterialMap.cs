using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinEmoticonMaterialMap : EntityTypeConfiguration<TpxinEmoticonMaterial>
    {
        public TpxinEmoticonMaterialMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            this.Property(t => t.Author)
                    .IsRequired()
                    .HasMaxLength(50);
            this.Property(t => t.Intr)
                    .IsRequired()
                    .HasMaxLength(500);
            this.Property(t => t.Filesize)
                    .IsRequired()
                    .HasMaxLength(50);
            this.Property(t => t.Price)
                    .IsRequired()
                    .HasPrecision(10, 2 );
            this.Property(t => t.Configid)
                    .IsRequired();
            this.Property(t => t.Url)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Filedir)
                    .IsRequired()
                    .HasMaxLength(50);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Buycount)
                    .IsRequired();
            this.Property(t => t.Sendprice)
                    .IsRequired()
                    .HasPrecision(10, 2 );

            // Table & Column Mappings
            this.ToTable("TPXIN_EMOTICON_MATERIAL", DbContextHelper.GetOwnerByTableName("TPXIN_EMOTICON_MATERIAL"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Name).HasColumnName("NAME");
            this.Property(t => t.Author).HasColumnName("AUTHOR");
            this.Property(t => t.Intr).HasColumnName("INTR");
            this.Property(t => t.Filesize).HasColumnName("FILESIZE");
            this.Property(t => t.Price).HasColumnName("PRICE");
            this.Property(t => t.Configid).HasColumnName("CONFIGID");
            this.Property(t => t.Url).HasColumnName("URL");
            this.Property(t => t.Filedir).HasColumnName("FILEDIR");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Buycount).HasColumnName("BUYCOUNT");
            this.Property(t => t.Sendprice).HasColumnName("SENDPRICE");
                  }
    }
}
