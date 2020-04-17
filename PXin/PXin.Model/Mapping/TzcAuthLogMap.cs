using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Common.Mvc;

namespace PXin.Model.Mapping
{
    public class TzcAuthLogMap : EntityTypeConfiguration<TzcAuthLog>
    {
        public TzcAuthLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Infoid);
            // Properties
            this.Property(t => t.Infoid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Nodeid)
                .IsRequired();
            this.Property(t => t.Realname)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Sex)
                    .IsOptional();
            this.Property(t => t.Birthday)
                    .IsOptional();
            this.Property(t => t.Idcard)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Idcardpic1)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Idcardpic2)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Auditnodeid)
                    .IsOptional();
            this.Property(t => t.Audittime)
                    .IsOptional();
            this.Property(t => t.Payment)
                    .IsRequired();
            this.Property(t => t.Isidentify)
                    .IsRequired();
            this.Property(t => t.Race)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Address)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.IssuedBy)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.ValidDate)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Idcardpic3)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Baiduapiret)
                    .IsOptional()
                    .HasMaxLength(400);

            // Table & Column Mappings
            this.ToTable("TZC_AUTH_LOG", DbContextHelper.GetOwnerByTableName("TZC_AUTH_LOG"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Realname).HasColumnName("REALNAME");
            this.Property(t => t.Sex).HasColumnName("SEX");
            this.Property(t => t.Birthday).HasColumnName("BIRTHDAY");
            this.Property(t => t.Idcard).HasColumnName("IDCARD");
            this.Property(t => t.Idcardpic1).HasColumnName("IDCARDPIC1");
            this.Property(t => t.Idcardpic2).HasColumnName("IDCARDPIC2");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Auditnodeid).HasColumnName("AUDITNODEID");
            this.Property(t => t.Audittime).HasColumnName("AUDITTIME");
            this.Property(t => t.Payment).HasColumnName("PAYMENT");
            this.Property(t => t.Isidentify).HasColumnName("ISIDENTIFY");
            this.Property(t => t.Race).HasColumnName("RACE");
            this.Property(t => t.Address).HasColumnName("ADDRESS");
            this.Property(t => t.IssuedBy).HasColumnName("ISSUED_BY");
            this.Property(t => t.ValidDate).HasColumnName("VALID_DATE");
            this.Property(t => t.Idcardpic3).HasColumnName("IDCARDPIC3");
            this.Property(t => t.Baiduapiret).HasColumnName("BAIDUAPIRET");
        }
    }
}
