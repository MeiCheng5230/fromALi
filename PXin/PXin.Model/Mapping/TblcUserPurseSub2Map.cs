using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class TblcUserPurseSub2Map : EntityTypeConfiguration<TblcUserPurseSub2>
    {
        public TblcUserPurseSub2Map()
        {
            // Primary Key
            this.HasKey(t => t.Infoid);
            // Properties
            this.Property(t => t.Infoid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Purseid)
                .IsRequired();
            this.Property(t => t.Subpurseid)
                    .IsRequired();
            this.Property(t => t.Pursecode)
                    .IsRequired()
                    .HasMaxLength(400);
            this.Property(t => t.Pursename)
                    .IsRequired()
                    .HasMaxLength(400);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Typeid)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TBLC_USER_PURSE_SUB2", DbContextHelper.GetOwnerByTableName("TBLC_USER_PURSE_SUB2"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Purseid).HasColumnName("PURSEID");
            this.Property(t => t.Subpurseid).HasColumnName("SUBPURSEID");
            this.Property(t => t.Pursecode).HasColumnName("PURSECODE");
            this.Property(t => t.Pursename).HasColumnName("PURSENAME");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
        }
    }
}
