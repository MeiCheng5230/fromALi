using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatFriendMap : EntityTypeConfiguration<TchatFriend>
    {
        public TchatFriendMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Createtime)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Mynodeid)
                .IsRequired();
            this.Property(t => t.Friendnodeid)
                .IsRequired();
            this.Property(t => t.Createtime)
                .IsRequired();
            this.Property(t => t.Friendstatus)
                .IsRequired();
            this.Property(t => t.Remarks)
                .IsOptional()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TCHAT_FRIEND", DbContextHelper.GetOwnerByTableName("TCHAT_FRIEND"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Mynodeid).HasColumnName("MYNODEID");
            this.Property(t => t.Friendnodeid).HasColumnName("FRIENDNODEID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Friendstatus).HasColumnName("FRIENDSTATUS");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
        }
    }
}
