using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatLiveFansMap : EntityTypeConfiguration<TchatLiveFans>
    {
        public TchatLiveFansMap()
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
            this.Property(t => t.Fansid)
                .IsRequired();
            this.Property(t => t.Createtime)
                .IsRequired();
            this.Property(t => t.Status)
                .IsRequired();
            this.Property(t => t.Canceltime)
                .IsOptional();

            // Table & Column Mappings
            this.ToTable("TCHAT_LIVE_FANS", DbContextHelper.GetOwnerByTableName("TCHAT_LIVE_FANS"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Mynodeid).HasColumnName("MYNODEID");
            this.Property(t => t.Fansid).HasColumnName("FANSID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Canceltime).HasColumnName("CANCELTIME");
        }
    }
}
