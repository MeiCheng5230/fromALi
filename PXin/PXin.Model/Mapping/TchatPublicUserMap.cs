using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatPublicUserMap : EntityTypeConfiguration<TchatPublicUser>
    {
        public TchatPublicUserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.PublicId)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.NodeId)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("TCHAT_PUBLIC_USER", DbContextHelper.GetOwnerByTableName("TCHAT_PUBLIC_USER"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.PublicId).HasColumnName("PUBLIC_ID");
            this.Property(t => t.NodeId).HasColumnName("NODE_ID");
        }
    }
}
