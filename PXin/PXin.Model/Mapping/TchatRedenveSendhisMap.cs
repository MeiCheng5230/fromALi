using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatRedenveSendhisMap : EntityTypeConfiguration<TchatRedenveSendhis>
    {
        public TchatRedenveSendhisMap()
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
            this.Property(t => t.Redtype)
                .IsRequired();
            this.Property(t => t.Destid)
                .IsRequired();
            this.Property(t => t.Pursetype)
                .IsRequired();
            this.Property(t => t.Amount)
                .IsRequired()
                .HasPrecision(12, 2);
            this.Property(t => t.Num)
                .IsRequired();
            this.Property(t => t.Createtime)
                .IsRequired();
            this.Property(t => t.Status)
                .IsRequired();
            this.Property(t => t.Remars)
                .IsOptional()
                .HasMaxLength(50);
            this.Property(t => t.Opennum)
                .IsRequired();
            this.Property(t => t.Openamount)
                .IsRequired()
                .HasPrecision(12, 2);
            this.Property(t => t.Backamount)
                .IsRequired()
                .HasPrecision(12, 2);

            // Table & Column Mappings
            this.ToTable("TCHAT_REDENVE_SENDHIS", DbContextHelper.GetOwnerByTableName("TCHAT_REDENVE_SENDHIS"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Redtype).HasColumnName("REDTYPE");
            this.Property(t => t.Destid).HasColumnName("DESTID");
            this.Property(t => t.Pursetype).HasColumnName("PURSETYPE");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Num).HasColumnName("NUM");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Remars).HasColumnName("REMARS");
            this.Property(t => t.Opennum).HasColumnName("OPENNUM");
            this.Property(t => t.Openamount).HasColumnName("OPENAMOUNT");
            this.Property(t => t.Backamount).HasColumnName("BACKAMOUNT");
        }
    }
}
