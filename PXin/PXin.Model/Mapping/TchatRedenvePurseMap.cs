using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatRedenvePurseMap : EntityTypeConfiguration<TchatRedenvePurse>
    {
        public TchatRedenvePurseMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Createtime)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Pursename)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.Pursetype)
                .IsRequired();
            this.Property(t => t.Pursesubid)
                .IsRequired();
            this.Property(t => t.Createtime)
                .IsRequired();
            this.Property(t => t.Remarks)
                .IsOptional()
                .HasMaxLength(50);
            this.Property(t => t.Remarks)
                .IsOptional()
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("TCHAT_REDENVE_PURSE", DbContextHelper.GetOwnerByTableName("TCHAT_REDENVE_PURSE"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Pursename).HasColumnName("PURSENAME");
            this.Property(t => t.Pursetype).HasColumnName("PURSETYPE");
            this.Property(t => t.Pursesubid).HasColumnName("PURSESUBID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Limitamount).HasColumnName("LIMITAMOUNT");
        }
    }
}
