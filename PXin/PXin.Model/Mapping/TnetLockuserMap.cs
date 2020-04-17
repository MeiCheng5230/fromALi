using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TnetLockuserMap : EntityTypeConfiguration<TnetLockuser>
    {
        public TnetLockuserMap()
        {
            // Primary Key
            this.HasKey(t => t.Nodeid);
            // Properties
            this.Property(t => t.Nodeid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
                   this.Property(t => t.Nodecode)
                    .IsOptional()
                    .HasMaxLength(10);
            this.Property(t => t.Locktime)
                    .IsOptional();
            this.Property(t => t.Unlocktime)
                    .IsOptional();
            this.Property(t => t.Locktype)
                    .IsOptional();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TNET_LOCKUSER", DbContextHelper.GetOwnerByTableName("TNET_LOCKUSER"));
            this.Property(t => t.Nodecode).HasColumnName("NODECODE");
            this.Property(t => t.Locktime).HasColumnName("LOCKTIME");
            this.Property(t => t.Unlocktime).HasColumnName("UNLOCKTIME");
            this.Property(t => t.Locktype).HasColumnName("LOCKTYPE");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
                  }
    }
}
