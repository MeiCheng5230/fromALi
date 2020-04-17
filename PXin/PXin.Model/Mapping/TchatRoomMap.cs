using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatRoomMap : EntityTypeConfiguration<TchatRoom>
    {
        public TchatRoomMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Createtime)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Roomtype)
                .IsRequired();
            this.Property(t => t.Roomname)
                .IsRequired()
                .HasMaxLength(100);
            this.Property(t => t.Roompwd)
                .IsOptional()
                .HasMaxLength(20);
            this.Property(t => t.Remarks)
                .IsOptional()
                .HasMaxLength(500);
            this.Property(t => t.Creater)
                .IsRequired();
            this.Property(t => t.Createtime)
                .IsRequired();
            this.Property(t => t.Roomstate)
                .IsRequired();
            this.Property(t => t.Transferid)
                .IsRequired();
            this.Property(t => t.Dismisstime)
                .IsOptional();
            this.Property(t => t.Roompic)
                .IsOptional()
                .HasMaxLength(100);
            this.Property(t => t.Personcount)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("TCHAT_ROOM", DbContextHelper.GetOwnerByTableName("TCHAT_ROOM"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Roomtype).HasColumnName("ROOMTYPE");
            this.Property(t => t.Roomname).HasColumnName("ROOMNAME");
            this.Property(t => t.Roompwd).HasColumnName("ROOMPWD");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Creater).HasColumnName("CREATER");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Roomstate).HasColumnName("ROOMSTATE");
            this.Property(t => t.Transferid).HasColumnName("TRANSFERID");
            this.Property(t => t.Dismisstime).HasColumnName("DISMISSTIME");
            this.Property(t => t.Roompic).HasColumnName("ROOMPIC");
            this.Property(t => t.Personcount).HasColumnName("PERSONCOUNT");
        }
    }
}
