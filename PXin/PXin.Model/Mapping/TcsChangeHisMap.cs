using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TcsChangeHisMap : EntityTypeConfiguration<TcsChangeHis>
    {
        public TcsChangeHisMap()
        {
            // Primary Key
            this.HasKey(t => t.Hisid);
            // Properties
            this.Property(t => t.Hisid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Olddata)
                    .IsOptional()
                    .HasMaxLength(4000);
            this.Property(t => t.Newdata)
                    .IsOptional()
                    .HasMaxLength(4000);
            this.Property(t => t.Fee)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Opnodeid)
                    .IsOptional();
            this.Property(t => t.Note)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(400);

            // Table & Column Mappings
            this.ToTable("TCS_CHANGE_HIS", DbContextHelper.GetOwnerByTableName("TCS_CHANGE_HIS"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Olddata).HasColumnName("OLDDATA");
            this.Property(t => t.Newdata).HasColumnName("NEWDATA");
            this.Property(t => t.Fee).HasColumnName("FEE");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Opnodeid).HasColumnName("OPNODEID");
            this.Property(t => t.Note).HasColumnName("NOTE");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
