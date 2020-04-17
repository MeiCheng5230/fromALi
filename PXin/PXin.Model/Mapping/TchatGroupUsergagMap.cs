using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatGroupUsergagMap : EntityTypeConfiguration<TchatGroupUsergag>
    {
        public TchatGroupUsergagMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Createtime)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Groupid)
                .IsRequired();
            this.Property(t => t.Userid)
                .IsRequired();
            this.Property(t => t.Minute)
                .IsRequired();
            this.Property(t => t.Createtime)
                .IsRequired();
            this.Property(t => t.Optnodeid)
                .IsRequired();
            this.Property(t => t.Status)
                .IsRequired();
            this.Property(t => t.Canceltime)
                .IsOptional();

            // Table & Column Mappings
            this.ToTable("TCHAT_GROUP_USERGAG", DbContextHelper.GetOwnerByTableName("TCHAT_GROUP_USERGAG"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Groupid).HasColumnName("GROUPID");
            this.Property(t => t.Userid).HasColumnName("USERID");
            this.Property(t => t.Minute).HasColumnName("MINUTE");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Optnodeid).HasColumnName("OPTNODEID");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Canceltime).HasColumnName("CANCELTIME");
        }
    }
}
