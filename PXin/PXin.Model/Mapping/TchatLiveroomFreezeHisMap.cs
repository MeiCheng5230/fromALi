using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class TchatLiveroomFreezeHisMap : EntityTypeConfiguration<TchatLiveroomFreezeHis>
    {
        public TchatLiveroomFreezeHisMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Visitid)
                    .IsRequired();
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Giftpurseid)
                    .IsRequired();
            this.Property(t => t.Amount)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Freezeid)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Unfreezetime)
                    .IsOptional();

            // Table & Column Mappings
            this.ToTable("TCHAT_LIVEROOM_FREEZE_HIS", DbContextHelper.GetOwnerByTableName("TCHAT_LIVEROOM_FREEZE_HIS"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Visitid).HasColumnName("VISITID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Giftpurseid).HasColumnName("GIFTPURSEID");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Freezeid).HasColumnName("FREEZEID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Unfreezetime).HasColumnName("UNFREEZETIME");
                  }
    }
}
