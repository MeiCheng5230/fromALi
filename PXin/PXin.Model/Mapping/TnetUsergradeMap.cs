using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TnetUsergradeMap : EntityTypeConfiguration<TnetUsergrade>
    {
        public TnetUsergradeMap()
        {
            // Primary Key
            this.HasKey(t => t.Idno);

            // Properties
            this.Property(t => t.Idno)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Nodeid)
                .IsRequired();
            this.Property(t => t.Gradeid)
                .IsRequired();
            this.Property(t => t.Createtime)
                .IsOptional();
            this.Property(t => t.Sid)
                .IsRequired();
            this.Property(t => t.Typeid)
                .IsRequired();
            this.Property(t => t.Rate)
                .IsRequired()
                .HasPrecision(16, 5);

            // Table & Column Mappings
            this.ToTable("TNET_USERGRADE", DbContextHelper.GetOwnerByTableName("TNET_USERGRADE"));
            this.Property(t => t.Idno).HasColumnName("IDNO");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Gradeid).HasColumnName("GRADEID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Sid).HasColumnName("SID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Rate).HasColumnName("RATE");
        }
    }
}
