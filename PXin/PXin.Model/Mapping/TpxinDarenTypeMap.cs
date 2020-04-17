using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinDarenTypeMap : EntityTypeConfiguration<TpxinDarenType>
    {
        public TpxinDarenTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Typeid);
            // Properties
            this.Property(t => t.Typeid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Ptypeid)
                    .IsRequired();
            this.Property(t => t.Typename)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Pic)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Status)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TPXIN_DAREN_TYPE", DbContextHelper.GetOwnerByTableName("TPXIN_DAREN_TYPE"));
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Ptypeid).HasColumnName("PTYPEID");
            this.Property(t => t.Typename).HasColumnName("TYPENAME");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Pic).HasColumnName("PIC");
            this.Property(t => t.Status).HasColumnName("STATUS");
                  }
    }
}
