using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TblUserJxsChangeMap : EntityTypeConfiguration<TblUserJxsChange>
    {
        public TblUserJxsChangeMap()
        {
            // Primary Key
            this.HasKey(t => t.Hisid);
            // Properties
            this.Property(t => t.Hisid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Infoid)
                    .IsRequired();
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Fromstatus)
                    .IsRequired();
            this.Property(t => t.Endstatus)
                    .IsRequired();
            this.Property(t => t.Note)
                    .IsRequired()
                    .HasMaxLength(400);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(400);

            // Table & Column Mappings
            this.ToTable("TBL_USER_JXS_CHANGE", DbContextHelper.GetOwnerByTableName("TBL_USER_JXS_CHANGE"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Fromstatus).HasColumnName("FROMSTATUS");
            this.Property(t => t.Endstatus).HasColumnName("ENDSTATUS");
            this.Property(t => t.Note).HasColumnName("NOTE");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
