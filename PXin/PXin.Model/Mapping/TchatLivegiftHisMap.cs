using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatLivegiftHisMap : EntityTypeConfiguration<TchatLivegiftHis>
    {
        public TchatLivegiftHisMap()
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
            this.Property(t => t.Fromnodeid)
                .IsRequired();
            this.Property(t => t.Gifttype)
                .IsRequired();
            this.Property(t => t.Giftpurseid)
                .IsRequired();
            this.Property(t => t.Giftprice)
                .IsRequired()
                .HasPrecision(10, 2);
            this.Property(t => t.Giftnum)
                .IsRequired()
                .HasPrecision(10, 2);
            this.Property(t => t.Createtime)
                .IsRequired();
            this.Property(t => t.Status)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("TCHAT_LIVEGIFT_HIS", DbContextHelper.GetOwnerByTableName("TCHAT_LIVEGIFT_HIS"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Roomid).HasColumnName("ROOMID");
            this.Property(t => t.Periodnum).HasColumnName("PERIODNUM");
            this.Property(t => t.Visitid).HasColumnName("VISITID");
            this.Property(t => t.Fromnodeid).HasColumnName("FROMNODEID");
            this.Property(t => t.Gifttype).HasColumnName("GIFTTYPE");
            this.Property(t => t.Giftpurseid).HasColumnName("GIFTPURSEID");
            this.Property(t => t.Giftprice).HasColumnName("GIFTPRICE");
            this.Property(t => t.Giftnum).HasColumnName("GIFTNUM");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Status).HasColumnName("STATUS");
        }
    }
}
