using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TsmsAlitemplateMap : EntityTypeConfiguration<TsmsAlitemplate>
    {
        public TsmsAlitemplateMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Tmpcode)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Tmpcnt)
                    .IsRequired()
                    .HasMaxLength(200);
            this.Property(t => t.Smssign)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Tmpval)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.AccessKeyId)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.AccessKeySecret)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TSMS_ALITEMPLATE", DbContextHelper.GetOwnerByTableName("TSMS_ALITEMPLATE"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Tmpcode).HasColumnName("TMPCODE");
            this.Property(t => t.Tmpcnt).HasColumnName("TMPCNT");
            this.Property(t => t.Smssign).HasColumnName("SMSSIGN");
            this.Property(t => t.Tmpval).HasColumnName("TMPVAL");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.AccessKeyId).HasColumnName("ACCESS_KEY_ID");
            this.Property(t => t.AccessKeySecret).HasColumnName("ACCESS_KEY_SECRET");
                  }
    }
}
