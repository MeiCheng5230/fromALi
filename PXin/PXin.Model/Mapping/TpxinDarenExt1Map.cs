using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinDarenExt1Map : EntityTypeConfiguration<TpxinDarenExt1>
    {
        public TpxinDarenExt1Map()
        {
            // Primary Key
            this.HasKey(t => t.Extid);
            // Properties
            this.Property(t => t.Extid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Ptypeid)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TPXIN_DAREN_EXT1", DbContextHelper.GetOwnerByTableName("TPXIN_DAREN_EXT1"));
            this.Property(t => t.Extid).HasColumnName("EXTID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Ptypeid).HasColumnName("PTYPEID");
                  }
    }
}
