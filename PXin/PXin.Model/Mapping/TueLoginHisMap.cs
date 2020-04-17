using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TueLoginHisMap : EntityTypeConfiguration<TueLoginHis>
    {
        public TueLoginHisMap()
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
            this.Property(t => t.Clientid)
                    .IsRequired();
            this.Property(t => t.Version)
                    .IsRequired()
                    .HasMaxLength(400);
            this.Property(t => t.Sid)
                    .IsRequired();
            this.Property(t => t.Token)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Accesskeyid)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Timestamp)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Signaturenonce)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Signature)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Longitude)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Latitude)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Ip)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(400);

            // Table & Column Mappings
            this.ToTable("TUE_LOGIN_HIS", DbContextHelper.GetOwnerByTableName("TUE_LOGIN_HIS"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Clientid).HasColumnName("CLIENTID");
            this.Property(t => t.Version).HasColumnName("VERSION");
            this.Property(t => t.Sid).HasColumnName("SID");
            this.Property(t => t.Token).HasColumnName("TOKEN");
            this.Property(t => t.Accesskeyid).HasColumnName("ACCESSKEYID");
            this.Property(t => t.Timestamp).HasColumnName("TIMESTAMP");
            this.Property(t => t.Signaturenonce).HasColumnName("SIGNATURENONCE");
            this.Property(t => t.Signature).HasColumnName("SIGNATURE");
            this.Property(t => t.Longitude).HasColumnName("LONGITUDE");
            this.Property(t => t.Latitude).HasColumnName("LATITUDE");
            this.Property(t => t.Ip).HasColumnName("IP");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
