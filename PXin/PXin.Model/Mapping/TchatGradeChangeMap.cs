using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatGradeChangeMap : EntityTypeConfiguration<TchatGradeChange>
    {
        public TchatGradeChangeMap()
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
            this.Property(t => t.Fromgrade)
                    .IsRequired();
            this.Property(t => t.Tograde)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TCHAT_GRADE_CHANGE", DbContextHelper.GetOwnerByTableName("TCHAT_GRADE_CHANGE"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Fromgrade).HasColumnName("FROMGRADE");
            this.Property(t => t.Tograde).HasColumnName("TOGRADE");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
                  }
    }
}
