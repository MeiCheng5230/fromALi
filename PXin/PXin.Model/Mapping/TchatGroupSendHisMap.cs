using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatGroupSendHisMap : EntityTypeConfiguration<TchatGroupSendHis>
    {
        public TchatGroupSendHisMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Nodeid)
                .IsRequired();
            this.Property(t => t.Groupid)
                .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TCHAT_GROUP_SEND_HIS", DbContextHelper.GetOwnerByTableName("TCHAT_GROUP_SEND_HIS"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Groupid).HasColumnName("GROUPID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
        }
    }
}
