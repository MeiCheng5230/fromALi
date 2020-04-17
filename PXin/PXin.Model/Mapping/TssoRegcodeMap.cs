using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TssoRegcodeMap : EntityTypeConfiguration<TssoRegcode>
    {
        public TssoRegcodeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Regcode)
                    .IsRequired()
                    .HasMaxLength(50);
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Authcode)
                    .IsRequired()
                    .HasMaxLength(50);
            this.Property(t => t.Codetype)
                    .IsRequired();
            this.Property(t => t.Indate)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(250);
            this.Property(t => t.Introducer)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.Appstoreid)
                    .IsOptional();
            this.Property(t => t.Sourceip)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.Regtype)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TSSO_REGCODE", DbContextHelper.GetOwnerByTableName("TSSO_REGCODE"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Regcode).HasColumnName("REGCODE");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Authcode).HasColumnName("AUTHCODE");
            this.Property(t => t.Codetype).HasColumnName("CODETYPE");
            this.Property(t => t.Indate).HasColumnName("INDATE");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Introducer).HasColumnName("INTRODUCER");
            this.Property(t => t.Appstoreid).HasColumnName("APPSTOREID");
            this.Property(t => t.Sourceip).HasColumnName("SOURCEIP");
            this.Property(t => t.Regtype).HasColumnName("REGTYPE");
                  }
    }
}
