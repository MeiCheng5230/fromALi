using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatLiveroomPeriodHisMap : EntityTypeConfiguration<TchatLiveroomPeriodHis>
    {
        public TchatLiveroomPeriodHisMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Roomid)
                .IsRequired();
            this.Property(t => t.Periodnum)
                .IsRequired();
            this.Property(t => t.Roomlocation)
                .IsOptional()
                .HasMaxLength(100);
            this.Property(t => t.Livewatchcount)
                .IsRequired();
            this.Property(t => t.Liveadmirecount)
                .IsRequired();
            this.Property(t => t.Createtime)
                .IsRequired();
            this.Property(t => t.Endtime)
                .IsRequired();
            this.Property(t => t.Settlestatus)
                .IsRequired();
            this.Property(t => t.Settletime)
                .IsOptional();

            // Table & Column Mappings
            this.ToTable("TCHAT_LIVEROOM_PERIOD_HIS", DbContextHelper.GetOwnerByTableName("TCHAT_LIVEROOM_PERIOD_HIS"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Roomid).HasColumnName("ROOMID");
            this.Property(t => t.Periodnum).HasColumnName("PERIODNUM");
            this.Property(t => t.Roomlocation).HasColumnName("ROOMLOCATION");
            this.Property(t => t.Livewatchcount).HasColumnName("LIVEWATCHCOUNT");
            this.Property(t => t.Liveadmirecount).HasColumnName("LIVEADMIRECOUNT");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Endtime).HasColumnName("ENDTIME");
            this.Property(t => t.Settlestatus).HasColumnName("SETTLESTATUS");
            this.Property(t => t.Settletime).HasColumnName("SETTLETIME");
        }
    }
}
