using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinActivityMap : EntityTypeConfiguration<TpxinActivity>
    {
        public TpxinActivityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.ActivityName)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.ActivityStarttime)
                    .IsRequired();
            this.Property(t => t.ActivityEndtime)
                    .IsRequired();
            this.Property(t => t.PayStarttime)
                    .IsRequired();
            this.Property(t => t.PayEndtime)
                    .IsRequired();
            this.Property(t => t.Cover)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TPXIN_ACTIVITY", DbContextHelper.GetOwnerByTableName("TPXIN_ACTIVITY"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.ActivityName).HasColumnName("ACTIVITY_NAME");
            this.Property(t => t.ActivityStarttime).HasColumnName("ACTIVITY_STARTTIME");
            this.Property(t => t.ActivityEndtime).HasColumnName("ACTIVITY_ENDTIME");
            this.Property(t => t.PayStarttime).HasColumnName("PAY_STARTTIME");
            this.Property(t => t.PayEndtime).HasColumnName("PAY_ENDTIME");
            this.Property(t => t.Cover).HasColumnName("COVER");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
