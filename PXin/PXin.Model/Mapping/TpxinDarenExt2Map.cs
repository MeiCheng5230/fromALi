using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinDarenExt2Map : EntityTypeConfiguration<TpxinDarenExt2>
    {
        public TpxinDarenExt2Map()
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
            this.Property(t => t.Showname)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Education)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Pic)
                    .IsOptional()
                    .HasMaxLength(1000);
            this.Property(t => t.Fromtime)
                    .IsRequired();
            this.Property(t => t.Endtime)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Subject)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TPXIN_DAREN_EXT2", DbContextHelper.GetOwnerByTableName("TPXIN_DAREN_EXT2"));
            this.Property(t => t.Extid).HasColumnName("EXTID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Showname).HasColumnName("SHOWNAME");
            this.Property(t => t.Education).HasColumnName("EDUCATION");
            this.Property(t => t.Pic).HasColumnName("PIC");
            this.Property(t => t.Fromtime).HasColumnName("FROMTIME");
            this.Property(t => t.Endtime).HasColumnName("ENDTIME");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Subject).HasColumnName("SUBJECT");
                  }
    }
}
