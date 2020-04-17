using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinDarenBrowseHisMap : EntityTypeConfiguration<TpxinDarenBrowseHis>
    {
        public TpxinDarenBrowseHisMap()
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
            this.Property(t => t.Pnodeid)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Modifytime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Videoid)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TPXIN_DAREN_BROWSE_HIS", DbContextHelper.GetOwnerByTableName("TPXIN_DAREN_BROWSE_HIS"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Pnodeid).HasColumnName("PNODEID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Modifytime).HasColumnName("MODIFYTIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Videoid).HasColumnName("VIDEOID");
                  }
    }
}
