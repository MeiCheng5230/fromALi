using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatLivesettleHisMap : EntityTypeConfiguration<TchatLivesettleHis>
    {
        public TchatLivesettleHisMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Createtime)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Roomid)
                .IsRequired();
            this.Property(t => t.Periodnum)
                .IsRequired();
            this.Property(t => t.Visitid)
                .IsRequired();
            this.Property(t => t.Nodeid)
                .IsRequired();
            this.Property(t => t.Giftpurseid)
                .IsRequired();
            this.Property(t => t.Amount)
                .IsRequired()
                .HasPrecision(12, 2);
            this.Property(t => t.Transferid)
                .IsRequired();
            this.Property(t => t.Createtime)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("TCHAT_LIVESETTLE_HIS", DbContextHelper.GetOwnerByTableName("TCHAT_LIVESETTLE_HIS"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Roomid).HasColumnName("ROOMID");
            this.Property(t => t.Periodnum).HasColumnName("PERIODNUM");
            this.Property(t => t.Visitid).HasColumnName("VISITID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Giftpurseid).HasColumnName("GIFTPURSEID");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Transferid).HasColumnName("TRANSFERID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
        }
    }
}
