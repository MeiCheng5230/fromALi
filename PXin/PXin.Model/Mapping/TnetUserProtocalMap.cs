using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class TnetUserProtocalMap : EntityTypeConfiguration<TnetUserProtocal>
    {
        public TnetUserProtocalMap()
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
                    .HasMaxLength(50);
            this.Property(t => t.Type)
                    .IsRequired();
            this.Property(t => t.Version)
                    .IsRequired();
            this.Property(t => t.Content)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TNET_USER_PROTOCAL", DbContextHelper.GetOwnerByTableName("TNET_USER_PROTOCAL"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("NAME");
            this.Property(t => t.Type).HasColumnName("TYPE");
            this.Property(t => t.Version).HasColumnName("VERSION");
            this.Property(t => t.Content).HasColumnName("CONTENT");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
