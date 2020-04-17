using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class TnetUserAgreementMap : EntityTypeConfiguration<TnetUserAgreement>
    {
        public TnetUserAgreementMap()
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
            this.Property(t => t.Type)
                    .IsRequired();
            this.Property(t => t.Agreed)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Version)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TNET_USER_AGREEMENT", DbContextHelper.GetOwnerByTableName("TNET_USER_AGREEMENT"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Type).HasColumnName("TYPE");
            this.Property(t => t.Agreed).HasColumnName("AGREED");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Version).HasColumnName("VERSION");
                  }
    }
}
