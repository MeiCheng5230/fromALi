using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatFriendNickMap : EntityTypeConfiguration<TchatFriendNick>
    {
        public TchatFriendNickMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Mynodeid)
                .IsRequired();
            this.Property(t => t.Friendnodeid)
                .IsRequired();
            this.Property(t => t.Nickname)
                .IsOptional()
                .HasMaxLength(100);
            this.Property(t => t.Allowviewmedynamic)
                .IsRequired();
            this.Property(t => t.Viewhedynamic)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("TCHAT_FRIEND_NICK", DbContextHelper.GetOwnerByTableName("TCHAT_FRIEND_NICK"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Mynodeid).HasColumnName("MYNODEID");
            this.Property(t => t.Friendnodeid).HasColumnName("FRIENDNODEID");
            this.Property(t => t.Nickname).HasColumnName("NICKNAME");
            this.Property(t => t.Allowviewmedynamic).HasColumnName("ALLOWVIEWMEDYNAMIC");
            this.Property(t => t.Viewhedynamic).HasColumnName("VIEWHEDYNAMIC");
        }
    }
}
