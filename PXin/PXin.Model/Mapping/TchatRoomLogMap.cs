using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatRoomLogMap : EntityTypeConfiguration<TchatRoomLog>
    {
        public TchatRoomLogMap()
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
            this.Property(t => t.Roomid)
                .IsRequired();
            this.Property(t => t.Actiontype)
                .IsRequired();
            this.Property(t => t.Createtime)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("TCHAT_ROOM_LOG", DbContextHelper.GetOwnerByTableName("TCHAT_ROOM_LOG"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Roomid).HasColumnName("ROOMID");
            this.Property(t => t.Actiontype).HasColumnName("ACTIONTYPE");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
        }
    }
}
