using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatGroupMap : EntityTypeConfiguration<TchatGroup>
    {
        public TchatGroupMap()
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
            this.Property(t => t.Grouptype)
                    .IsRequired();
            this.Property(t => t.Transferid)
                    .IsRequired();
            this.Property(t => t.Usergradelevel)
                    .IsRequired();
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
            this.Property(t => t.PersonCount)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TCHAT_GROUP", DbContextHelper.GetOwnerByTableName("TCHAT_GROUP"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Groupname).HasColumnName("GROUPNAME");
            this.Property(t => t.Descript).HasColumnName("DESCRIPT");
            this.Property(t => t.Creater).HasColumnName("CREATER");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Groupstate).HasColumnName("GROUPSTATE");
            this.Property(t => t.Dismisstime).HasColumnName("DISMISSTIME");
            this.Property(t => t.Grouptype).HasColumnName("GROUPTYPE");
            this.Property(t => t.Transferid).HasColumnName("TRANSFERID");
            this.Property(t => t.Usergradelevel).HasColumnName("USERGRADELEVEL");
            this.Property(t => t.Grouppic).HasColumnName("GROUPPIC");
            this.Property(t => t.Groupcode).HasColumnName("GROUPCODE");
            this.Property(t => t.Auditstate).HasColumnName("AUDITSTATE");
            this.Property(t => t.Authstate).HasColumnName("AUTHSTATE");
            this.Property(t => t.PersonCount).HasColumnName("PERSONCOUNT");
        }
    }
}
