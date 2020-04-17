using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TbossMeetinfoMap : EntityTypeConfiguration<TbossMeetinfo>
    {
        public TbossMeetinfoMap()
        {
            // Primary Key
            this.HasKey(t => t.Infoid);
            // Properties
            this.Property(t => t.Infoid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Starttime)
                    .IsRequired();
            this.Property(t => t.Address)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Detail)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Opnoded)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TBOSS_MEETINFO", DbContextHelper.GetOwnerByTableName("TBOSS_MEETINFO"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Title).HasColumnName("TITLE");
            this.Property(t => t.Starttime).HasColumnName("STARTTIME");
            this.Property(t => t.Address).HasColumnName("ADDRESS");
            this.Property(t => t.Detail).HasColumnName("DETAIL");
            this.Property(t => t.Opnoded).HasColumnName("OPNODED");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
