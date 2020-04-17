using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TblUserJxsXfMap : EntityTypeConfiguration<TblUserJxsXf>
    {
        public TblUserJxsXfMap()
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
            this.Property(t => t.Infoid)
                    .IsRequired();
            this.Property(t => t.Amount)
                    .IsRequired()
                    .HasPrecision(13, 2 );
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TBL_USER_JXS_XF", DbContextHelper.GetOwnerByTableName("TBL_USER_JXS_XF"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
