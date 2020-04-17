using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatLivegiftPurseMap : EntityTypeConfiguration<TchatLivegiftPurse>
    {
        public TchatLivegiftPurseMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Pursename)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.Ownertype)
                .IsRequired();
            this.Property(t => t.Pursertype)
                .IsRequired();
            this.Property(t => t.Subid)
                .IsRequired();
            this.Property(t => t.Unitname)
                .IsOptional();

            // Table & Column Mappings
            this.ToTable("TCHAT_LIVEGIFT_PURSE", DbContextHelper.GetOwnerByTableName("TCHAT_LIVEGIFT_PURSE"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Pursename).HasColumnName("PURSENAME");
            this.Property(t => t.Ownertype).HasColumnName("OWNERTYPE");
            this.Property(t => t.Pursertype).HasColumnName("PURSERTYPE");
            this.Property(t => t.Subid).HasColumnName("SUBID");
            this.Property(t => t.Unitname).HasColumnName("UNITNAME");
        }
    }
}
