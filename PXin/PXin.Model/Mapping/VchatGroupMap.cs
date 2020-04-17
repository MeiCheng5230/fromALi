using Common.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Model.Mapping
{
    public class VchatGroupMap : EntityTypeConfiguration<VchatGroup>
    {
        public VchatGroupMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Groupname)
                .IsRequired()
                .HasMaxLength(100);
            this.Property(t => t.Descript)
                    .IsOptional()
                    .HasMaxLength(500);
            this.Property(t => t.Creater)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Groupstate)
                    .IsRequired();
            this.Property(t => t.Dismisstime)
                    .IsOptional();
            this.Property(t => t.Grouppic)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Groupcode)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Auditstate)
                    .IsRequired();
            this.Property(t => t.Authstate)
                    .IsRequired();
            this.Property(t => t.Personcount)
                  .IsRequired();
            this.Property(t => t.Grouptype)
                 .IsRequired();
            this.Property(t => t.Createnodecode)
                   .IsOptional()
                   .HasMaxLength(100);
            this.Property(t => t.Createnodename)
                  .IsOptional()
                  .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("VCHAT_GROUP", DbContextHelper.GetOwnerByTableName("VCHAT_GROUP"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Groupname).HasColumnName("GROUPNAME");
            this.Property(t => t.Descript).HasColumnName("DESCRIPT");
            this.Property(t => t.Creater).HasColumnName("CREATER");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Groupstate).HasColumnName("GROUPSTATE");
            this.Property(t => t.Dismisstime).HasColumnName("DISMISSTIME");
            this.Property(t => t.Grouppic).HasColumnName("GROUPPIC");
            this.Property(t => t.Groupcode).HasColumnName("GROUPCODE");
            this.Property(t => t.Auditstate).HasColumnName("AUDITSTATE");
            this.Property(t => t.Authstate).HasColumnName("AUTHSTATE");

            this.Property(t => t.Personcount).HasColumnName("PERSONCOUNT");
            this.Property(t => t.Grouptype).HasColumnName("GROUPTYPE");
            this.Property(t => t.Createnodecode).HasColumnName("CREATENODECODE");
            this.Property(t => t.Createnodename).HasColumnName("CREATENODENAME");

            this.Ignore(t => t.Grouppicfull);
        }

    }
}
