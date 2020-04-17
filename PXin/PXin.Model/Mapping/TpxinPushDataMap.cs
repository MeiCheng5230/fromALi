using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class TpxinPushDataMap : EntityTypeConfiguration<TpxinPushData>
    {
        public TpxinPushDataMap()
        {
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Id)
                .IsRequired();
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(50);
            this.Property(t => t.Content)
                    .IsRequired()
                    .HasMaxLength(250);
            this.Property(t => t.Url)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Expecttime)
                    .IsRequired();
            this.Property(t => t.Pushtime)
                    .IsRequired();
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TPXIN_PUSH_DATA", DbContextHelper.GetOwnerByTableName("TPXIN_PUSH_DATA"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Title).HasColumnName("TITLE");
            this.Property(t => t.Content).HasColumnName("CONTENT");
            this.Property(t => t.Url).HasColumnName("URL");
            this.Property(t => t.Expecttime).HasColumnName("EXPECTTIME");
            this.Property(t => t.Pushtime).HasColumnName("PUSHTIME");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
        }
    }
}
