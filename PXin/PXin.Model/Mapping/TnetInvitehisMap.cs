using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class TnetInvitehisMap : EntityTypeConfiguration<TnetInvitehis>
    {
        public TnetInvitehisMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Sid)
                .IsRequired();
            this.Property(t => t.Pnodeid)
                    .IsRequired();
            this.Property(t => t.Mobileno)
                    .IsRequired()
                    .HasMaxLength(20);
            this.Property(t => t.Transferid)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Status)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TNET_INVITEHIS", DbContextHelper.GetOwnerByTableName("TNET_INVITEHIS"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Sid).HasColumnName("SID");
            this.Property(t => t.Pnodeid).HasColumnName("PNODEID");
            this.Property(t => t.Mobileno).HasColumnName("MOBILENO");
            this.Property(t => t.Transferid).HasColumnName("TRANSFERID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Status).HasColumnName("STATUS");
        }
    }
}
