using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatLiveroomMap : EntityTypeConfiguration<TchatLiveroom>
    {
        public TchatLiveroomMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Roomid)
                .IsRequired()
                .HasMaxLength(100);
            this.Property(t => t.Roomname)
                .IsRequired()
                .HasMaxLength(100);
            this.Property(t => t.Roompic)
                .IsOptional()
                .HasMaxLength(100);
            this.Property(t => t.Remarks)
                .IsOptional()
                .HasMaxLength(500);
            this.Property(t => t.Creater)
                .IsRequired();
            this.Property(t => t.Createtime)
                .IsRequired();
            this.Property(t => t.Roomstate)
                .IsRequired();
            this.Property(t => t.Periodnum)
                .IsRequired();
            this.Property(t => t.Roomlocation)
                .IsOptional()
                .HasMaxLength(100);
            this.Property(t => t.Livehbtime)
                .IsRequired();
            this.Property(t => t.Livewatchcount)
                .IsRequired();
            this.Property(t => t.Liveadmirecount)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("TCHAT_LIVEROOM", DbContextHelper.GetOwnerByTableName("TCHAT_LIVEROOM"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Roomid).HasColumnName("ROOMID");
            this.Property(t => t.Roomname).HasColumnName("ROOMNAME");
            this.Property(t => t.Roompic).HasColumnName("ROOMPIC");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Creater).HasColumnName("CREATER");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Roomstate).HasColumnName("ROOMSTATE");
            this.Property(t => t.Periodnum).HasColumnName("PERIODNUM");
            this.Property(t => t.Roomlocation).HasColumnName("ROOMLOCATION");
            this.Property(t => t.Livehbtime).HasColumnName("LIVEHBTIME");
            this.Property(t => t.Livewatchcount).HasColumnName("LIVEWATCHCOUNT");
            this.Property(t => t.Liveadmirecount).HasColumnName("LIVEADMIRECOUNT");
        }
    }
}
