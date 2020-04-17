using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Common.Mvc;

namespace PXin.Model.Mapping
{
    public class TzcIdcardrecLogMap : EntityTypeConfiguration<TzcIdcardrecLog>
    {
        public TzcIdcardrecLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Pic)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.Recresult)
                    .IsRequired()
                    .HasMaxLength(1000);
            this.Property(t => t.Createtime)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TZC_IDCARDREC_LOG", DbContextHelper.GetOwnerByTableName("TZC_IDCARDREC_LOG"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Pic).HasColumnName("PIC");
            this.Property(t => t.Recresult).HasColumnName("RECRESULT");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
        }
    }
}
