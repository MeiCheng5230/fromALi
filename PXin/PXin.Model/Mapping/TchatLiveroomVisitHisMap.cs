using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatLiveroomVisitHisMap : EntityTypeConfiguration<TchatLiveroomVisitHis>
    {
        public TchatLiveroomVisitHisMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Nodeid)
                .IsRequired();
            this.Property(t => t.Roomid)
                .IsRequired();
            this.Property(t => t.Periodnum)
                .IsRequired();
            this.Property(t => t.Inputtime)
                .IsRequired();
            this.Property(t => t.Hbtime)
                .IsRequired();
            this.Property(t => t.Outputtime)
                .IsOptional();
            this.Property(t => t.Freezeids)
                .IsOptional()
                .HasMaxLength(100);
            this.Property(t => t.Freezegiftpurseids)
                .IsOptional()
                .HasMaxLength(100);
            this.Property(t => t.Freezeamounts)
                .IsOptional()
                .HasMaxLength(100);
            this.Property(t => t.Freezestatus)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("TCHAT_LIVEROOM_VISIT_HIS", DbContextHelper.GetOwnerByTableName("TCHAT_LIVEROOM_VISIT_HIS"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Roomid).HasColumnName("ROOMID");
            this.Property(t => t.Periodnum).HasColumnName("PERIODNUM");
            this.Property(t => t.Inputtime).HasColumnName("INPUTTIME");
            this.Property(t => t.Hbtime).HasColumnName("HBTIME");
            this.Property(t => t.Outputtime).HasColumnName("OUTPUTTIME");
            this.Property(t => t.Freezeids).HasColumnName("FREEZEIDS");
            this.Property(t => t.Freezegiftpurseids).HasColumnName("FREEZEGIFTPURSEIDS");
            this.Property(t => t.Freezeamounts).HasColumnName("FREEZEAMOUNTS");
            this.Property(t => t.Freezestatus).HasColumnName("FREEZESTATUS");
        }
    }
}
