using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TueMessageMap : EntityTypeConfiguration<TueMessage>
    {
        public TueMessageMap()
        {
            // Primary Key
            this.HasKey(t => t.Hisid);
            // Properties
            this.Property(t => t.Hisid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(400);
            this.Property(t => t.Content)
                    .IsOptional()
                    .HasMaxLength(4000);
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Modifytime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Fkid)
                    .IsRequired();
            this.Property(t => t.Jumpurl)
                    .IsOptional()
                    .HasMaxLength(400);

            // Table & Column Mappings
            this.ToTable("TUE_MESSAGE", DbContextHelper.GetOwnerByTableName("TUE_MESSAGE"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Title).HasColumnName("TITLE");
            this.Property(t => t.Content).HasColumnName("CONTENT");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Modifytime).HasColumnName("MODIFYTIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Fkid).HasColumnName("FKID");
            this.Property(t => t.Jumpurl).HasColumnName("JUMPURL");
                  }
    }
}
