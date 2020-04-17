using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatGroupUserMap : EntityTypeConfiguration<TchatGroupUser>
    {
        public TchatGroupUserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Groupid)
                .IsRequired();
            this.Property(t => t.Userid)
                .IsRequired();
            this.Property(t => t.Creattime)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("TCHAT_GROUP_USER", DbContextHelper.GetOwnerByTableName("TCHAT_GROUP_USER"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Groupid).HasColumnName("GROUPID");
            this.Property(t => t.Userid).HasColumnName("USERID");
            this.Property(t => t.Creattime).HasColumnName("CREATTIME");
        }
    }
}
